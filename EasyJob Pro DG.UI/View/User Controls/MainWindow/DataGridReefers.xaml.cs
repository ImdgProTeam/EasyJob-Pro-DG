using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.View.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EasyJob_ProDG.UI.View.User_Controls
{
    /// <summary>
    /// Логика взаимодействия для DataGridReefers.xaml
    /// </summary>
    public partial class DataGridReefers : UserControl
    {
        public DataGridReefers()
        {
            InitializeComponent();

            LoadColumnSettings();

            MainWindow.OnWindowClosingEventHandler += new MainWindow.WindowClosing(UpdateColumnSettings);
        }

        #region Column settings
        /// <summary>
        /// Loads column settings for ReeferDataTable from settings.settings
        /// </summary>
        private void LoadColumnSettings()
        {
            var displayIndexes = Properties.Settings.Default.ReeferDataTableDisplayIndex.Split(';');
            var widths = Properties.Settings.Default.ReeferDataTableWidth.Split(';');
            var visibilitys = Properties.Settings.Default.ReeferDataTableVisibilities.Split(';');

            if (displayIndexes.Count() != MainReeferDataTable.Columns.Count) return;

            try
            {
                for (int i = 0; i < displayIndexes.Count(); i++)
                {
                    MainReeferDataTable.Columns[i].DisplayIndex = int.Parse(displayIndexes[i]);
                    MainReeferDataTable.Columns[i].Width = double.Parse(widths[i]);
                    MainReeferDataTable.Columns[i].Visibility = (System.Windows.Visibility)Enum.Parse(typeof(System.Windows.Visibility), visibilitys[i]);
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("-----> Restoring of ReefererDataTable column settings caused an exception");
                int i = 0;
                foreach (var column in MainReeferDataTable.Columns)
                {
                    column.DisplayIndex = i;
                    column.Width = DataGridLength.Auto;
                    column.Visibility = Visibility.Visible;
                    i++;
                }
                Debug.WriteLine("-----> Default columns in ReeferDataTable created");
            }
        }

        /// <summary>
        /// Updates settings.settings with ReeferDataTable column settings
        /// </summary>
        private void UpdateColumnSettings()
        {
            List<int> displayIndexes = new List<int>();
            List<double> widths = new List<double>();
            List<string> visibilitys = new List<string>();

            foreach (var column in MainReeferDataTable.Columns)
            {
                displayIndexes.Add(column.DisplayIndex);
                widths.Add(column.ActualWidth);
                visibilitys.Add(column.Visibility.ToString());
            }

            Properties.Settings.Default.ReeferDataTableDisplayIndex = String.Join(";", displayIndexes);
            Properties.Settings.Default.ReeferDataTableWidth = String.Join(";", widths);
            Properties.Settings.Default.ReeferDataTableVisibilities = string.Join(";", visibilitys);
        }

        #endregion

        private void ExportToExcel(object sender, RoutedEventArgs e)
        {
            ExportDataGridToExcel.ExportToExcel(MainReeferDataTable);
        }
    }
}
