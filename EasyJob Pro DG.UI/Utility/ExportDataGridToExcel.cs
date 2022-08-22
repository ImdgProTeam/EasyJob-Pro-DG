using EasyJob_ProDG.Data;
using EasyJob_ProDG.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Excel = Microsoft.Office.Interop.Excel;

namespace EasyJob_ProDG.UI.Utility
{
    /// <summary>
    /// Class deals with exporting to excel of data as displayed
    /// </summary>
    public static class ExportDataGridToExcel
    {
        private static MainWindowViewModel MainWindowVM => ViewModelLocator.MainWindowViewModel;

        /// <summary>
        /// Exports data from DataGrid as it is displayed.
        /// </summary>
        /// <param name="dataGrid">DataGrid containing data to export.</param>
        /// <param name="addSummary">If true, total sum of each column (starting from B) will be added after the last row.</param>
        public static void ExportToExcel(DataGrid dataGrid, bool addSummary = false)
        {
            MainWindowVM.StatusBarControl.StartProgressBar(10, "Exporting...");
            MainWindowVM.SetIsLoading(true);

            ListCollectionView collection = CollectionViewSource.GetDefaultView(dataGrid.ItemsSource) as ListCollectionView;
            if (collection == null) return;

            ColumnProperty[] columnProperties = new ColumnProperty[dataGrid.Columns.Count];

            MainWindowVM.StatusBarControl.ChangeBarSet(20);

            //set correct columns order
            foreach (var column in dataGrid.Columns)
            {
                if (column.Visibility != Visibility.Visible)
                    continue;
                int index = column.DisplayIndex;
                try
                {
                    columnProperties[index] = new ColumnProperty();
                    columnProperties[index].ColumnHeader = column.Header?.ToString() ?? null;
                    columnProperties[index].ColumnPropertyName = String.IsNullOrEmpty(column.Header?.ToString()) ? String.Empty
                        : PropertiesDictionary[column.Header?.ToString()] ?? column.Header?.ToString();
                    columnProperties[index].ColumnWidth = (int)column.ActualWidth / 8;
                }
                catch
                {
                    columnProperties[index] = null;
                }
            }

            //setting to set status bar increment value for a single row
            int statusBarIncrementValue = 50 / dataGrid.Columns.Count;

            MainWindowVM.StatusBarControl.ChangeBarSet(25);

            //Excel
            Task.Run(() =>
            { //Column headers and width

                Excel.Application excel = new Excel.Application();
                excel.Visible = false;
                Excel.Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
                Excel.Worksheet sheet = (Excel.Worksheet)workbook.Sheets[1];
                MainWindowVM.StatusBarControl.ChangeBarSet(40);

                int subtraction = 0;

                for (int i = 0; i < dataGrid.Columns.Count; i++)
                {
                    //clear invisible columns
                    if (columnProperties[i] == null)
                    {
                        //Status bar update
                        if (MainWindowVM.StatusBarControl.ProgressPercentage < 95)
                            MainWindowVM.StatusBarControl.ProgressPercentage += statusBarIncrementValue;

                        subtraction++;
                        continue;
                    }

                    Excel.Range range = (Excel.Range)sheet.Cells[1, i + 1 - subtraction];
                    sheet.Cells[1, i + 1 - subtraction].Font.Bold = true;
                    sheet.Columns[i + 1 - subtraction].ColumnWidth = columnProperties[i].ColumnWidth;
                    if (string.IsNullOrEmpty(columnProperties[i].ColumnPropertyName))
                    {
                        //Status bar update
                        if (MainWindowVM.StatusBarControl.ProgressPercentage < 95)
                            MainWindowVM.StatusBarControl.ProgressPercentage += statusBarIncrementValue;
                        
                        subtraction++;
                        continue;
                    }

                    range.Value2 = columnProperties[i].ColumnHeader;


                    //create records
                    for (int j = 0; j < collection.Count; j++)
                    {
                        string value;
                        try
                        {
                            var item = collection.GetItemAt(j);
                            PropertyInfo property = item.GetType().GetProperty(columnProperties[i].ColumnPropertyName);
                            if (property == null) continue;

                            if (Object.ReferenceEquals(property?.PropertyType, typeof(Boolean)))
                            {
                                value = (bool)property?.GetValue(item) ? "Y" : "";
                            }
                            else
                            {
                                value = property?.GetValue(item)?.ToString();
                            }
                            range = (Excel.Range)sheet.Cells[j + 2, i + 1 - subtraction];
                            range.Value2 = value;
                        }
                        catch (Exception ex)
                        {
                            LogWriter.Write($"Exception {ex.Message} called while exporting dataGrid to excel.");
                        }
                    }

                    //Status bar update
                    if (MainWindowVM.StatusBarControl.ProgressPercentage < 95)
                        MainWindowVM.StatusBarControl.ProgressPercentage += statusBarIncrementValue;
                    
                }


                //Summary
                if (addSummary)
                {
                    try
                    {
                        //Row header
                        Excel.Range range = (Excel.Range)sheet.Cells[collection.Count + 2, 1];
                        range.Value2 = "Total:";
                        range.HorizontalAlignment = HorizontalAlignment.Right;
                        range.Font.Bold = true;

                        for (int i = 2; i < dataGrid.Columns.Count + 1 - subtraction; i++)
                        {
                            range = (Excel.Range)sheet.Cells[collection.Count + 2, i];
                            string formula = $"=SUM({(char)(i + 64)}2:{(char)(i + 64)}{collection.Count + 1})";
                            range.Formula = formula;
                        }
                    }
                    catch (Exception ex)
                    {
                        LogWriter.Write($"Exception {ex.Message} called while creating summary row in excel.");
                    }
                }

                excel.Visible = true;

                MainWindowVM.StatusBarControl.ChangeBarSet(100);
                MainWindowVM.SetIsLoading(false);
            });

        }


