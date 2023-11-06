using System;
using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace EasyJob_ProDG.UI.View.AttachedProperties
{
    /// <summary>
    /// Interface extending IComparer for DataGrid custom sort purpose.
    /// </summary>
    public interface ICustomSorter : IComparer
    {
        ListSortDirection SortDirection { get; set; }
        string SortMemberPath { get; set; }
    }

    /// <summary>
    /// Attached property to DataGrid to allow CustomSort (needs to be set <see cref="AllowCustomSort"/> to "True" for entire DataGrid)
    /// in any of the columns (need to assisn <see cref="CustomSorter"/> a ICustomSorter as StaticResourse
    /// </summary>
    internal class DataGridSortBehavior
    {
        public static readonly DependencyProperty CustomSorterProperty =
            DependencyProperty.RegisterAttached("CustomSorter", typeof(ICustomSorter), typeof(DataGridSortBehavior));
        public static ICustomSorter GetCustomSorter(DataGridColumn column)
        {
            return (ICustomSorter)column.GetValue(CustomSorterProperty);
        }

        public static void SetCustomSorter(DataGridColumn column, ICustomSorter value)
        {
            column.SetValue(CustomSorterProperty, value);
        }



        public static readonly DependencyProperty AllowCustomSortProperty =
            DependencyProperty.RegisterAttached("AllowCustomSort", typeof(bool), typeof(DataGridSortBehavior), new UIPropertyMetadata(false, OnAllowCustomSortChanged));

        public static bool GetAllowCustomSort(DataGrid grid)
        {
            return (bool)grid.GetValue(AllowCustomSortProperty);
        }

        public static void SetAllowCustomSort(DataGrid grid, bool value)
        {
            grid.SetValue(AllowCustomSortProperty, value);
        }

        private static void OnAllowCustomSortChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var existing = d as DataGrid;
            if (existing == null) return;

            var oldAllow = (bool)e.OldValue;
            var newAllow = (bool)e.NewValue;

            if (!oldAllow && newAllow)
            {
                existing.Sorting += HandleCustomSorting;
            }
            else
            {
                existing.Sorting -= HandleCustomSorting;
            }
        }

        private static void HandleCustomSorting(object sender, DataGridSortingEventArgs e)
        {
            var sorter = GetCustomSorter(e.Column);
            if (sorter == null) return;

            var dataGrid = sender as DataGrid;
            if (dataGrid == null || !GetAllowCustomSort(dataGrid)) return;

            var listColView = dataGrid.ItemsSource as ListCollectionView;
            if ((listColView == null))
                throw new Exception("The DataGrid's ItemsSource property must be of type ListCollectionView");


            e.Handled = true;

            var direction = (e.Column.SortDirection != ListSortDirection.Ascending)
                ? ListSortDirection.Ascending
                : ListSortDirection.Descending;

            e.Column.SortDirection = sorter.SortDirection = direction;
            sorter.SortMemberPath = e.Column.SortMemberPath;

            listColView.CustomSort = sorter;

        }

    }
}
