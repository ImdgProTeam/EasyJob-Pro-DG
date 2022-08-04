using EasyJob_ProDG.UI.Wrapper;
using System.Windows;
using System.Windows.Controls;

namespace EasyJob_ProDG.UI.View.AttachedProperties
{
    /// <summary>
    /// Attached property on DgWrapper allowing to scroll and focus on selected item in DgDataGrid
    /// </summary>
    public class SelectingDgDataGridItem
    {
        /// <summary>
        /// Allows scroll to and focus of a SelectedDg in DgDataGrid 
        /// </summary>
        public static readonly DependencyProperty SelectingItemProperty = DependencyProperty.RegisterAttached(
            "SelectingItem",
            typeof(DgWrapper),
            typeof(SelectingDgDataGridItem),
            new PropertyMetadata(default(DgWrapper), OnSelectingDgDataGridItemChanged));

        public static DgWrapper GetSelectingItem(DependencyObject target)
        {
            return (DgWrapper)target.GetValue(SelectingItemProperty);
        }

        public static void SetSelectingItem(DependencyObject target, DgWrapper value)
        {
            target.SetValue(SelectingItemProperty, value);
        }

        private static void OnSelectingDgDataGridItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var grid = sender as DataGrid;
            if (grid == null || grid.SelectedItem == null)
            {
                return;
            }

            grid.Dispatcher.InvokeAsync(() =>
                {
                    //Scroll to view
                    grid.UpdateLayout();
                    grid.ScrollIntoView(grid.SelectedItem, null);

                    //Get focus
                    var column = grid.CurrentColumn ?? grid.Columns[1];
                    var cellContent = column.GetCellContent(grid.SelectedItem);
                    var cell = cellContent?.Parent as DataGridCell;
                    cell?.Focus();
                });

        }
    }

}
