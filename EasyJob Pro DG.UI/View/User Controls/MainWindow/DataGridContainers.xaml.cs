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
    /// Логика взаимодействия для DataGridContainers.xaml
    /// </summary>
    public partial class DataGridContainers : UserControl
    {
        private int currentRowIndex = 1;
        private bool isDeletingRow = false;

        public DataGridContainers()
        {
            InitializeComponent();
            LoadColumnSettings();

            MainWindow.OnWindowClosingEventHandler += new MainWindow.WindowClosing(UpdateColumnSettings);
        }

        #region Column settings
        /// <summary>
        /// Loads column settings for ContainerDataTable from settings.settings
        /// </summary>
        private void LoadColumnSettings()
        {
            var displayIndexes = Properties.Settings.Default.ContainerDataTableDisplayIndex.Split(';');
            var widths = Properties.Settings.Default.ContainerDataTableWidth.Split(';');
            var visibilitys = Properties.Settings.Default.ContainerDataTableVisibilities.Split(';');

            if (displayIndexes.Count() != MainContainerDataTable.Columns.Count || displayIndexes.Any(x => int.Parse(x) < 0)) return;

            try
            {
                int index;
                double width;

                for (int i = 0; i < displayIndexes.Count(); i++)
                {
                    index = int.Parse(displayIndexes[i]);
                    if (index < 0) throw new ArgumentOutOfRangeException(nameof(index), "Negative display index");
                    MainContainerDataTable.Columns[i].DisplayIndex = index;

                    width = double.Parse(widths[i]);
                    if (width < 0) throw new ArgumentOutOfRangeException(nameof(width), "Negative column width");
                    MainContainerDataTable.Columns[i].Width = width;

                    MainContainerDataTable.Columns[i].Visibility = (System.Windows.Visibility)Enum.Parse(typeof(System.Windows.Visibility), visibilitys[i]);
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("-----> Restoring of ContainerDataTable column settings caused an exception");
                int i = 0;
                foreach (var column in MainContainerDataTable.Columns)
                {
                    column.DisplayIndex = i;
                    column.Width = DataGridLength.Auto;
                    column.Visibility = Visibility.Visible;
                    i++;
                }
                Debug.WriteLine("-----> Default columns in ConainerDataTable created");
            }
        }

        /// <summary>
        /// Updates settings.settings with ContainerDataTable column settings
        /// </summary>
        private void UpdateColumnSettings()
        {
            List<int> displayIndexes = new List<int>();
            List<double> widths = new List<double>();
            List<string> visibilitys = new List<string>();

            foreach (var column in MainContainerDataTable.Columns)
            {
                displayIndexes.Add(column.DisplayIndex);
                widths.Add(column.ActualWidth);
                visibilitys.Add(column.Visibility.ToString());
            }

            Properties.Settings.Default.ContainerDataTableDisplayIndex = String.Join(";", displayIndexes);
            Properties.Settings.Default.ContainerDataTableWidth = String.Join(";", widths);
            Properties.Settings.Default.ContainerDataTableVisibilities = string.Join(";", visibilitys);
        }


        #endregion

 
        private void MainContainerDataTable_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            currentRowIndex = grid?.SelectedIndex ?? currentRowIndex;

            //Delete row
            if (e.Key == Key.Delete)
            {
                isDeletingRow = true;
                if (currentRowIndex == MainContainerDataTable.Items.Count - 1) currentRowIndex--;
                return;
            }

        }


        /// <summary>
        /// Sets focus on a selected row by index
        /// </summary>
        /// <param name="rowIndex"></param>
        private void FocusOnRow(int rowIndex)
        {
            try
            {
                var cellContent = MainContainerDataTable.Columns[0].GetCellContent(MainContainerDataTable.Items[rowIndex]);
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
        private void MainContainerDataTable_UnloadingRow(object sender, DataGridRowEventArgs e)
        {
            if (!isDeletingRow) return;
            FocusOnRow(currentRowIndex);
            isDeletingRow = false;
        }
    }
}
