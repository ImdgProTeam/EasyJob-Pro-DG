using EasyJob_ProDG.UI.Wrapper;
using System;
using System.Globalization;
using System.Windows.Data;

namespace EasyJob_ProDG.UI.View.Converters
{
    [ValueConversion(typeof(CellPositionWrapper), typeof(string))]
    public class CellToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            CellPositionWrapper cell;
            if(value != null)
            {
                cell = value as CellPositionWrapper;
                if (cell.IsEmpty()) return string.Empty; 
                else return cell.ToString();
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
