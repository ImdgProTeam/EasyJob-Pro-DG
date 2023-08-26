using System;
using System.Globalization;
using System.Windows.Data;

namespace EasyJob_ProDG.UI.View.Converters
{
    [ValueConversion(typeof(string),typeof(bool))]
    public class ZeroNullStringToFalseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var text = value as string;
            if (string.IsNullOrEmpty(text) || text == "0")
            {
                return false;
            }
            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
