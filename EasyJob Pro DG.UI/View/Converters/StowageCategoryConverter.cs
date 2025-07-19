using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace EasyJob_ProDG.UI.View.Converters
{
    [ValueConversion(typeof(char), typeof(string))]
    [MarkupExtensionReturnType(typeof(StowageCategoryConverter))]
    internal class StowageCategoryConverter : ConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            char input = (char)value;
            if(char.IsLetter(input)) 
                return input.ToString();
            switch (input)
            {
                //case 'A':
                //    return "A";
                //case 'B':
                //    return "B";
                //case 'C':
                //    return "C";
                //case 'D':
                //    return "D";
                //case 'E':
                //    return "E";
                case '1':
                    return "01";
                case '2':
                    return "02";
                case '3':
                    return "03";
                case '4':
                    return "04";
                case '5':
                    return "05";
                case '0':
                default:
                    return "";
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var newValue = value as string;
            if (string.IsNullOrEmpty(newValue)) return '0';
            if(newValue.Length == 1) return (char)newValue[0];
            return (char)newValue[1];

        }
    }
}
