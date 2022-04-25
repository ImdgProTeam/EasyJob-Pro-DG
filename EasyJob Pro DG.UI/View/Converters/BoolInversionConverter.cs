using System;
using System.Globalization;
using System.Windows.Data;

namespace EasyJob_ProDG.UI.View.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    internal class BoolInversionConverter : ConverterBase
    {
        private bool parsedValue;

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string directValue = value.ToString();
            if (directValue == null || !Boolean.TryParse(directValue, out parsedValue))
            {
                throw new ArgumentException("value", "Must be boolean type");
            }

            return !parsedValue;

        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}
