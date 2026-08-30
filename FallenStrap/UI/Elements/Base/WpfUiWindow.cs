using System.Windows;
using System.Windows.Interop;
using Microsoft.Win32;
using Wpf.Ui.Appearance;
using Wpf.Ui.Controls;
using Wpf.Ui.Mvvm.Contracts;
using Wpf.Ui.Mvvm.Services;

namespace FallenStrap.UI.Elements.Base
{
    public abstract class WpfUiWindow : UiWindow
    {
        private readonly IThemeService _themeService = new ThemeService();

        public WpfUiWindow()
        {
            ApplyTheme();
        }

        public void ApplyTheme()
        {
            const int customThemeIndex = 2; // index for CustomTheme merged dictionary

            ThemeType themeType = App.Settings.Prop.Theme switch
            {
                Enums.Theme.Light => ThemeType.Light,
                Enums.Theme.Dark => ThemeType.Dark,
                _ => DetectSystemTheme()
            };

            _themeService.SetTheme(themeType);
            _themeService.SetSystemAccent();

            // there doesn't seem to be a way to query the name for merged dictionaries
            string styleSource = themeType == ThemeType.Light ? "pack://application:,,,/UI/Style/Light.xaml" : "pack://application:,,,/UI/Style/Dark.xaml";
            var dict = new ResourceDictionary { Source = new Uri(styleSource) };
            Application.Current.Resources.MergedDictionaries[customThemeIndex] = dict;

#if QA_BUILD
            this.BorderBrush = System.Windows.Media.Brushes.Red;
            this.BorderThickness = new Thickness(4);
#endif
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            if (App.Settings.Prop.WPFSoftwareRender || App.LaunchSettings.NoGPUFlag.Active)
            {
                if (PresentationSource.FromVisual(this) is HwndSource hwndSource)
                    hwndSource.CompositionTarget.RenderMode = RenderMode.SoftwareOnly;
            }

            base.OnSourceInitialized(e);
        }

        private static ThemeType DetectSystemTheme()
        {
            try
            {
                using RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize");

                if (key?.GetValue("AppsUseLightTheme") is int value && value == 1)
                    return ThemeType.Light;
            }
            catch (Exception ex)
            {
                App.Logger.WriteException("WpfUiWindow::DetectSystemTheme", ex);
            }

            return ThemeType.Dark;
        }
    }
}
