using System.Data;

namespace EasyJob_ProDG.UI.View.DialogWindows.Summaries
{
    /// <summary>
    /// Extension and helper methods for creation and filling <see cref="DataTable"/>
    /// </summary>
    internal static class DataTableHelpers
    {
        /// <summary>
        /// Adds column with specified value type to DataTable.
        /// </summary>
        /// <typeparam name="TValue">Type of values in cells.</typeparam>
        /// <param name="dataTable"><see cref="DataTable"/> to add the column to.</param>
        /// <param name="columnName">Specified column name.</param>
        /// <param name="columnIndex">Column index, if required.</param>
        internal static void AddColumn<TValue>(this DataTable dataTable, string columnName, int columnIndex = -1)
        {
            DataColumn column = new DataColumn(columnName, typeof(TValue));
            dataTable.Columns.Add(column);
            if (columnIndex > -1)
            {
                column.SetOrdinal(columnIndex);
            }
            int newColumnIndex = dataTable.Columns.IndexOf(column);
            foreach (DataRow row in dataTable.Rows)
            {
                row[newColumnIndex] = default(TValue);
            }
        }


        /// <summary>
        /// Adds column with string value type to DataTable.
        /// </summary>
        /// <param name="dataTable"><see cref="DataTable"/> to add the column to.</param>
        /// <param name="columnName">Specified column name.</param>
        /// <param name="columnIndex">Column index, if required.</param>
        internal static void AddColumnWithStringValues(this DataTable dataTable, string columnName, int columnIndex = -1)
        {
            DataColumn column = new DataColumn(columnName, typeof(string));
            dataTable.Columns.Add(column);
            if (columnIndex > -1)
            {
                column.SetOrdinal(columnIndex);
            }
            int newColumnIndex = dataTable.Columns.IndexOf(column);
            foreach (DataRow row in dataTable.Rows)
            {
                row[newColumnIndex] = default(string);
            }
        }

        /// <summary>
        /// Adds row to DataTable.
        /// </summary>
        /// <param name="targetDataTable"><see cref="DataTable"/> to add the row to.</param>
        /// <param name="columnValues">An array of values to add to the <paramref name="targetDataTable"/>.</param>
        internal static void AddRow(this DataTable targetDataTable, params object[] columnValues)
        {
            DataRow rowModelWithCurrentColumns = targetDataTable.NewRow();
            targetDataTable.Rows.Add(rowModelWithCurrentColumns);

            for (int columnIndex = 0; columnIndex < targetDataTable.Columns.Count; columnIndex++)
            {
                var s = columnValues[columnIndex];
                rowModelWithCurrentColumns[columnIndex] = s;
            }
        }
    }
}
