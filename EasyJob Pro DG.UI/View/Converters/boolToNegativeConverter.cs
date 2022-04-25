using System;
using System.Globalization;
using System.Windows.Data;

namespace EasyJob_ProDG.UI.View.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    class boolInversionConverter : IValueConverter
    {
        private bool parsedValue;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string directValue = value.ToString();
            if (directValue == null || !Boolean.TryParse(directValue, out parsedValue))
            {
                throw new ArgumentException("value", "Must be boolean type");
            }

            return !parsedValue;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}
