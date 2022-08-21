using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using EasyJob_ProDG.Data.Info_data;
using EasyJob_ProDG.Model.Cargo;
using ExcelApp = Microsoft.Office.Interop.Excel;

namespace EasyJob_ProDG.Model.IO.Excel
{

    public static partial class WithXl
    {
        static ExcelTemplate template = new ExcelTemplate();

        #region Columns enumeration
        public static readonly List<char> Columns = new List<char>() { '0', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        #endregion

        //------------------- Import - Export methods -------------------------------------------------------------------------

        /// <summary>
        /// Method exports the list of Dg into Excel in default format.
        /// </summary>
        /// <param name="dgList">DgList to be exported.</param>
        public static void Export(List<Dg> dgList)
        {
            bool isTemplateRead = template.ReadTemplate();

            //Change template or use default
            if (isTemplateRead)
            {
                Output.ThrowMessage(
                    "Template will be used to create dg list in excel.");
            }

            //Creating workbook
            ExcelApp.Application excelApp = new ExcelApp.Application { Visible = false, SheetsInNewWorkbook = 1 };
            excelApp.Workbooks.Add(Type.Missing);
            ExcelApp.Worksheet excelWorkSheet = excelApp.ActiveWorkbook.ActiveSheet;
            excelWorkSheet.Name = template.WorkingSheet;
            ExcelApp.Range excelCells;

            //List of headings
            List<string> titles = new List<string>()
            {
                "","NN", "Position","Container number","POL","POD","UNNO","Class","Subclass","Proper shipping name","PKG","FP","MP","LQ","EMS","Remarks","Net weight",
                "Technical Name", "Number and type of packages", "Final destination", "Operator", "Emergency contact"
            };

            //List of column widths
            List<int> columnWidth = new List<int>()
            {
                0,6,0,18,0,0,0,0,0,45,5,5,5,5,0,30,20,45,30,0,0,18

            };

            #region Headings row
            //Creating heading titles
            for (int i = 1; i <= template.MaxColumnNumber; i++)
            {
                excelCells = (ExcelApp.Range)excelWorkSheet.Cells[template.StartRow - 1, i];
                excelCells.Font.Bold = true;
                excelCells.HorizontalAlignment = ExcelApp.Constants.xlCenter;
                excelCells.Borders.LineStyle = ExcelApp.XlLineStyle.xlContinuous;
                excelCells.WrapText = true;
                int x = GetColumnNr(i);
                if (columnWidth[x] != 0) excelCells.ColumnWidth = columnWidth[x];
                excelCells.Value2 = titles[x];
            }
            #endregion

            #region Filling table
            //Filling information into the table
            for (int excelrow = 1; excelrow <= dgList.Count; excelrow++)
            {
                Dg dg = dgList[excelrow - 1];
                for (int excelcol = 1; excelcol <= template.MaxColumnNumber; excelcol++)
                {
                    string value = null;
                    excelCells = (ExcelApp.Range)excelWorkSheet.Cells[excelrow + template.StartRow - 1, excelcol];
                    excelCells.VerticalAlignment = ExcelApp.Constants.xlTop;
                    switch (GetColumnNr(excelcol))
                    {
                        case 1:
                            value = excelrow.ToString();
                            break;
                        case 2:
                            value = dg.Location;
                            excelCells.NumberFormat = "000000";
                            break;
                        case 3:
                            value = dg.ContainerNumber;
                            excelCells.HorizontalAlignment = ExcelApp.Constants.xlLeft;
                            break;
                        case 4:
                            value = dg.POL;
                            excelCells.HorizontalAlignment = ExcelApp.Constants.xlRight;
                            break;
                        case 5:
                            value = dg.POD;
                            excelCells.HorizontalAlignment = ExcelApp.Constants.xlRight;
                            break;
                        case 6:
                            value = dg.Unno.ToString();
                            excelCells.NumberFormat = "0000";
                            break;
                        case 7:
                            excelCells.NumberFormat = "@";
                            value = dg.DgClass.ToString(CultureInfo.InvariantCulture);
                            excelCells.HorizontalAlignment = ExcelApp.Constants.xlRight;
                            break;
                        case 8:
                            excelCells.NumberFormat = "@";
                            value = dg.DgSubclass;
                            excelCells.HorizontalAlignment = ExcelApp.Constants.xlRight;
                            break;
                        case 9:
                            value = dg.Name;
                            excelCells.WrapText = true;
                            break;
                        case 10:
                            value = dg.PackingGroup;
                            excelCells.HorizontalAlignment = ExcelApp.Constants.xlCenter;
                            break;
                        case 11:
                            value = Math.Abs(dg.FlashPointDouble - 9999) < 1 ? "" : dg.FlashPoint;
                            excelCells.NumberFormat = "0.0";
                            break;
                        case 12:
                            value = dg.IsMp ? "P" : "";
                            break;
                        case 13:
                            value = dg.IsLq ? "LQ" : "";
                            break;
                        case 14:
                            value = dg.DgEMS;
                            break;
                        case 15:
                            if (dg.Liquid) value = "Liquid";
                            if (dg.Flammable) value += value == null ? "Flammable" : ", flammable";
                            if (dg.EmitFlammableVapours)
                                value += value == null ? "Emitting flammable vapours" : ", emitting flammable vapours";
                            if (dg.SegregationGroupList.Count == 0) continue;
                            foreach (var x in dg.SegregationGroupList)
                            {
                                if (value != null) value += ", ";
                                value += IMDGCode.SegregationGroups[x];
                            }
                            if (!dg.IsClosed) value += value == null ? "Open" : ", open";
                            value += (string.IsNullOrEmpty(value) ? "" : "\n") + dg.Remarks;
                            excelCells.WrapText = true;
                            break;
                        case 16:
                            value = dg.DgNetWeight.ToString();
                            excelCells.NumberFormat = "0.000";
                            break;
                        case 17:
                            value = dg.TechnicalName;
                            excelCells.WrapText = true;
                            break;
                        case 18:
                            value = dg.NumberAndTypeOfPackages;
                            excelCells.WrapText = true;
                            break;
                        case 19:
                            value = dg.FinalDestination;
                            excelCells.HorizontalAlignment = ExcelApp.Constants.xlRight;
                            break;
                        case 20:
                            value = dg.Carrier;
                            excelCells.HorizontalAlignment = ExcelApp.Constants.xlRight;
                            break;
                        case 21:
                            value = dg.EmergencyContacts;
                            break;
                        default:
                            value = "";
                            break;
                    }

                    excelCells.Value2 = value;
                }
            }
            #endregion

            excelApp.Visible = true;
        }

        /// <summary>
        /// Method imports Dg list from Excel sheet with all the fields according to Template.
        /// Returns Dg List.
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="ship"></param>
        /// <param name="containers"></param>
        /// <returns></returns>
        public static List<Dg> Import(string workbook, Transport.ShipProfile ship, out ICollection<Container> containers)
        {
            List<Dg> resultDgList = new List<Dg>();
            containers = new List<Container>();


            bool isTemplateRead = template.ReadTemplate();

            if (isTemplateRead)
            {
                Output.ThrowMessage(
                    "Template will be used to read excel file. Press any key to continue. " +
                    "\nTo change the template press '1'");
            }

            //connecting xl file
            ExcelApp.Range excelcells = null;
            ExcelApp.Application excelapp = new ExcelApp.Application { Visible = false };
            ExcelApp.Workbooks workbooks = null;
            ExcelApp.Workbook activeWorkbook = null;
            ExcelApp.Worksheet excelWorksheet = null;

            try
            {
                workbooks = excelapp.Workbooks;
                workbooks.Open(workbook, ExcelApp.XlUpdateLinks.xlUpdateLinksNever, ReadOnly: true);
                activeWorkbook = excelapp.ActiveWorkbook;
                excelWorksheet = ChooseCorrectSheet(activeWorkbook, template.WorkingSheet);
                Data.LogWriter.Write("Reading DG data...");

                //Determine number of rows = number of dg
                int rowscount = CountRows(ref excelcells, excelWorksheet);

                //Create dg list & container list
                for (int line = 0; line < rowscount; line++)
                {
                    int row = template.StartRow + line;
                    Dg unit = new Dg();
                    Container cont = new Container();
                    for (int col = 1; col <= template.MaxColumnNumber; col++)
                    {
                        excelcells = excelWorksheet.Cells[row, col];
                        if (excelcells.Value2 == null) continue;

                        if (col == template.ColumnContainerNumber) cont.ContainerNumber = excelcells.Value2;
                        else if (col == template.ColumnLocation) cont.Location = Convert.ToString(excelcells.Value2);
                        else if (col == template.ColumnPOL) cont.POL = excelcells.Value2;
                        else if (col == template.ColumnPOD) cont.POD = excelcells.Value2;
                        else if (col == template.ColumnUnno)
                        {
                            unit.Unno = Convert.ToUInt16(excelcells.Value2);
                            unit.AssignSegregationGroup();
                        }
                        else if (col == template.ColumnClass)
                        {
                            unit.DgClass = WithXlAssistToRead.DgClass(excelcells, unit, excelapp);
                            if (!string.IsNullOrEmpty(unit.DgClass)) unit.AssignSegregationTableRowNumber();
                        }
                        else if (col == template.ColumnSubclass)
                        {
                            unit.DgSubclassArray = WithXlAssistToRead.DgSubClass(excelcells, unit, excelapp);
                        }
                        else if (col == template.ColumnName) unit.Name = excelcells.Value2;
                        else if (col == template.ColumnPkg) unit.PackingGroup = excelcells.Value2.ToString();
                        else if (col == template.ColumnMP)
                        {
                            if (excelcells.Value2 == "true" || excelcells.Value2 == "Y" || excelcells.Value2 == "P")
                                unit.IsMp = true;
                            if (excelcells.Value2 != null) unit.mpDetermined = true;
                        }
                        else if (col == template.ColumnLQ)
                        {
                            if (excelcells.Value2.ToString().ToLower() == "true" ||
                                excelcells.Value2.ToString().ToLower() == "y" ||
                                excelcells.Value2.ToString().ToLower() == "lq")
                                unit.IsLq = true;
                        }
                        else if (col == template.ColumnFP)
                        {
                            unit.FlashPointDouble = WithXlAssistToRead.DgFp(excelcells);
                        }
                        else if (col == template.ColumnEms) unit.DgEMS = excelcells.Value2;
                        //the following parses 'Remark' column and reads properties and segregation groups, if any
                        else if (col == template.ColumnRemark) WithXlAssistToRead.ParseRemarkColumn(excelcells.Value2, unit, cont);
                        else if (col == template.ColumnNetWeight)
                        {
                            decimal tempValue = 0.0M;
                            string tmp = excelcells.Value2.ToString();
                            bool success = decimal.TryParse(tmp, out tempValue);
                            unit.DgNetWeight = (decimal)tempValue;
                        }
                    }

                    cont.HoldNr = ship.DefineCargoHoldNumber(cont.Bay);
                    unit.CopyContainerInfo(cont);
                    resultDgList.Add(unit);

                    //Update containers
                    var container = containers.FirstOrDefault(c => string.Equals(c.ContainerNumber, cont.ContainerNumber, StringComparison.OrdinalIgnoreCase));
                    if (container == null)
                    {
                        cont.DgCountInContainer++;
                        containers.Add(cont);
                    }
                    else container.DgCountInContainer++;
                }
            }
            catch (Exception ex)
            {
                Data.LogWriter.Write($"Attempt to read excel workbook {workbook} thrown an exception {ex.Message}.");
            }
            finally
            {
                activeWorkbook.Close(null, null, null);
                if (workbooks != null) workbooks.Close();

                excelapp.Quit();
                RunGarbageCollector();
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excelcells);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excelWorksheet);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(activeWorkbook);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(workbooks);


                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excelapp);

            }

