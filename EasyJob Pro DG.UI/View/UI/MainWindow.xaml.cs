using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace EasyJob_ProDG.UI.View.UI
{
    public partial class MainWindow
    {
        /// <summary>
        /// Cargo condition file path to be used when the program started.
        /// </summary>
        public string StartupFilePath;


        public MainWindow(string path = null)
        {
            StartupFilePath = path;

            InitializeComponent();

            SetWindowLocationOnStartup();
        }

        #region Window location
        /// <summary>
        /// Sets Window size and location from settings. 
        /// </summary>
        private void SetWindowLocationOnStartup()
        {
            try
            {
                if (Properties.Settings.Default.WindowStateMaximized)
                {
                    WindowState = WindowState.Maximized;
                }
                else
                {
                    Left = Properties.Settings.Default.WindowPosition.Left;
                    Top = Properties.Settings.Default.WindowPosition.Top;
                    Width = Properties.Settings.Default.WindowPosition.Width;
                    Height = Properties.Settings.Default.WindowPosition.Height;
                }
            }
            catch
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
        }

        /// <summary>
        /// Saves window location and state to settings.
        /// </summary>
        private void SaveCurrentWindowLocationToSettings()
        {
            Properties.Settings.Default.WindowStateMaximized = this.WindowState == WindowState.Maximized;
            Properties.Settings.Default.WindowPosition = this.RestoreBounds;
            Properties.Settings.Default.Save();
        } 
        #endregion

        private void ClosingApplication(object sender, CancelEventArgs e)
        {
            OnWindowClosingEventHandler.Invoke();

            SaveCurrentWindowLocationToSettings();
        }


        // --------- Events -----------------------------------------------
        public delegate void WindowClosing();
        public static event WindowClosing OnWindowClosingEventHandler = null;

        //#region Sorting
        ///// <summary>
        ///// Sorting in accordance with selected pattern of items in Reefer data grid
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void DataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        //{
        //    DataGrid grid = sender as DataGrid;
        //    ListCollectionView collection = CollectionViewSource.GetDefaultView(grid.ItemsSource) as ListCollectionView;

        //    if (e.Column != null && e.Column.Header.ToString() == "Location" || e.Column.Header.ToString() == "SortableLocation")
        //    {
        //        SortDescription sortAscending = new SortDescription("LocationSortable", ListSortDirection.Ascending);
        //        SortDescription sortDescending = new SortDescription("LocationSortable", ListSortDirection.Descending);
        //        bool ascending = collection.SortDescriptions.Contains(sortAscending);
        //        if (collection.SortDescriptions.Count > 0)
        //            collection.SortDescriptions.Clear();
        //        if (ascending)
        //            collection.SortDescriptions.Add(sortDescending);
        //        else
        //            collection.SortDescriptions.Add(sortAscending);
        //        e.Handled = true;
        //    }
        //}

        ///// <summary>
        ///// Sorting in accordance with selected pattern of items in Container data grid
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void DataGrid_Sorting_1(object sender, DataGridSortingEventArgs e)
        //{
        //    DataGrid grid = sender as DataGrid;
        //    ListCollectionView collection = CollectionViewSource.GetDefaultView(grid.ItemsSource) as ListCollectionView;

        //    //Location as per location sortable
        //    if (e.Column != null && e.Column.Header.ToString() == "Location" || e.Column.Header.ToString() == "SortableLocation")
        //    {
        //        SortDescription sortAscending = new SortDescription("LocationSortable", ListSortDirection.Ascending);
        //        SortDescription sortDescending = new SortDescription("LocationSortable", ListSortDirection.Descending);
        //        bool ascending = collection.SortDescriptions.Contains(sortAscending);
        //        if (collection.SortDescriptions.Count > 0)
        //            collection.SortDescriptions.Clear();
        //        if (ascending)
        //            collection.SortDescriptions.Add(sortDescending);
        //        else
        //            collection.SortDescriptions.Add(sortAscending);
        //        e.Handled = true;
        //    }

        //    //Booleans: true first
        //    if (e.Column != null && e.Column.Header.ToString() == "ContainsDgCargo" ||
        //        e.Column.Header.ToString() == "IsUnderdeck" || e.Column.Header.ToString() == "IsClosed" ||
        //        e.Column.Header.ToString() == "IsRf" || e.Column.Header.ToString() == "TypeRecognized" ||
        //        e.Column.Header.ToString() == "HasError")
        //    {
        //        SortDescription sortAscending = new SortDescription(e.Column.Header.ToString(), ListSortDirection.Ascending);
        //        SortDescription sortDescending = new SortDescription(e.Column.Header.ToString(), ListSortDirection.Descending);
        //        bool descending = collection.SortDescriptions.Contains(sortDescending);
        //        if (collection.SortDescriptions.Count > 0)
        //            collection.SortDescriptions.Clear();
        //        if (descending)
        //            collection.SortDescriptions.Add(sortAscending);
        //        else
        //            collection.SortDescriptions.Add(sortDescending);
        //        e.Handled = true;
        //    }
        //}

        //#endregion
    }
}
