using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EasyJob_ProDG.UI.View.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class InverseBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? isNotVisible = value as bool?;
            if (isNotVisible == null) return Visibility.Visible;
            if ((bool)isNotVisible) return Visibility.Collapsed;
            else return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility? visibility = value as Visibility?;
            if (visibility == null) return true;
            if (visibility == Visibility.Visible) return false;
            else return true;
        }
    }
}
