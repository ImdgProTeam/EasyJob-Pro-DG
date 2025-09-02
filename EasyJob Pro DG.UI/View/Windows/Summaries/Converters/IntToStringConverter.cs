using EasyJob_ProDG.UI.View.Converters;
using System;
using System.Globalization;

namespace EasyJob_ProDG.UI.View.DialogWindows.Summaries.Converters
{
    /// <summary>
    /// Converts int value into string with added text from <see cref="parameter"/>.
    /// Use single quotes (') for parameter text in wpf markup.
    /// </summary>
    internal class IntToStringConverter : ConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int intValue)
            {
                return $"{intValue:## ##0} {parameter?.ToString()}";
            }
            return value;
        }
    }
}
