﻿using EasyJob_ProDG.Data;
using EasyJob_ProDG.Data.Info_data;
using EasyJob_ProDG.UI.IO;
using EasyJob_ProDG.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace EasyJob_ProDG.UI.View.User_Controls
{
    /// <summary>
    /// Логика взаимодействия для DgDataGrid.xaml
    /// </summary>
    public partial class DataGridDg : CommonDataGridUserControl
    {
        private double OriginalScrollPosition { get; set; }
        private bool IsResizingColumn { get; set; }


        public DataGridDg() : base(Animations.AnimationTypes.SlideAndFadeInFromLeft)
        {
            InitializeComponent();

            MainDataTable = MainDgTable;
            CallBaseConstructorMethods();
        }


        #region Column settings

        /// <summary>
        /// Loads column settings for DgDataTable from settings.settings
        /// </summary>
        internal override void LoadColumnSettings()
        {
            var displayIndexes = Properties.Settings.Default.DgDataTableDisplayIndex.Split(';');
            var widths = Properties.Settings.Default.DgDataTableWidth.Split(';');
            var visibilitys = Properties.Settings.Default.DgDataTableVisibilities.Split(';');

            if (displayIndexes.Count() != MainDgTable.Columns.Count) return;

            try
            {
                int index;
                double width;

                for (int i = 0; i < displayIndexes.Count(); i++)
                {
                    index = int.Parse(displayIndexes[i]);
                    if (index < 0) throw new ArgumentOutOfRangeException(nameof(index), "Negative display index");
                    MainDgTable.Columns[i].DisplayIndex = index;

                    width = double.Parse(widths[i]);
                    if (width < 0) throw new ArgumentOutOfRangeException(nameof(width), "Negative column width");
                    MainDgTable.Columns[i].Width = width;

                    MainDgTable.Columns[i].Visibility = (System.Windows.Visibility)Enum.Parse(typeof(System.Windows.Visibility), visibilitys[i]);
                }
            }
            catch (Exception ex)
            {
                LogWriter.Write($"-----> Restoring of DgDataTable column settings caused an exception: {ex.Message}");
                int i = 0;
                foreach (var column in MainDgTable.Columns)
                {
                    column.DisplayIndex = i;
                    column.Width = DataGridLength.Auto;
                    column.Visibility = Visibility.Visible;
                    i++;
                }
                LogWriter.Write("-----> Default columns in DgDataTable created");
            }
        }

        /// <summary>
        /// Updates settings.settings with DgDataTable actual column settings
        /// </summary>
        internal override void SaveColumnSettings()
        {
            List<int> displayIndexes = new List<int>();
            List<double> widths = new List<double>();
            List<string> visibilitys = new List<string>();

            foreach (var column in MainDgTable.Columns)
            {
                displayIndexes.Add(column.DisplayIndex);
                widths.Add(column.ActualWidth);
                visibilitys.Add(column.Visibility.ToString());
            }

            Properties.Settings.Default.DgDataTableDisplayIndex = String.Join(";", displayIndexes);
            Properties.Settings.Default.DgDataTableWidth = String.Join(";", widths);
            Properties.Settings.Default.DgDataTableVisibilities = string.Join(";", visibilitys);
        }

        #endregion


        #region Input and cell / text select logic

        internal override void MainDataTable_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            DataGrid grid = sender as DataGrid;
            DataGridColumn column = grid?.CurrentColumn;

            base.MainDataTable_PreviewKeyDown(sender, e);

            DgWrapper dg = grid?.SelectedItem as DgWrapper;

            string strKey = e.Key.ToString();
            string trimmedStrKey;
            if (strKey.Length > 1 && (strKey.StartsWith("D") || strKey.StartsWith("NumPad")))
            {
                trimmedStrKey = strKey.Replace("D", "").Replace("NumPad", "");
            }
            else
            {
                trimmedStrKey = strKey;
            }
            bool isCharKey = Char.TryParse(trimmedStrKey, out var key);

            //Stowage category validation
            if (column?.Header != null && column.Header.ToString() == "Stowage category")
            {
                if (!isCharKey) return;
                if (IMDGCode.AllValidStowageCategories.Contains(key))
                {
                    if (dg != null) dg.StowageCat = key;
                }
            }

            //Proper shipping name
            if (column?.Header != null && column.Header.ToString() == "Proper shipping name")
            {
                if (!isCharKey) return;
            }
        }

        /// <summary>
        /// Selects all text in TextBox
        /// </summary>
        /// <param name="txb"></param>
        private void SelectAllText(TextBox txb)
        {
            try
            {
                txb.SelectAll();
            }
            finally
            {

            }
        }

        private void txbNameColumn_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 3)
            {
                SelectAllText(sender as TextBox);
                return;
            }

        }

        private void txbNameColumn_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            try
            {
                SelectAllText((TextBox)sender);
            }
            finally
            {

            }
        }

        #endregion


        #region Scroll logic

        private void DataGrid_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            if (this.IsResizingColumn
              && TryFindVisualChildElement(dataGrid, out ScrollViewer scrollViewer))
            {
                this.Dispatcher.InvokeAsync(() =>
                {
                    scrollViewer.ScrollToHorizontalOffset(this.OriginalScrollPosition);
                    this.IsResizingColumn = false;
                }, DispatcherPriority.Background);
            }
        }
        private void DataGridCell_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var dataGridCell = sender as DataGridCell;
            if (TryFindVisualChildElement(this.MainDgTable, out ScrollViewer scrollViewer))
            {
                this.OriginalScrollPosition = scrollViewer.HorizontalOffset;
                this.IsResizingColumn = true;
            }
        }

        private bool TryFindVisualChildElement<TChild>(DependencyObject parent, out TChild resultElement)
          where TChild : DependencyObject
        {
            resultElement = null;

            if (parent is Popup popup)
            {
                parent = popup.Child;
                if (parent == null)
                {
                    return false;
                }
            }

            for (var childIndex = 0; childIndex < VisualTreeHelper.GetChildrenCount(parent); childIndex++)
            {
                DependencyObject childElement = VisualTreeHelper.GetChild(parent, childIndex);

                if (childElement is TChild child)
                {
                    resultElement = child;
                    return true;
                }

                if (TryFindVisualChildElement(childElement, out resultElement))
                {
                    return true;
                }
            }

            return false;
        }

        #endregion


        #region Export to excel

        /// <summary>
        /// Exports MainDataGrid as it is displayed to excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExportToExcel(object sender, RoutedEventArgs e)
        {
            ExportDataGridToExcel.ExportToExcel(MainDgTable);
        }

        #endregion


        /// <summary>
        /// Method paints each alternate column in DataGrid with specified color
        /// </summary>
        /// <param name="color"></param>
        private void PaintAlternateColumn(Color color)
        {
            int alt = 0;
            for (byte i = 0; i < MainDgTable.Columns.Count; i++)
            {
                var column = MainDgTable.Columns.FirstOrDefault(x => x.DisplayIndex == i);
                if (column?.Visibility == Visibility.Visible)
                {
                    if (alt == 0)
                    {
                        alt = 1;
                        continue;
                    }

                    FrameworkElement cellContent;
                    for (int rowIndex = 0; rowIndex < MainDgTable.Items.Count; rowIndex++)
                    {
                        cellContent = column.GetCellContent(MainDgTable.Items[rowIndex]);
                        if (cellContent?.Parent is DataGridCell cell) cell.Background = new SolidColorBrush(color);
                    }
                    alt = 0;
                }
            }
        }

    }
}
