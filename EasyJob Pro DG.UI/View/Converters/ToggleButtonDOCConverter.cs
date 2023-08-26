using System;
using System.Globalization;

namespace EasyJob_ProDG.UI.View.Converters
{
    internal class ToggleButtonDOCConverter : ConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var a = value as byte?;
            if (!a.HasValue) return "";
            return a == 0 ? "N" : "Y";

        }
    }
}
