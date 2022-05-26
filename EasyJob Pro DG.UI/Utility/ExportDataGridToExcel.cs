using System;
using System.Collections.Generic;
using System.Reflection;
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
        /// <summary>
        /// Exports data from DataGrid as it is displayed.
        /// </summary>
        /// <param name="dataGrid">DataGrid containing data to export.</param>
        public static void ExportToExcel(DataGrid dataGrid)
        {
            ListCollectionView collection = CollectionViewSource.GetDefaultView(dataGrid.ItemsSource) as ListCollectionView;
            if (collection == null) return;

            Excel.Application excel = new Excel.Application();
            excel.Visible = false;
            Excel.Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
            Excel.Worksheet sheet = (Excel.Worksheet)workbook.Sheets[1];

            ColumnProperty[] columnProperties = new ColumnProperty[dataGrid.Columns.Count];

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
                    columnProperties[index].ColumnPropertyName = PropertiesDictionary[column.Header?.ToString()] ?? column.Header?.ToString();
                    columnProperties[index].ColumnWidth = (int)column.ActualWidth / 8;
                }
                catch
                {
                    columnProperties[index] = null;
                }
            }

            //Column headers and width
            int subtraction = 0;
            for (int i = 0; i < dataGrid.Columns.Count; i++)
            {
                //clear invisible columns
                if (columnProperties[i] == null)
                {
                    subtraction++;
                    continue;
                }

                Excel.Range range = (Excel.Range)sheet.Cells[1, i + 1 - subtraction];
                sheet.Cells[1, i + 1 - subtraction].Font.Bold = true;
                sheet.Columns[i + 1 - subtraction].ColumnWidth = columnProperties[i].ColumnWidth;
                if (string.IsNullOrEmpty(columnProperties[i].ColumnPropertyName))
                {
                    subtraction++;
                    continue;
                }

                range.Value2 = columnProperties[i].ColumnHeader;


                //create records
                for (int j = 0; j < collection.Count - 1; j++)
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
                    catch
                    {
                        //ignore
                    }
                }
            }
            excel.Visible = true;
        }


        /// <summary>
        /// Dictionary used to match column headers with item properties.
        /// </summary>
        private static readonly Dictionary<string, string> PropertiesDictionary = new Dictionary<string, string>()
        {
            {"Position", "Location"},
            {"Hold number", "HoldNr" },
            {"Container number", "ContainerNumber"},
            {"Operator", "Carrier"},
            {"POL", "POL"},
            {"POD", "POD"},
            {"Dg class", "DgClass"},
            {"UNNO", "Unno"},

            {"Final destination", "FinalDestination"},
            {"Dg subclass", "DgSubclass"},
            {"Proper shipping name", "Name"},
            {"Packing group", "PackingGroup"},
            {"Flash point", "FlashPoint"},
            {"Net weight", "DgNetWeight"},
            {"LQ", "IsLq"},
            {"Marine pollutant", "IsMp"},
            {"Segregation group", "SegregationGroup"},
            {"Container type", "ContainerType"},
            {"Open type", "IsOpen"},
            {"Old Position", "LocationBeforeRestow"},
            {"Max 1L", "IsMax1L"},
            {"Waste", "IsWaste"},
            {"EmS", "DgEMS"},
            {"Technical name", "TechnicalName"},
            {"Number and type of Packages", "NumberAndTypeOfPackages"},
            {"Emergency contact", "EmergencyContacts"},
            {"Remarks", "Remarks"},
            {"Contains Dg cargo", "ContainsDgCargo" },

            {"Commodity", "Commodity" },
            {"Vent", "VentSetting" },
            {"Set point", "SetTemperature" },
            {"Load temperature", "LoadTemperature" },
            {"Special", "ReeferSpecial" },
            {"Remark", "ReeferRemark" }

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
