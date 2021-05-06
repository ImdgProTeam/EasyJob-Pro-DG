using EasyJob_ProDG.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Excel = Microsoft.Office.Interop.Excel;

namespace EasyJob_ProDG.UI.Utility
{
    public static class ExportDataGridToExcel
    {
        /// <summary>
        /// Exports DataGrid data to excel in the same order as on the screen
        /// </summary>
        /// <param name="dataGrid">DataGrid with DgWrappers or ContainerWrappers.</param>
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
                    columnProperties[index].ColumnHeader = column.Header.ToString();
                    columnProperties[index].ColumnPropertyName = PropertiesDictionary[column.Header.ToString()];
                    columnProperties[index].ColumnWidth = (int) column.ActualWidth / 8;
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
            }

            //filling the data into the table
            subtraction = 0;
            for (int i = 0; i < dataGrid.Columns.Count; i++)
            {
                //skip if column is not mentioned in dictionary
                if (columnProperties[i] == null)
                {
                    subtraction++;
                    continue;
                }

                //create records
                for (int j = 0; j < collection.Count - 1; j++)
                {
                    string value;
                    try
                    {
                        DgWrapper dg = (DgWrapper)collection.GetItemAt(j);
                        PropertyInfo property = dg.GetType().GetProperty(columnProperties[i].ColumnPropertyName);
                        if (property == null) continue;

                        if (Object.ReferenceEquals(property?.PropertyType, typeof(Boolean)))
                        {
                            value = (bool)property?.GetValue(dg) ? "Y" : "";
                        }
                        else
                        {
                            value = property?.GetValue(dg)?.ToString();
                        }
                        Excel.Range range = (Excel.Range)sheet.Cells[j + 2, i + 1 - subtraction];
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
        /// Dictionary used to match column headers with DgWrapper properties.
        /// </summary>
        private static readonly Dictionary<string, string> PropertiesDictionary = new Dictionary<string, string>()
        {
            {"Position", "Location"},
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
            {"Remarks", "Remarks"}

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
