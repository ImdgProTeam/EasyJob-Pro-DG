using System;
using System.Globalization;
using System.Windows.Data;

namespace EasyJob_ProDG.UI.View.Converters
{
    /// <summary>
    /// Converters substracts second value from the first.
    /// Both should be double.
    /// </summary>
    internal class SubstractValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is double doubleValue && values[1] is double reducerValue)
            {
                return doubleValue - reducerValue;
            }
            return values[0];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
