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
            if (rowsReserve > 10) rowsReserve = 10;
            if (grid.SelectedIndex < 30) rowsReserve = 0;
            grid.ScrollIntoView(grid.Items[grid.SelectedIndex + rowsReserve]);
        }

        /// <summary>
        /// Checks if the element is or contains a <see cref="ComboBox"/>.
        /// </summary>
        /// <param name="element"></param>
        /// <returns>True if the element is or contains a <see cref="ComboBox"/></returns>
        internal static bool IsInsideComboBox(DependencyObject element)
        {
            while (element != null)
            {
                if(element is ComboBox || element is ComboBoxItem)
                {
                    return true;
                }

                element = VisualTreeHelper.GetParent(element);
            }
            return false;
        }

        /// <summary>
        /// Searches for a parent of chosen type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="child"></param>
        /// <returns>Null if no parent of chosen type is found.</returns>
        internal static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;

            if (parentObject is T parent)
                return parent;

            return FindParent<T>(parentObject);
        }
    }
}
