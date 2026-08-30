using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using CommunityToolkit.Mvvm.Input;

using FallenStrap.UI.Elements.Settings;

namespace FallenStrap.UI.ViewModels.Settings
{
    public class HomeViewModel : NotifyPropertyChangedViewModel
    {
        private readonly Page _page;

        public const string FpsCapFlag = "DFIntTaskSchedulerTargetFps";

        public const string GraphicsQualityFlag = "DFIntDebugFRMQualityLevelOverride";

        public HomeViewModel(Page page)
        {
            _page = page;
            RefreshProfiles();
        }

        public string ProjectName => App.ProjectName;
        public string VersionText => $"v{App.Version}";
        public string RobloxStatus => App.IsPlayerInstalled ? "Roblox instalado" : "Roblox no está instalado";
        public string ActiveProfile => App.Profiles.Prop.Active;

        public string CurrentFpsCap => App.FastFlags.GetValue(FpsCapFlag) is string fps ? $"{fps} FPS" : "60 FPS (predeterminado)";

        public string CurrentGraphicsQuality
        {
            get
            {
                if (App.FastFlags.GetValue(GraphicsQualityFlag) is not string value)
                    return "Predeterminada";

                foreach (var pair in FastFlagManager.GraphicsQualityLevels)
                {
                    if (pair.Value == value)
                        return $"Nivel {value} ({pair.Key.ToString()})";
                }

                return $"Nivel {value} (personalizada)";
            }
        }

        public ObservableCollection<string> ProfilesList { get; } = new();

        public string? SelectedProfile { get; set; } = App.Profiles.Prop.Active;

        public string NewProfileName { get; set; } = "";

        public ICommand PlayNowCommand => new RelayCommand(PlayNow);
        public ICommand ApplyFpsCommand => new RelayCommand<string>(ApplyFps);
        public ICommand ApplyQualityCommand => new RelayCommand<string>(ApplyQuality);
        public ICommand AddProfileCommand => new RelayCommand(AddProfile);
        public ICommand SwitchProfileCommand => new RelayCommand(SwitchProfile);
        public ICommand DeleteProfileCommand => new RelayCommand(DeleteProfile);

        private void SaveAll()
        {
            App.Settings.Save();
            App.State.Save();
            App.FastFlags.Save();

            foreach (var pair in App.PendingSettingTasks)
            {
                var task = pair.Value;

                if (task.Changed)
                {
                    App.Logger.WriteLine("HomeViewModel::SaveAll", $"Executing pending task '{task}'");
                    task.Execute();
                }
            }

            App.PendingSettingTasks.Clear();
        }

        private void PlayNow()
        {
            SaveAll();

            MainWindow.LaunchOnClose = true;
            ((MainWindow)Window.GetWindow(_page)!).Close();
        }

        private void ApplyFps(string? fps)
        {
            if (String.IsNullOrEmpty(fps))
                return;

            App.FastFlags.SetValue(FpsCapFlag, fps);
            App.FastFlags.Save();

            OnPropertyChanged(nameof(CurrentFpsCap));
        }

        private void ApplyQuality(string? quality)
        {
            App.FastFlags.SetValue(GraphicsQualityFlag, String.IsNullOrEmpty(quality) ? null : quality);
            App.FastFlags.Save();

            OnPropertyChanged(nameof(CurrentGraphicsQuality));
        }

        private void AddProfile()
        {
            string name = NewProfileName.Trim();

            if (String.IsNullOrEmpty(name) || name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                Frontend.ShowMessageBox("El nombre del perfil no es válido.", MessageBoxImage.Warning);
                return;
            }

            if (App.Profiles.Exists(name))
            {
                Frontend.ShowMessageBox("Ya existe un perfil con ese nombre.", MessageBoxImage.Warning);
                return;
            }

            App.Profiles.Create(name);

            // the new profile starts as a copy of the current one
            string dir = Path.Combine(Paths.Base, "Profiles", name);
            Directory.CreateDirectory(dir);

            if (File.Exists(App.Settings.FileLocation))
                File.Copy(App.Settings.FileLocation, Path.Combine(dir, "Settings.json"), true);

            if (File.Exists(App.FastFlags.FileLocation))
                File.Copy(App.FastFlags.FileLocation, Path.Combine(dir, "ClientAppSettings.json"), true);

            NewProfileName = "";
            OnPropertyChanged(nameof(NewProfileName));

            RefreshProfiles();
        }

        private void SwitchProfile()
        {
            if (SelectedProfile is null || SelectedProfile == App.Profiles.Prop.Active || !App.Profiles.Exists(SelectedProfile))
                return;

            // save the current state into the current profile before switching
            SaveAll();

            App.Profiles.Switch(SelectedProfile);
            App.RelaunchToMenuAfterExit = true;

            App.Logger.WriteLine("HomeViewModel::SwitchProfile", $"Switching to profile '{SelectedProfile}', relaunching to menu");

            ((MainWindow)Window.GetWindow(_page)!).Close();
        }

        private void DeleteProfile()
        {
            if (SelectedProfile is null || SelectedProfile == "default" || SelectedProfile == App.Profiles.Prop.Active)
                return;

            var result = Frontend.ShowMessageBox(
                $"¿Eliminar el perfil '{SelectedProfile}'? Se borrarán también sus archivos.",
                MessageBoxImage.Warning,
                MessageBoxButton.YesNo
            );

            if (result != MessageBoxResult.Yes)
                return;

            App.Profiles.Delete(SelectedProfile);

            SelectedProfile = App.Profiles.Prop.Active;
            OnPropertyChanged(nameof(SelectedProfile));

            RefreshProfiles();
        }

        private void RefreshProfiles()
        {
            ProfilesList.Clear();

            foreach (string name in App.Profiles.Profiles)
                ProfilesList.Add(name);

            OnPropertyChanged(nameof(ActiveProfile));
        }
    }
}