            return resultDgList;
        }


        //------------------- Assisting methods ------------------------------------------------------------------------------

        /// <summary>
        /// Counts number of rows containing Dg data in excel sheet.
        /// </summary>
        /// <param name="excelCells"></param>
        /// <param name="excelWorksheet"></param>
        /// <returns></returns>
        internal static int CountRows(ref ExcelApp.Range excelCells, ExcelApp.Worksheet excelWorksheet, int startRow = 0)
        {
            bool stop = false;
            int rowsCount = 0;
            int startRowUsed = startRow == 0 ? template.StartRow : startRow;

            while (!stop)
            {
                excelCells = excelWorksheet.Cells[startRowUsed + rowsCount, startRow == 0 ? template.ColumnContainerNumber : 1];
                stop = excelCells.Value2 == null;
                if (!stop) rowsCount++;
            }

            return rowsCount;
        }

        /// <summary>
        /// Method deals with multiple sheets in excel workbook
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="templateName"></param>
        /// <returns></returns>
        private static ExcelApp.Worksheet ChooseCorrectSheet(ExcelApp.Workbook workbook, string templateName)
        {
            //Choosing worksheet
            ExcelApp.Worksheet excelWorksheet = null;

            //First check if there is only one sheet
            if (workbook.Sheets.Count == 1) excelWorksheet = workbook.Sheets[1];

            //Then try to use given name in template
            else
            {
                try
                {
                    excelWorksheet = workbook.Sheets[templateName];
                }
                //...if not matching create a list of sheets and offer a choice
                catch (Exception)
                {
                    //TODO: Generate list of excel worksheets in a book for opening
                    return workbook.Sheets[1];
                    //int i = 1;
                    //foreach (Excel.Worksheet sheet in workbook.Worksheets)
                    //{
                    //    Output.DisplayLine("{0}. {1}", i, sheet.Name);
                    //    i++;
                    //}

                    //while (excelWorksheet == null)
                    //    try
                    //    {
                    //        Style.FontColor("Gray");
                    //        string choise = Output.ReadLine();
                    //        if (choise != null && choise.Length == 1 && char.IsDigit(choise[0]))
                    //            excelWorksheet = workbook.Worksheets[Convert.ToInt16(choise)];
                    //        else
                    //            excelWorksheet = workbook.Worksheets[choise];
                    //    }
                    //    catch
                    //    {
                    //        Output.DisplayLine("Unrecognized command. Please, choose excel work sheet from the list.");
                    //    }
                }
            }

            return excelWorksheet;
        }

        /// <summary>
        /// Assisting class to establish which information is assigned to the column i.
        /// Literally translates default order into order according to the template
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private static int GetColumnNr(int i)
        {
            if (i == template.ColumnLocation) return 2;
            else if (i == template.ColumnContainerNumber) return 3;
            else if (i == template.ColumnPOL) return 4;
            else if (i == template.ColumnPOD) return 5;
            else if (i == template.ColumnUnno) return 6;
            else if (i == template.ColumnClass) return 7;
            else if (i == template.ColumnSubclass) return 8;
            else if (i == template.ColumnName) return 9;
            else if (i == template.ColumnPkg) return 10;
            else if (i == template.ColumnFP) return 11;
            else if (i == template.ColumnMP) return 12;
            else if (i == template.ColumnLQ) return 13;
            else if (i == template.ColumnEms) return 14;
            else if (i == template.ColumnRemark) return 15;
            else if (i == template.ColumnNetWeight) return 16;
            else if (i == template.ColumnTechName) return 17;
            else if (i == template.ColumnPackage) return 18;
            else if (i == template.ColumnFinalDestination) return 19;
            else if (i == template.ColumnOperator) return 20;
            else if (i == template.ColumnEmergencyContact) return 21;
            else return 0;
        }


        /// <summary>
        /// Runs implicitly garbage collector.
        /// </summary>
        private static void RunGarbageCollector()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
