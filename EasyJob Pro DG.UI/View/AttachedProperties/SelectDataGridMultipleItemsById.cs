using EasyJob_ProDG.UI.Wrapper;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace EasyJob_ProDG.UI.View.AttachedProperties
{
    /// <summary>
    /// Attached property on DataGrid allowing to select multiple items according to list of ids or numbers.
    /// </summary>
    public class SelectDataGridMultipleItems
    {
        // By Id for DataGridDg
        public static List<string> GetSelectDataGridMultipleItemsById(DependencyObject obj)
        {
            return (List<string>)obj.GetValue(SelectDataGridMultipleItemsByIdProperty);
        }

        public static void SetSelectDataGridMultipleItemsById(DependencyObject obj, List<string> value)
        {
            obj.SetValue(SelectDataGridMultipleItemsByIdProperty, value);
        }


        public static readonly DependencyProperty SelectDataGridMultipleItemsByIdProperty =
            DependencyProperty.RegisterAttached("SelectDataGridMultipleItemsById", typeof(List<string>), typeof(SelectDataGridMultipleItems), new PropertyMetadata(null, OnSelectDataGridMultipleItemsByIdChanged));

        private static void OnSelectDataGridMultipleItemsByIdChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var grid = sender as DataGrid;
            if (grid == null || grid.Items.Count == 0)
                return;

            grid.SelectedItems.Clear();
            foreach (var unit in grid.Items)
            {
                if (((List<string>)e.NewValue).Contains(((DgWrapper)unit).Model.ID.ToString()))
                    grid.SelectedItems.Add(unit);
            }
            grid.Focus();
            if (grid.SelectedItems.Count > 0)
                grid.ScrollIntoView(grid.SelectedItems[0]);
        }

        // By ContainerNumber for DataGridContainer

        public static List<string> GetSelectDataGridMultipleItemsByContainerNumber(DependencyObject obj)
        {
            return (List<string>)obj.GetValue(GetSelectDataGridMultipleItemsByContainerNumberProperty);
        }

        public static void SetGetSelectDataGridMultipleItemsByContainerNumber(DependencyObject obj, List<string> value)
        {
            obj.SetValue(GetSelectDataGridMultipleItemsByContainerNumberProperty, value);
        }


        public static readonly DependencyProperty GetSelectDataGridMultipleItemsByContainerNumberProperty =
            DependencyProperty.RegisterAttached("GetSelectDataGridMultipleItemsByContainerNumber", typeof(List<string>), typeof(SelectDataGridMultipleItems), new PropertyMetadata(null, OnGetSelectDataGridMultipleItemsByContainerNumberChanged));

        private static void OnGetSelectDataGridMultipleItemsByContainerNumberChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var grid = sender as DataGrid;
            if (grid == null || grid.Items.Count == 0)
                return;

            grid.SelectedItems.Clear();
            foreach (var unit in grid.Items)
            {
                if (((List<string>)e.NewValue).Contains(((ContainerWrapper)unit).ContainerNumber))
                    grid.SelectedItems.Add(unit);
            }
            if (grid.SelectedItems.Count > 0)
                grid.ScrollIntoView(grid.SelectedItems[0]);
            grid.Focus();
        }
    }

}
