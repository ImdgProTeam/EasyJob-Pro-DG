using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EasyJob_ProDG.UI.View.DialogWindows.Summaries.Converters
{
    internal class UpdateReportBlockVisibilityMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length < 2) return Visibility.Visible;
            if (values[0] is not int || values[1] is not bool) return Visibility.Visible;

            int value1 = (int)values?[0];
            bool value2 = (bool)values?[1];

            if (value1 == 0 && !value2)
                return Visibility.Collapsed;
            return Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
