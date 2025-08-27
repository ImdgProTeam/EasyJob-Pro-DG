using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EasyJob_ProDG.UI.View.User_Controls
{
    /// <summary>
    /// Логика взаимодействия для DataGridContainers.xaml
    /// </summary>
    public partial class DataGridUpdates : CommonDataGridUserControl
    {
        public DataGridUpdates() : base(Animations.AnimationTypes.None)
        {
            InitializeComponent();

            MainDataTable = MainUpdatesDataTable;
            CallBaseConstructorMethods();
        }


        #region Column settings
        /// <summary>
        /// Loads column settings for ContainerDataTable from settings.settings
        /// </summary>
        internal override void LoadColumnSettings()
        {
            var displayIndexes = Properties.Settings.Default.UpdatesDataTableDisplayIndex.Split(';');
            var widths = Properties.Settings.Default.UpdatesDataTableWidth.Split(';');
            var visibilitys = Properties.Settings.Default.UpdatesDataTableVisibilities.Split(';');

            if (displayIndexes.Count() != MainUpdatesDataTable.Columns.Count || displayIndexes.Any(x => int.Parse(x) < 0)) return;

            try
            {
                int index;
                double width;

                for (int i = 0; i < displayIndexes.Count(); i++)
                {
                    index = int.Parse(displayIndexes[i]);
                    if (index < 0) throw new ArgumentOutOfRangeException(nameof(index), "Negative display index");
                    MainUpdatesDataTable.Columns[i].DisplayIndex = index;

                    width = double.Parse(widths[i]);
                    if (width < 0) throw new ArgumentOutOfRangeException(nameof(width), "Negative column width");
                    MainUpdatesDataTable.Columns[i].Width = width;

                    MainUpdatesDataTable.Columns[i].Visibility = (System.Windows.Visibility)Enum.Parse(typeof(System.Windows.Visibility), visibilitys[i]);
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("-----> Restoring of ContainerDataTable column settings caused an exception");
                int i = 0;
                foreach (var column in MainUpdatesDataTable.Columns)
                {
                    column.DisplayIndex = i;
                    column.Width = DataGridLength.Auto;
                    column.Visibility = Visibility.Visible;
                    i++;
                }
                Debug.WriteLine("-----> Default columns in ConainerDataTable created");
            }

            RestoreSummaryControlColumnWidth();
        }

        /// <summary>
        /// Updates settings.settings with ContainerDataTable column settings
        /// </summary>
        internal override void SaveColumnSettings()
        {
            List<int> displayIndexes = new List<int>();
            List<double> widths = new List<double>();
            List<string> visibilitys = new List<string>();

            foreach (var column in MainUpdatesDataTable.Columns)
            {
                displayIndexes.Add(column.DisplayIndex);
                widths.Add(column.ActualWidth);
                visibilitys.Add(column.Visibility.ToString());
            }

            Properties.Settings.Default.UpdatesDataTableDisplayIndex = String.Join(";", displayIndexes);
            Properties.Settings.Default.UpdatesDataTableWidth = String.Join(";", widths);
            Properties.Settings.Default.UpdatesDataTableVisibilities = string.Join(";", visibilitys);

            SaveSummaryControlColumnWidth();
        }

        private void SaveSummaryControlColumnWidth()
        {
            if (MainControlGrid.ColumnDefinitions[1].ActualWidth < 1) return;
            Properties.Settings.Default.UpdatesControlWidth = MainControlGrid.ColumnDefinitions[1].ActualWidth;
        }

        private void RestoreSummaryControlColumnWidth()
        {
            MainControlGrid.ColumnDefinitions[1].Width = new GridLength(Properties.Settings.Default.UpdatesControlWidth);
        }

        #endregion

    }
}
