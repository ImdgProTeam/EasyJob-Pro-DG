using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace EasyJob_ProDG.UI.View.Converters
{
    [ValueConversion(typeof(string), typeof(string))]
    [MarkupExtensionReturnType(typeof(AnyCellToNullConverter))]
    internal class AnyCellToNullConverter : ConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string text = (string)value;
            if(text == "bay: any row: any tier: any " || string.IsNullOrEmpty(text))
            {
                return null;
            }
            return text;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
