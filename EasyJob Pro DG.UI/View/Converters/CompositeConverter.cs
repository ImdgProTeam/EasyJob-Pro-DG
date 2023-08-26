using System;
using System.Globalization;
using System.Windows.Data;

namespace EasyJob_ProDG.UI.View.Converters
{
    /// <summary>
    /// Composite converter will carry out double convertion with FirstConverter 
    /// and then with SecondConverter set as parameters of this Composite converter.
    /// </summary>
    internal class CompositeConverter : ConverterBase
    {
        public IValueConverter FirstConverter { get; set; }
        public IValueConverter SecondConverter { get; set; }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result1 = FirstConverter?.Convert(value, targetType, parameter, culture) ?? value;
            var result2 = SecondConverter?.Convert(result1, targetType, parameter, culture) ?? result1;

            return result2;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result2 = SecondConverter?.ConvertBack(value, targetType, parameter, culture) ?? value;
            var result1 = FirstConverter?.ConvertBack(result2, targetType, parameter, culture) ?? result2;

            return result1;
        }
    }
}
