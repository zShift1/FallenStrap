using Bloxstrap.AppData;
using Bloxstrap.Integrations;
using Bloxstrap.Models;

namespace Bloxstrap
{
    public class Watcher : IDisposable
    {
        private readonly InterProcessLock _lock = new("Watcher");

        private readonly WatcherData? _watcherData;
        
        private readonly NotifyIconWrapper? _notifyIcon;

        public readonly ActivityWatcher? ActivityWatcher;

        public readonly DiscordRichPresence? RichPresence;

        public Watcher()
        {
            const string LOG_IDENT = "Watcher";

            if (!_lock.IsAcquired)
            {
                App.Logger.WriteLine(LOG_IDENT, "Watcher instance already exists");
                return;
            }

            string? watcherDataArg = App.LaunchSettings.WatcherFlag.Data;

            if (String.IsNullOrEmpty(watcherDataArg))
            {
#if DEBUG
                string path = new RobloxPlayerData().ExecutablePath;
                if (!File.Exists(path))
                    throw new ApplicationException("Roblox player is not been installed");

                using var gameClientProcess = Process.Start(path);

                _watcherData = new() { ProcessId = gameClientProcess.Id };
#else
                throw new Exception("Watcher data not specified");
#endif
            }
            else
            {
                _watcherData = JsonSerializer.Deserialize<WatcherData>(Encoding.UTF8.GetString(Convert.FromBase64String(watcherDataArg)));
            }

            if (_watcherData is null)
                throw new Exception("Watcher data is invalid");

            if (App.Settings.Prop.EnableActivityTracking)
            {
                ActivityWatcher = new(_watcherData.LogFile);

                if (App.Settings.Prop.UseDisableAppPatch)
                {
                    ActivityWatcher.OnAppClose += delegate
                    {
                        App.Logger.WriteLine(LOG_IDENT, "Received desktop app exit, closing Roblox");
                        using var process = Process.GetProcessById(_watcherData.ProcessId);
                        process.CloseMainWindow();
                    };
                }

                if (App.Settings.Prop.UseDiscordRichPresence)
                    RichPresence = new(ActivityWatcher);
            }

            _notifyIcon = new(this);
        }

        public void KillRobloxProcess() => CloseProcess(_watcherData!.ProcessId, true);

        public void CloseProcess(int pid, bool force = false)
        {
            const string LOG_IDENT = "Watcher::CloseProcess";

            try
            {
                using var process = Process.GetProcessById(pid);

                App.Logger.WriteLine(LOG_IDENT, $"Killing process '{process.ProcessName}' (pid={pid}, force={force})");

                if (process.HasExited)
                {
                    App.Logger.WriteLine(LOG_IDENT, $"PID {pid} has already exited");
                    return;
                }

                if (force)
                    process.Kill();
                else
                    process.CloseMainWindow();
            }
            catch (Exception ex)
            {
                App.Logger.WriteLine(LOG_IDENT, $"PID {pid} could not be closed");
                App.Logger.WriteException(LOG_IDENT, ex);
            }
        }

        public async Task Run()
        {
            if (!_lock.IsAcquired || _watcherData is null)
                return;

            ActivityWatcher?.Start();

            while (IsRobloxProcessAlive())
                await Task.Delay(1000);

            if (_watcherData.AutoclosePids is not null)
            {
                foreach (int pid in _watcherData.AutoclosePids)
                    CloseProcess(pid);
            }

            if (App.LaunchSettings.TestModeFlag.Active)
                Process.Start(Paths.Process, "-settings -testmode");
        }

        private bool IsRobloxProcessAlive()
        {
            const string LOG_IDENT = "Watcher::IsRobloxProcessAlive";

            // the game client is expected to stay alive for the whole session, so finding it by PID
            // is the primary check. this can fail for a variety of reasons (the client relaunching
            // itself after an update, Process.GetProcesses() throwing during enumeration, etc.), so
            // we fall back to checking whether the client's log file is still being written to, and
            // ultimately whether any player/studio client is running at all.
            try
            {
                var processes = Utilities.GetProcessesSafe();

                if (processes.Any(x => x.Id == _watcherData!.ProcessId))
                    return true;

                if (_watcherData!.LogFile is not null)
                {
                    var logInfo = new FileInfo(_watcherData.LogFile);

                    // Roblox writes to its log frequently while running; anything under ~10 seconds means it's alive
                    if (logInfo.Exists && DateTime.UtcNow - logInfo.LastWriteTimeUtc < TimeSpan.FromSeconds(10))
                        return true;
                }

                if (processes.Any(x => x.ProcessName is "RobloxPlayerBeta" or "RobloxStudioBeta"))
                {
                    App.Logger.WriteLine(LOG_IDENT, $"PID {_watcherData.ProcessId} not found in process list, but a client process is running; assuming the game is alive");
                    return true;
                }

                App.Logger.WriteLine(LOG_IDENT, $"PID {_watcherData.ProcessId} not found in process list, log file is stale, and no client processes are running");
            }
            catch (Exception ex)
            {
                // if we can't even check, assume the game is still running rather than killing the watcher early
                App.Logger.WriteException(LOG_IDENT, ex);
                return true;
            }

            return false;
        }

        public void Dispose()
        {
            App.Logger.WriteLine("Watcher::Dispose", "Disposing Watcher");

            _notifyIcon?.Dispose();
            RichPresence?.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
