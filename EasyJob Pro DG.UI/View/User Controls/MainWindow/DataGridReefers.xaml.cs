using EasyJob_ProDG.UI.IO;
using EasyJob_ProDG.UI.View.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.View.User_Controls
{
    /// <summary>
    /// Логика взаимодействия для DataGridReefers.xaml
    /// </summary>
    public partial class DataGridReefers : UserControl
    {
        private int currentRowIndex = 1;
        private bool isDeletingRow = false;

        public DataGridReefers()
        {
            InitializeComponent();

            LoadColumnSettings();

            MainWindow.OnWindowClosingEventHandler += new MainWindow.WindowClosing(UpdateColumnSettings);
        }

        #region Column settings
        /// <summary>
        /// Loads column settings for ReeferDataTable from settings.settings
        /// </summary>
        internal void LoadColumnSettings()
        {
            var displayIndexes = Properties.Settings.Default.ReeferDataTableDisplayIndex.Split(';');
            var widths = Properties.Settings.Default.ReeferDataTableWidth.Split(';');
            var visibilitys = Properties.Settings.Default.ReeferDataTableVisibilities.Split(';');

            if (displayIndexes.Count() != MainReeferDataTable.Columns.Count) return;

            try
            {
                int index; 
                double width;

                for (int i = 0; i < displayIndexes.Count(); i++)
                {
                    index = int.Parse(displayIndexes[i]);
                    if (index < 0) throw new ArgumentOutOfRangeException(nameof(index), "Negative display index");
                    MainReeferDataTable.Columns[i].DisplayIndex = index;

                    width = double.Parse(widths[i]);
                    if(width < 0) throw new ArgumentOutOfRangeException(nameof(width), "Negative column width");
                    MainReeferDataTable.Columns[i].Width = width;

                    MainReeferDataTable.Columns[i].Visibility = (System.Windows.Visibility)Enum.Parse(typeof(System.Windows.Visibility), visibilitys[i]);
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("-----> Restoring of ReefererDataTable column settings caused an exception");
                int i = 0;
                foreach (var column in MainReeferDataTable.Columns)
                {
                    column.DisplayIndex = i;
                    column.Width = DataGridLength.Auto;
                    column.Visibility = Visibility.Visible;
                    i++;
                }
                Debug.WriteLine("-----> Default columns in ReeferDataTable created");
            }
        }

        /// <summary>
        /// Updates settings.settings with ReeferDataTable column settings
        /// </summary>
        private void UpdateColumnSettings()
        {
            List<int> displayIndexes = new List<int>();
            List<double> widths = new List<double>();
            List<string> visibilitys = new List<string>();

            foreach (var column in MainReeferDataTable.Columns)
            {
                displayIndexes.Add(column.DisplayIndex);
                widths.Add(column.ActualWidth);
                visibilitys.Add(column.Visibility.ToString());
            }

            Properties.Settings.Default.ReeferDataTableDisplayIndex = String.Join(";", displayIndexes);
            Properties.Settings.Default.ReeferDataTableWidth = String.Join(";", widths);
            Properties.Settings.Default.ReeferDataTableVisibilities = string.Join(";", visibilitys);
        }

        #endregion


        #region Export to Excel
        private void ExportToExcel(object sender, RoutedEventArgs e)
        {
            ExportDataGridToExcel.ExportToExcel(MainReeferDataTable);
        }

        #endregion


        #region Input logic

        private void MainReeferDataTable_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            currentRowIndex = grid?.SelectedIndex ?? currentRowIndex;

            //Delete row
            if (e.Key == Key.Delete)
            {
                isDeletingRow = true;
                if (currentRowIndex == MainReeferDataTable.Items.Count - 1) currentRowIndex--;
                return;
            }

        } 

        #endregion


        #region Focus logic

        /// <summary>
        /// Sets focus on a selected row by index
        /// </summary>
        /// <param name="rowIndex"></param>
        private void FocusOnRow(int rowIndex)
        {
            try
            {
                var cellContent = MainReeferDataTable.Columns[0].GetCellContent(MainReeferDataTable.Items[rowIndex]);
                if (cellContent?.Parent is DataGridCell cell) cell.Focus();
            }
            catch
            {
                //ignore
            }
        }

        /// <summary>
        /// Used to focus on row after deletion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainReeferDataTable_UnloadingRow(object sender, DataGridRowEventArgs e)
        {
            if (!isDeletingRow) return;
            FocusOnRow(currentRowIndex);
            isDeletingRow = false;
        }

        #endregion

        private void MainContainerDataTable_Sorting(object sender, DataGridSortingEventArgs e)
        {
            if (e.Column.SortMemberPath.StartsWith("Is")
                || e.Column.SortMemberPath.StartsWith("Has")
                || e.Column.SortMemberPath.StartsWith("Contains"))
                if (e.Column.SortDirection == null)
                {
                    e.Column.SortDirection = System.ComponentModel.ListSortDirection.Ascending;
                }
        }

    }
}
