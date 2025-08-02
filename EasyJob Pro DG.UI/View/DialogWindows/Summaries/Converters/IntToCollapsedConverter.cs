using EasyJob_ProDG.UI.View.Converters;
using System;
using System.Globalization;
using System.Windows;

namespace EasyJob_ProDG.UI.View.DialogWindows.Summaries.Converters
{
    internal class IntToCollapsedConverter : ConverterBase
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int && int.TryParse(parameter.ToString(), out int intParameter))
            {
                if ((int)value == intParameter)
                    return Visibility.Collapsed;
            }
            return default(Visibility);
        }
    }
}
