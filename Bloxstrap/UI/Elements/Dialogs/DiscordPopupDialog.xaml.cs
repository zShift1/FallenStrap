using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

using Bloxstrap.UI.ViewModels;

namespace Bloxstrap.UI.Elements.Dialogs
{
    /// <summary>
    /// Interaction logic for DiscordPopupDialog.xaml
    /// </summary>
    public partial class DiscordPopupDialog
    {
        private readonly DispatcherTimer _closeTimer;

        public DiscordPopupDialog()
        {
            InitializeComponent();

            _closeTimer = new DispatcherTimer
            {
                Interval = System.TimeSpan.FromSeconds(5)
            };
            _closeTimer.Tick += (_, _) =>
            {
                _closeTimer.Stop();
                Close();
            };

            Loaded += (_, _) =>
            {
                // drain the countdown bar over the 5 seconds
                var animation = new DoubleAnimation(100, 0, System.TimeSpan.FromSeconds(5));
                CountdownBar.BeginAnimation(ProgressBar.ValueProperty, animation);

                _closeTimer.Start();
            };
        }

        private void OpenDiscordButton_Click(object sender, RoutedEventArgs e)
        {
            _closeTimer.Stop();
            GlobalViewModel.OpenWebpageCommand.Execute("https://discord.gg/Dxg8Ayj4RY");
            Close();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            _closeTimer.Stop();
            Close();
        }
    }
}