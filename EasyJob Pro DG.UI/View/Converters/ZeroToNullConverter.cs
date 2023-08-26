using System;
using System.Globalization;
using System.Windows.Data;

namespace EasyJob_ProDG.UI.View.Converters
{
    [ValueConversion(typeof(byte), typeof(string))]
    class ZeroToNullConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var text = value.ToString();
            if (text == "0") text = "";
            return text?? value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var newValue = value as string;
            byte returnValue;
            byte.TryParse(newValue, out returnValue);
            return returnValue;
        }
    }
}
