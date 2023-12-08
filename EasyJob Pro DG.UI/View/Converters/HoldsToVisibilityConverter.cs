using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EasyJob_ProDG.UI.View.Converters
{
    [ValueConversion(typeof(byte), typeof(Visibility))]
    internal class HoldsToVisibilityConverter : ConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            byte? number = value as byte?;
            bool success = byte.TryParse((string)parameter, out byte hold);
            if (success) 
                if (number >= hold) return Visibility.Visible;
            return Visibility.Hidden;
        }
    }
}
