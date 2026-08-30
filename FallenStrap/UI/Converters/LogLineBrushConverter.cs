using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace FallenStrap.UI.Converters
{
    /// <summary>
    /// Colors a log line based on its content: red for failures/exceptions,
    /// amber for warnings/pending changes, green for successful milestones.
    /// </summary>
    class LogLineBrushConverter : IValueConverter
    {
        private static readonly SolidColorBrush Error = new(Color.FromRgb(0xFF, 0x5C, 0x5C));
        private static readonly SolidColorBrush Warning = new(Color.FromRgb(0xFF, 0xC1, 0x5C));
        private static readonly SolidColorBrush Success = new(Color.FromRgb(0x8F, 0xD0, 0xA8));
        private static readonly SolidColorBrush Normal = new(Color.FromRgb(0xC2, 0xC2, 0xD4));

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string line = value as string ?? "";

            if (line.Contains("Exception") || line.Contains("(0x") || line.Contains("Failed to") || line.Contains(" failed"))
                return Error;

            if (line.Contains("pending") || line.Contains("Warning") || line.Contains("Cleaning up") || line.Contains("Possible duplicate"))
                return Warning;

            if (line.Contains("Successful") || line.Contains("successful") || line.Contains("Finished") || line.Contains("Save complete"))
                return Success;

            return Normal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}