using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.ViewModel;
using EasyJob_ProDG.UI.Wrapper;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace EasyJob_ProDG.UI.View.User_Controls
{
    /// <summary>
    /// Логика взаимодействия для DgDataGrid.xaml
    /// </summary>
    public partial class DataGridDg : UserControl
    {
        DataGridDgViewModel viewModel;
        private static bool IsCellEditingOn;
        private int currentRowIndex = 1;
        private bool isDeletingRow = false;
        private double OriginalScrollPosition { get; set; }
        private bool IsResizingColumn { get; set; }


        public DataGridDg()
        {
            InitializeComponent();
            viewModel = MainDgTable.DataContext as DataGridDgViewModel;
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
                isDeletingRow = true;
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
        /// Exports MainDataGrid as it is displayed to excel
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

        /// <summary>
        /// Used to focus on row after deletion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainDgTable_OnUnloadingRow(object sender, DataGridRowEventArgs e)
        {
            if (!isDeletingRow) return;
            FocusOnRow(currentRowIndex);
            isDeletingRow = false;
        }

        private void DataGrid_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            if (this.IsResizingColumn
              && TryFindVisualChildElement(dataGrid, out ScrollViewer scrollViewer))
            {
                this.Dispatcher.InvokeAsync(() =>
                {
                    scrollViewer.ScrollToHorizontalOffset(this.OriginalScrollPosition);
                    this.IsResizingColumn = false;
                }, DispatcherPriority.Background);
            }
        }
        private void DataGridCell_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var dataGridCell = sender as DataGridCell;
            if (TryFindVisualChildElement(this.MainDgTable, out ScrollViewer scrollViewer))
            {
                this.OriginalScrollPosition = scrollViewer.HorizontalOffset;
                this.IsResizingColumn = true;
            }
        }

        private bool TryFindVisualChildElement<TChild>(DependencyObject parent, out TChild resultElement)
          where TChild : DependencyObject
        {
            resultElement = null;

            if (parent is Popup popup)
            {
                parent = popup.Child;
                if (parent == null)
                {
                    return false;
                }
            }

            for (var childIndex = 0; childIndex < VisualTreeHelper.GetChildrenCount(parent); childIndex++)
            {
                DependencyObject childElement = VisualTreeHelper.GetChild(parent, childIndex);

                if (childElement is TChild child)
                {
                    resultElement = child;
                    return true;
                }

                if (TryFindVisualChildElement(childElement, out resultElement))
                {
                    return true;
                }
            }

            return false;
        }

        private void MainDgTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                MainDgTable.ScrollIntoView(MainDgTable.SelectedItem);
            }
            catch
            {

            }
        }

        /// <summary>
        /// Method paints each alternate column in DataGrid with specified color
        /// </summary>
        /// <param name="color"></param>
        private void PaintAlternateColumn(Color color)
        {
            int alt = 0;
            for (byte i = 0; i < MainDgTable.Columns.Count; i++)
            {
                var column = MainDgTable.Columns.FirstOrDefault(x => x.DisplayIndex == i);
                if (column?.Visibility == Visibility.Visible)
                {
                    if(alt == 0)
                    {
                        alt = 1;
                        continue;
                    }

                    FrameworkElement cellContent;
                    for (int rowIndex = 0; rowIndex < MainDgTable.Items.Count; rowIndex++)
                    {
                        cellContent = column.GetCellContent(MainDgTable.Items[rowIndex]);
                        if (cellContent?.Parent is DataGridCell cell) cell.Background = new SolidColorBrush(color);
                    }
                    alt = 0;
                }
            }
        }
    }
}
