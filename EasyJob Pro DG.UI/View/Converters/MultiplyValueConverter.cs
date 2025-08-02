using System;
using System.Globalization;

namespace EasyJob_ProDG.UI.View.Converters
{
    /// <summary>
    /// Multiplies binding value by times as mentioned in converter parameter
    /// </summary>
    internal class MultiplyValueConverter : ConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null) return value;
            if (!double.TryParse((string)parameter, out double multiplier)) return value;

            if (value is int intValue)
            {
                return intValue * multiplier;
            }
            if (value is double doubleValue)
            {
                return doubleValue * multiplier;
            }

            return value;
        }
    }
}
