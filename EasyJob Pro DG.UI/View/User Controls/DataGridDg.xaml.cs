using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.ViewModel;
using EasyJob_ProDG.UI.Wrapper;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.View.User_Controls
{
    /// <summary>
    /// Логика взаимодействия для DgDataGrid.xaml
    /// </summary>
    public partial class DataGridDg : UserControl
    {
        DataGridDgViewModel viewModel;
        //DgSortOrderDirection dgSortOrderDirection;
        private static bool IsCellEditingOn;
        private bool isInitialized;
        private int currentColumnIndex = 0;
        private int currentRowIndex = 1;

        public DataGridDg()
        {
            InitializeComponent();
            viewModel = MainDgTable.DataContext as DataGridDgViewModel;
            //isInitialized = true;
        }

        /// <summary>
        /// TO BE CHANGED TO IMPLEMENT REQUIREMENTS OF SETTINGS
        /// Method called when sorting requested by clicking on the column header
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainDgTable_Sorting(object sender, DataGridSortingEventArgs e)
        {
            if (e.Column == null) return;
            ListCollectionView collection = CollectionViewSource.GetDefaultView(MainDgTable.ItemsSource) as ListCollectionView;

            switch (e.Column.Header.ToString())
            {
                case "Position":
                    SortDescription sortAscending = new SortDescription("LocationSortable", ListSortDirection.Ascending);
                    SortDescription sortDescending = new SortDescription("LocationSortable", ListSortDirection.Descending);

                    bool ascending = collection.SortDescriptions.Contains(sortAscending);
                    if (collection.SortDescriptions.Count > 0)
                        collection.SortDescriptions.Clear();
                    if (ascending)
                        collection.SortDescriptions.Add(sortDescending);
                    else
                        collection.SortDescriptions.Add(sortAscending);
                    e.Handled = true;
                    break;

                default:
                    break;
            }
        }

        private void MainDgTable_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

            TextBox txb = sender as TextBox;
            DataGrid grid = sender as DataGrid;
            DataGridColumn column = grid?.CurrentColumn;
            currentRowIndex = grid?.SelectedIndex ?? currentRowIndex;

            DgWrapper dg = grid?.SelectedItem as DgWrapper;

            string strKey = e.Key.ToString();
            string trimmedStrKey;
            if (strKey.StartsWith("D") && strKey.Length > 1)
            {
                trimmedStrKey = strKey.Replace("D", "");
            }
            else
            {
                trimmedStrKey = strKey;
            }
            bool isCharKey = Char.TryParse(trimmedStrKey, out var key);

            //Enter key
            if (e.Key == Key.Enter)
            {
                //To start editing cell on pressing Enter
                if (IsCellEditingOn == false && column is DataGridTextColumn)
                {
                    grid?.BeginEdit();
                    e.Handled = true;
                    return;
                }

                //To avoid shifting of focus to the next row after pressing Enter when editing
                grid?.CommitEdit();
                e.Handled = true;
                return;
            }

            //Delete row
            if (e.Key == Key.Delete)
            {
                return;
            }

            //Stowage category validation
            if (column?.Header != null && column.Header.ToString() == "Stowage category")
            {
                if (!isCharKey) return;
                if (DataGridDgViewModel.StowageCategories.Contains(key))
                {
                    if (dg != null) dg.StowageCat = key;
                }
            }

            if (column?.Header != null && column.Header.ToString() == "Proper shipping name")
            {
                if (!isCharKey) return;
            }


        }

        /// <summary>
        /// Selects all text in TextBox
        /// </summary>
        /// <param name="txb"></param>
        private void SelectAllText(TextBox txb)
        {
            try
            {
                txb.SelectAll();
            }
            finally
            {

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
                var cellContent = MainDgTable.Columns[0].GetCellContent(MainDgTable.Items[rowIndex]);
                if (cellContent?.Parent is DataGridCell cell) cell.Focus();
            }
            catch
            {
                //ignore
            }
        }

        /// <summary>
        /// Exports MainDataGrid to excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportToExcel(object sender, RoutedEventArgs e)
        {
            ExportDataGridToExcel.ExportToExcel(MainDgTable);
        }



        // --------------------- Event Handlers -------------------------------------

        /// <summary>
        /// Sets Cell into editing mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainDgTable_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            IsCellEditingOn = true;
        }

        /// <summary>
        /// Sets cell editing mode to off
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainDgTable_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            IsCellEditingOn = false;
        }

        private void txbNameColumn_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 3)
            {
                SelectAllText(sender as TextBox);
                return;
            }

        }

        private void txbNameColumn_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            try
            {
                SelectAllText((TextBox)sender);
            }
            finally
            {

            }
        }

        /// <summary>
        /// Handles verification of values added.
        /// TO BE REPLACED WITH VM VERIFICATION
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            DataGridColumn column = grid?.CurrentColumn;
            TextBox tBox = e.OriginalSource as TextBox;

            if (column == null) return;

            if (column.Header.ToString() == "UNNO" && tBox != null)
            {
                if (tBox.Text.Length >= 4)
                {
                    e.Handled = true;
                    return;
                }
                e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
            }
        }

        private void MainDgTable_Loaded(object sender, RoutedEventArgs e)
        {
            isInitialized = true;
        }

        private void MainDgTable_OnSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

        }

        private void MainDgTable_OnUnloadingRow(object sender, DataGridRowEventArgs e)
        {
            FocusOnRow(currentRowIndex);
        }

    }
}
