using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace EasyJob_ProDG.UI.View.Converters
{
    internal abstract class ConverterBase : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider) => this;
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Conversion back is not supported");
        }
    }
}
