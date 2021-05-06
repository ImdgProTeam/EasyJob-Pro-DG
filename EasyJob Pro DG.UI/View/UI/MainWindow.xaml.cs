using EasyJob_ProDG.UI.ViewModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using EasyJob_ProDG.UI.Services.FirstStartServices;

namespace EasyJob_ProDG.UI.View.UI
{
    public partial class MainWindow
    {
        public static bool Started;
        public string StartupFilePath;


        public MainWindow(string path = null)
        {
            //First start service
            //FirstStartService.DoFirstStart();

            StartupFilePath = path;

            //Initialize window
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            InitializeComponent();

            //MainWindowViewModel viewModel = DataContext as MainWindowViewModel;
            //viewModel?.SetupDialogService(this);

            Left = Properties.Settings.Default.WindowPosition.Left;
            Top = Properties.Settings.Default.WindowPosition.Top;
            Width = Properties.Settings.Default.WindowPosition.Width;
            Height = Properties.Settings.Default.WindowPosition.Height;

            Started = true;
        }

        private void ClosingApplication(object sender, CancelEventArgs e)
        {
            OnWindowClosingEventHandler.Invoke();

            Properties.Settings.Default.WindowPosition = this.RestoreBounds;
            Properties.Settings.Default.Save();
        }

        public delegate void WindowClosing();
        public static event WindowClosing OnWindowClosingEventHandler = null;

        /// <summary>
        /// Sorting in accordance with selected pattern of items in Reefer data grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            ListCollectionView collection = CollectionViewSource.GetDefaultView(grid.ItemsSource) as ListCollectionView;

            if (e.Column != null && e.Column.Header.ToString() == "Location" || e.Column.Header.ToString() == "SortableLocation")
            {
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
            }
        }

        /// <summary>
        /// Sorting in accordance with selected pattern of items in Container data grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGrid_Sorting_1(object sender, DataGridSortingEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            ListCollectionView collection = CollectionViewSource.GetDefaultView(grid.ItemsSource) as ListCollectionView;

            //Location as per location sortable
            if (e.Column != null && e.Column.Header.ToString() == "Location" || e.Column.Header.ToString() == "SortableLocation")
            {
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
            }

            //Booleans: true first
            if (e.Column != null && e.Column.Header.ToString() == "ContainsDgCargo" ||
                e.Column.Header.ToString() == "IsUnderdeck" || e.Column.Header.ToString() == "IsClosed" ||
                e.Column.Header.ToString() == "IsRf" || e.Column.Header.ToString() == "TypeRecognized" ||
                e.Column.Header.ToString() == "HasError")
            {
                SortDescription sortAscending = new SortDescription(e.Column.Header.ToString(), ListSortDirection.Ascending);
                SortDescription sortDescending = new SortDescription(e.Column.Header.ToString(), ListSortDirection.Descending);
                bool descending = collection.SortDescriptions.Contains(sortDescending);
                if (collection.SortDescriptions.Count > 0)
                    collection.SortDescriptions.Clear();
                if (descending)
                    collection.SortDescriptions.Add(sortAscending);
                else
                    collection.SortDescriptions.Add(sortDescending);
                e.Handled = true;
            }
        }

    }
}
