using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;

namespace Bloxstrap.UI.Elements.Settings.Pages
{
    /// <summary>
    /// Interaction logic for LogViewerPage.xaml
    /// </summary>
    public partial class LogViewerPage
    {
        private readonly DispatcherTimer _timer;

        public LogViewerPage()
        {
            InitializeComponent();

            Refresh();

            _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(500) };
            _timer.Tick += (_, _) => Refresh();
            _timer.Start();

            Unloaded += (_, _) => _timer.Stop();
        }

        private void Refresh()
        {
            bool stickToBottom = LogScroll.ScrollableHeight == 0 || LogScroll.VerticalOffset >= LogScroll.ScrollableHeight - 40;

            LogItems.ItemsSource = null;
            LogItems.ItemsSource = new List<string>(App.Logger.History);

            if (stickToBottom)
                LogScroll.ScrollToEnd();
        }

        private void OpenLogsFolder(object sender, RoutedEventArgs e)
        {
            if (App.Logger.FileLocation is string file)
                Process.Start("explorer.exe", $"/select,\"{file}\"");
            else
                Process.Start("explorer.exe", Paths.Logs);
        }

        private void CopyLog(object sender, RoutedEventArgs e) =>
            Clipboard.SetText(App.Logger.AsDocument);
    }
}