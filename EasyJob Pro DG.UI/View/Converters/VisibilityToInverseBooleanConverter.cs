using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EasyJob_ProDG.UI.View.Converters
{
    [ValueConversion(typeof(Visibility),typeof(bool))]
    public class VisibilityToInverseBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility? visibility = value as Visibility?;
            if (visibility == null) return false;
            if (visibility == Visibility.Visible) return true;
            else return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? ischecked = value as bool?;
            if (ischecked == null) return Visibility.Visible;
            if ((bool)ischecked) return Visibility.Visible;
            else return Visibility.Collapsed;
        }
    }
}
