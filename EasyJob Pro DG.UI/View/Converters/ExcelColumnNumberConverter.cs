using System;
using System.Globalization;
using System.Windows.Data;

namespace EasyJob_ProDG.UI.View.Converters
{
    public class ExcelColumnNumberConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int temp = (int)value;
            return Model.IO.Excel.WithXl.Columns[temp];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var input = value as char?;
            if (input == null) return null;
            char c = input.Value;
            bool notInt = !char.IsDigit(c);
            if (char.IsLetter(c)) return Model.IO.Excel.WithXl.Columns.IndexOf(c);

            if (notInt)
            {
                return null;
            }
            else return int.Parse(c.ToString());
        }
    }
}
