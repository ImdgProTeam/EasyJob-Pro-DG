using EasyJob_ProDG.UI.View.Animations;
using EasyJob_ProDG.UI.View.UI;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.View.User_Controls
{
    public abstract class CommonDataGridUserControl : AnimatedUserControl
    {
        protected static bool IsCellEditingOn;
        protected int currentRowIndex = 1;
        protected bool isDeletingRow = false;
        protected DataGrid MainDataTable { get; set; }

        public CommonDataGridUserControl(AnimationTypes userControlLoadAnimation, float slideInSeconds = 1.5f,
                                    AnimationTypes userControlUnloadAnimation = AnimationTypes.None, bool isAnimatedEachTime = false) : base(userControlLoadAnimation, slideInSeconds, userControlLoadAnimation, isAnimatedEachTime)
        {

        }

        /// <summary>
        /// This method contains common methods that shall be called from constructor after initializing of a component.
        /// </summary>
        protected void CallBaseConstructorMethods()
        {
            LoadColumnSettings();
            MainWindow.OnWindowClosingEventHandler -= new MainWindow.WindowClosing(SaveColumnSettings);
            MainWindow.OnWindowClosingEventHandler += new MainWindow.WindowClosing(SaveColumnSettings);
        }

        #region Column settings

        /// <summary>
        /// Loads column settings for DataTable from settings.settings
        /// </summary>
        internal abstract void LoadColumnSettings();

        /// <summary>
        /// Updates settings.settings with current DataTable colum settings
        /// </summary>
        internal abstract void SaveColumnSettings();

        #endregion

        #region Input logic

        internal virtual void MainDataTable_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            DataGridColumn column = grid?.CurrentColumn;
            currentRowIndex = grid?.SelectedIndex ?? currentRowIndex;



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
                if (currentRowIndex == MainDataTable.Items.Count - 1) currentRowIndex--;
                return;
            }

            //CheckBoxes
            if (e.Key == Key.Space &&
                column?.GetType() == typeof(DataGridTemplateColumn))
            {
                var cellContent = column.GetCellContent(grid.Items[currentRowIndex]);
                var checkBoxes = Helpers.FindVisualChildren<CheckBox>(cellContent);
                if (checkBoxes.Count() == 1)
                {
                    var checkBox = checkBoxes.FirstOrDefault();
                    if (checkBox.IsEnabled == false) return;
                    checkBox.IsChecked = !checkBox.IsChecked;
                }
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
                var cellContent = MainDataTable.Columns[0].GetCellContent(MainDataTable.Items[rowIndex]);
                if (cellContent?.Parent is DataGridCell cell)
                {
                    cell.Focus();
                    cell.Focus();
                    cell.Focus();
                }
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
        internal void MainDataTable_OnUnloadingRow(object sender, DataGridRowEventArgs e)
        {
            if (!isDeletingRow) return;
            FocusOnRow(currentRowIndex);
            isDeletingRow = false;
        }

        #endregion

        #region Sorting
        internal void MainDataTable_Sorting(object sender, DataGridSortingEventArgs e)
        {
            if (e.Column.SortMemberPath.StartsWith("Is")
                || e.Column.SortMemberPath.StartsWith("Has")
                || e.Column.SortMemberPath.StartsWith("Contains"))
                if (e.Column.SortDirection == null)
                {
                    e.Column.SortDirection = System.ComponentModel.ListSortDirection.Ascending;
                }
        }

        #endregion


        #region Editing

        /// <summary>
        /// Sets Cell into editing mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void MainTable_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            IsCellEditingOn = true;
        }

        /// <summary>
        /// Sets cell editing mode to off
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void MainTable_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            IsCellEditingOn = false;
        }
        #endregion
    }
}
