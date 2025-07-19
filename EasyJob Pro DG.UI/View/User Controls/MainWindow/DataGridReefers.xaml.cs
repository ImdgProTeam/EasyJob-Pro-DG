using EasyJob_ProDG.UI.IO;
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
    public partial class DataGridReefers : CommonDataGridUserControl
    {
        public DataGridReefers() : base(Animations.AnimationTypes.None)
        {
            InitializeComponent();

            MainDataTable = MainReeferDataTable;
            CallBaseConstructorMethods();
        }

        #region Column settings
        /// <summary>
        /// Loads column settings for ReeferDataTable from settings.settings
        /// </summary>
        internal override void LoadColumnSettings()
        {
            var displayIndexes = Properties.Settings.Default.ReeferDataTableDisplayIndex.Split(';');
            var widths = Properties.Settings.Default.ReeferDataTableWidth.Split(';');
            var visibilitys = Properties.Settings.Default.ReeferDataTableVisibilities.Split(';');

            if (displayIndexes.Count() != MainReeferDataTable.Columns.Count) return;

            try
            {
                int index;
                double width;

                for (int i = 0; i < displayIndexes.Count(); i++)
                {
                    index = int.Parse(displayIndexes[i]);
                    if (index < 0) throw new ArgumentOutOfRangeException(nameof(index), "Negative display index");
                    MainReeferDataTable.Columns[i].DisplayIndex = index;

                    width = double.Parse(widths[i]);
                    if (width < 0) throw new ArgumentOutOfRangeException(nameof(width), "Negative column width");
                    MainReeferDataTable.Columns[i].Width = width;

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
        internal override void SaveColumnSettings()
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


        #region Export to Excel
        private void ExportToExcel(object sender, RoutedEventArgs e)
        {
            ExportDataGridToExcel.ExportToExcel(MainReeferDataTable);
        }

        #endregion

    }
}