        /// <summary>
        /// Dictionary used to match column headers with item properties.
        /// </summary>
        private static readonly Dictionary<string, string> PropertiesDictionary = new Dictionary<string, string>()
        {
            {"Commodity", "Commodity" },
            {"Container number", "ContainerNumber"},
            {"Container type", "ContainerType"},
            {"Contains Dg cargo", "ContainsDgCargo" },
            {"Dg class", "DgClass"},
            {"Dg subclass", "DgSubclass"},
            {"Emergency contact", "EmergencyContacts"},
            {"EmS", "DgEMS"},
            {"Final destination", "FinalDestination"},
            {"Flash point", "FlashPoint"},
            {"Hold number", "HoldNr" },
            {"Load temperature", "LoadTemperature" },
            {"LQ", "IsLq"},
            {"Marine pollutant", "IsMp"},
            {"Max 1L", "IsMax1L"},
            {"Net weight", "DgNetWeight"},
            {"Number and type of Packages", "NumberAndTypeOfPackages"},
            {"Old Position", "LocationBeforeRestow"},
            {"Open type", "IsOpen"},
            {"Operator", "Carrier"},
            {"Packing group", "PackingGroup"},
            {"POD", "POD"},
            {"POL", "POL"},
            {"Position", "Location"},
            {"Proper shipping name", "Name"},
            {"Remark", "ReeferRemark" },
            {"Remarks", "Remarks"},
            {"Segregation group", "SegregationGroup"},
            {"Set point", "SetTemperature" },
            {"Special", "ReeferSpecial" },
            {"Technical name", "TechnicalName"},
            {"UNNO", "Unno"},
            {"Vent", "VentSetting" },
            {"Waste", "IsWaste"},

            // Cargo summary
            {"Port code", "Port" },
            {"Containers", "Containers" },
            {"Dg containers", "DgContainers" },
            {"Reefers", "Rf" },
            {"Dg net weight", "DgNetWt" },
            {"Marine pollutants weight", "MP" },
        };

        /// <summary>
        /// Accessory class to support correct column order.
        /// </summary>
        private class ColumnProperty
        {
            internal int ColumnWidth;
            internal string ColumnHeader;
            internal string ColumnPropertyName;

            internal ColumnProperty()
            {
            }
        }
    }
}
