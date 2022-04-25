using System;
using System.Globalization;
using System.Windows.Data;

namespace EasyJob_ProDG.UI.View.Converters
{
    internal abstract class ConverterBase : IValueConverter
    {
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Conversion back is not supported");
        }
    }
}
