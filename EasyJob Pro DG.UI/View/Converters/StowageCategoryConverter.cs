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
            return ConvertStowageCategory(input);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var newValue = value as string;
            if (string.IsNullOrEmpty(newValue)) return '0';
            if (newValue.Length == 1) return (char)newValue[0];
            return (char)newValue[1];

        }

        /// <summary>
        /// Convertis Char input of Stowage category to string in accordance with IMDG Code, or empty string otherwise.
        /// </summary>
        /// <param name="input">Char input of StowageCategory in accordance with IMDGCode.AllValidStowageCategories.</param>
        /// <returns>String Stowage category as it is given in IMDG Code.</returns>
        private static string ConvertStowageCategory(char input)
        {
            if (char.IsLetter(input))
                return input.ToString();
            switch (input)
            {
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                    return "0" + input;
                case '0':
                default:
                    return string.Empty;
            }
        }

    }
}
