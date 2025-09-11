using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EasyJob_ProDG.UI.View
{
    internal static class Helpers
    {
        internal static IEnumerable<T> FindVisualChildren<T>(DependencyObject dependencyObject) where T : DependencyObject
        {
            if (dependencyObject != null)
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(dependencyObject, i);

                    if (child != null && child is T)
                        yield return (T)child;

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                        yield return childOfChild;
                }
        }

        /// <summary>
        /// Method not scrolls up correctly. Shall not be used without modification.
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="obj"></param>
        internal static void ScrollIntoViewImproved(this DataGrid grid, object obj)
        {
            int rowsReserve = grid.Items.Count - grid.SelectedIndex;
            if(rowsReserve > 10) rowsReserve = 10;
            if (grid.SelectedIndex < 30) rowsReserve = 0;
            grid.ScrollIntoView(grid.Items[grid.SelectedIndex + rowsReserve]);
        }
    }
}
