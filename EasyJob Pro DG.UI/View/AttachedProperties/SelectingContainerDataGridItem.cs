using EasyJob_ProDG.UI.Wrapper;
using System.Windows;
using System.Windows.Controls;

namespace EasyJob_ProDG.UI.View.AttachedProperties
{
    public class SelectingContainerDataGridItem
    {
        /// <summary>
        /// Allows scroll to and focus of a SelectedDg in DgDataGrid 
        /// </summary>
        public static readonly DependencyProperty SelectingItemProperty = DependencyProperty.RegisterAttached(
            "SelectingItem",
            typeof(ContainerWrapper),
            typeof(SelectingContainerDataGridItem),
            new PropertyMetadata(default(ContainerWrapper), OnSelectingContainerDataGridItemChanged));

        public static ContainerWrapper GetSelectingItem(DependencyObject target)
        {
            return (ContainerWrapper)target.GetValue(SelectingItemProperty);
        }

        public static void SetSelectingItem(DependencyObject target, ContainerWrapper value)
        {
            target.SetValue(SelectingItemProperty, value);
        }

        private static void OnSelectingContainerDataGridItemChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
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
