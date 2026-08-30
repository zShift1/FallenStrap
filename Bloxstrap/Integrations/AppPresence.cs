using DiscordRPC;

namespace Bloxstrap.Integrations
{
    public static class AppPresence
    {
        private const string ClientId = "1543151617782714399";

        private static DiscordRpcClient? _rpcClient;

        private static DiscordRpcClient GetClient()
        {
            if (_rpcClient is null)
            {
                _rpcClient = new DiscordRpcClient(ClientId);

                _rpcClient.OnReady += (_, e) => App.Logger.WriteLine("AppPresence", $"Received ready from user {e.User}");
                _rpcClient.OnError += (_, e) => App.Logger.WriteLine("AppPresence", $"An RPC error occurred - {e.Message}");
                _rpcClient.OnClose += (_, e) => App.Logger.WriteLine("AppPresence", $"Connection closed - {e.Reason}");

                try
                {
                    _rpcClient.Initialize();
                }
                catch (Exception ex)
                {
                    App.Logger.WriteLine("AppPresence", "Failed to initialize Discord RPC (is Discord Desktop running?)");
                    App.Logger.WriteException("AppPresence", ex);
                }
            }

            return _rpcClient;
        }

        public static void Start()
        {
            if (!App.Settings.Prop.UseDiscordRichPresence)
                return;

            // if Roblox is already running, the in-game presence handles Discord
            if (Process.GetProcessesByName("RobloxPlayerBeta").Length > 0)
                return;

            App.Logger.WriteLine("AppPresence", "Setting app presence");

            GetClient().SetPresence(new DiscordRPC.RichPresence
            {
                Details = "Explorando la configuración",
                State = $"FallenStrap v{App.Version}",
                Timestamps = new Timestamps { Start = DateTime.UtcNow }
            });
        }

        public static void Stop()
        {
            if (_rpcClient is null)
                return;

            App.Logger.WriteLine("AppPresence", "Clearing app presence");
            _rpcClient.ClearPresence();
            _rpcClient.Dispose();
            _rpcClient = null;
        }
    }
}