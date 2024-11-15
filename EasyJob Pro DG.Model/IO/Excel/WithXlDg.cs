using EasyJob_ProDG.Data.Info_data;
using EasyJob_ProDG.Model.Cargo;
using System;
using System.Collections.Generic;
using System.Linq;
using ExcelApp = Microsoft.Office.Interop.Excel;

namespace EasyJob_ProDG.Model.IO.Excel
{
    public static class WithXlDg
    {
        #region Template

        static ExcelDgTemplate _template;
        static int templateStartRow => _template.StartRow;
        static byte templateMaxColumn => _template.GetMaxColumnNumber();

        /// <summary>
        /// Sets template value to be used.
        /// </summary>
        /// <param name="template"></param>
        internal static void SetTemplate(ExcelDgTemplate template)
        {
            _template = template;
        }

        #endregion


        /// <summary>
        /// Method imports Dg list from Excel sheet with all the fields according to Template.
        /// Returns Dg List.
        /// </summary>
        /// <param name="workbook">excel file full path and name</param>
        /// <param name="dgList">Resulting list of <see cref="Dg"/> as read from the file.</param>
        /// <param name="containers">Resulting list of <see cref="Container"/> as read from the file.</param>
        /// <returns>True if succesfully read till the end.</returns>
        public static bool Import(string workbook, out List<Dg> dgList, out ICollection<Container> containers)
        {
            bool isImported = false;
            dgList = new List<Dg>();
            containers = new List<Container>();


            if (_template == null)
            {
                Data.LogWriter.Write($"Excel template has not been read.");
                throw new System.ArgumentNullException("Excel template not read");
            }
            Data.ProgressBarReporter.ReportPercentage = 30;

            #region Connecting xl file
            //connecting xl file
            Data.LogWriter.Write($"Connecting excel file {workbook}");
            ExcelApp.Range excelcells = null;
            ExcelApp.Application excelapp = new ExcelApp.Application { Visible = false };
            ExcelApp.Workbooks workbooks = null;
            ExcelApp.Workbook activeWorkbook = null;
            ExcelApp.Worksheet excelWorksheet = null;
            #endregion

            try
            {
                workbooks = excelapp.Workbooks;
                workbooks.Open(workbook, ExcelApp.XlUpdateLinks.xlUpdateLinksNever, ReadOnly: true);
                activeWorkbook = excelapp.ActiveWorkbook;
                excelWorksheet = WithXl.ChooseCorrectSheet(activeWorkbook, _template.WorkingSheet);

                Data.LogWriter.Write("Reading DG data...");
                Data.ProgressBarReporter.ReportPercentage = 40;

                //Determine number of rows = number of dg
                int rowscount = WithXl.CountRows(excelWorksheet, _template.StartRow, int.Parse(_template[5]));

                #region StatusBar increment value setup
                //Setting StatusBar increment value
                float tempStatusBarIncrementValue = 30f / rowscount;
                float tempIncrementAccummulation = 0.0f;
                int statusBarIncrementValue = 0;
                if (tempStatusBarIncrementValue > 1)
                    statusBarIncrementValue = 30 / rowscount;
                #endregion

                //in order to handle missing container numbers and locations
                Container lastContainer = new Container();

                //Create dg list & container list
                for (int line = 0; line < rowscount; line++)
                {
                    int row = templateStartRow + line;
                    Dg unit = new Dg();
                    Container cont = new Container();

                    for (int col = 1; col <= templateMaxColumn; col++)
                    {
                        excelcells = excelWorksheet.Cells[row, col];
                        if (excelcells.Value2 == null) continue;
                        string value = excelcells.Value2.ToString().Trim();

                        if (col == _template.GetIntegerValueFromColumnsEnum(ExcelDgTemplate.Columns.colContNr))
                            cont.ContainerNumber = value.Replace(" ", "");
                        else if (col == _template.GetIntegerValueFromColumnsEnum(ExcelDgTemplate.Columns.colLocation))
                            cont.Location = value;
                        else if (col == _template.GetIntegerValueFromColumnsEnum(ExcelDgTemplate.Columns.colPOL))
                            cont.POL = value;
                        else if (col == _template.GetIntegerValueFromColumnsEnum(ExcelDgTemplate.Columns.colPOD))
                            cont.POD = value;
                        else if (col == _template.GetIntegerValueFromColumnsEnum(ExcelDgTemplate.Columns.colUnno))
                        {
                            unit.Unno = Convert.ToUInt16(value);
                            unit.AssignSegregationGroup();
                        }
                        else if (col == _template.GetIntegerValueFromColumnsEnum(ExcelDgTemplate.Columns.colClass))
                        {
                            unit.DgClass = WithXlAssistToRead.DgClass(value, unit.ContainerNumber);
                        }
                        else if (col == _template.GetIntegerValueFromColumnsEnum(ExcelDgTemplate.Columns.colSubclass))
                        {
                            unit.DgSubClassArray = WithXlAssistToRead.DgSubClass(value, unit.ContainerNumber);
                        }
                        else if (col == _template.GetIntegerValueFromColumnsEnum(ExcelDgTemplate.Columns.colName))
                            unit.Name = value;
                        else if (col == _template.GetIntegerValueFromColumnsEnum(ExcelDgTemplate.Columns.colPkg))
                            unit.PackingGroup = value;
                        else if (col == _template.GetIntegerValueFromColumnsEnum(ExcelDgTemplate.Columns.colMP))
                        {
                            if (value.ToLower() == "true"
                                || value.ToLower() == "y"
                                || value.ToLower() == "p")
                                unit.IsMp = true;
                            if (!string.IsNullOrEmpty(value)) unit.mpDetermined = true;
                        }
                        else if (col == _template.GetIntegerValueFromColumnsEnum(ExcelDgTemplate.Columns.colLQ))
                        {
                            if (value.ToLower() == "true" ||
                                value.ToLower() == "y" ||
                                value.ToLower() == "lq")
                                unit.IsLq = true;
                        }
                        else if (col == _template.GetIntegerValueFromColumnsEnum(ExcelDgTemplate.Columns.colFP))
                        {
                            unit.FlashPointAsDecimal = WithXlAssistToRead.DgFp(value);
                        }
                        else if (col == _template.GetIntegerValueFromColumnsEnum(ExcelDgTemplate.Columns.colEms))
                            unit.DgEMS = value;
                        //the following parses 'Remark' column and reads properties and segregation groups, if any
                        else if (col == _template.GetIntegerValueFromColumnsEnum(ExcelDgTemplate.Columns.colRemarks))
                            WithXlAssistToRead.ParseRemarkColumn(value, unit, cont);
                        else if (col == _template.GetIntegerValueFromColumnsEnum(ExcelDgTemplate.Columns.colNetWt))
                        {
                            decimal tempValue = 0.0M;
                            string tmp = value;
                            bool success = decimal.TryParse(tmp, out tempValue);
                            unit.DgNetWeight = (decimal)tempValue;
                        }
                        else if (col == _template.GetIntegerValueFromColumnsEnum(ExcelDgTemplate.Columns.colTechName))
                            unit.TechnicalName = value;
                        else if (col == _template.GetIntegerValueFromColumnsEnum(ExcelDgTemplate.Columns.colPackage))
                            unit.NumberAndTypeOfPackages = value;
                        else if (col == _template.GetIntegerValueFromColumnsEnum(ExcelDgTemplate.Columns.colFinalDestination))
                            unit.FinalDestination = value;
                        else if (col == _template.GetIntegerValueFromColumnsEnum(ExcelDgTemplate.Columns.colOperator))
                            unit.Carrier = value;
                    }

                    cont.HoldNr = Transport.ShipProfile.DefineCargoHoldNumber(cont.Bay);

                    // Handling missing container number
                    if (!string.IsNullOrEmpty(cont.ContainerNumber))
                        lastContainer = cont;
                    else
                        cont = lastContainer;

                    // Updating dg unit info and add it to the list
                    unit.CopyContainerAbstractInfo(cont);
                    dgList.Add(unit);

                    //Update containers
                    var container = containers.FirstOrDefault(c => string.Equals(c.ContainerNumber, cont.ContainerNumber, StringComparison.OrdinalIgnoreCase));
                    if (container == null)
                    {
                        cont.DgCountInContainer++;
                        containers.Add(cont);
                    }
                    else container.DgCountInContainer++;

                    //Status bar update
                    if (Data.ProgressBarReporter.ReportPercentage < 75)
                    {
                        if (statusBarIncrementValue == 0)
                        {
                            tempIncrementAccummulation += tempStatusBarIncrementValue;
                            if (tempIncrementAccummulation < 1) continue;
                            Data.ProgressBarReporter.ReportPercentage++;
                            tempIncrementAccummulation--;
                        }
                        else
                            Data.ProgressBarReporter.ReportPercentage += statusBarIncrementValue;
                    }
                }

                Data.LogWriter.Write($"Dg manifest data read from excel.");
                isImported = true;

            }
            catch (Exception ex)
            {
                Data.LogWriter.Write($"Attempt to read excel workbook {workbook} thrown an exception {ex.Message}.");
            }
            finally
            {
                #region Close and final release of excel objects

                activeWorkbook.Close(null, null, null);
                workbooks?.Close();
                excelapp.Quit();

                WithXl.RunGarbageCollector();
                if (excelcells != null)
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excelcells);
                if (excelWorksheet != null)
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excelWorksheet);
                if (activeWorkbook != null)
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(activeWorkbook);
                if (workbooks != null)
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(workbooks);
                if (excelapp != null)
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excelapp);

                #endregion
            }

            return isImported;
        }


        /// <summary>
        /// Method exports the list of Dg into Excel in format according to the template.
        /// </summary>
        /// <param name="dgList">DgList to be exported.</param>
        public static void Export(List<Dg> dgList)
        {
            if (_template == null)
            {
                Data.LogWriter.Write($"Excel template has not been read.");
                throw new System.ArgumentNullException("Excel template not read");
            }

            Data.ProgressBarReporter.ReportPercentage = 20;

            #region Creating workbook
            //Creating workbook
            ExcelApp.Application excelApp = new ExcelApp.Application { Visible = false, SheetsInNewWorkbook = 1 };
            excelApp.Workbooks.Add(Type.Missing);
            ExcelApp.Worksheet excelWorkSheet = excelApp.ActiveWorkbook.ActiveSheet;
            excelWorkSheet.Name = _template.WorkingSheet;
            ExcelApp.Range excelCells;
            #endregion

            Data.ProgressBarReporter.ReportPercentage = 25;
            bool isFirstColumnOccupied = false;

            #region Headings row

            var rowForHeaders = templateStartRow == 1 ? 1 : templateStartRow - 1;

            //Creating heading titles
            for (int i = 1; i <= templateMaxColumn; i++)
            {
                excelCells = (ExcelApp.Range)excelWorkSheet.Cells[rowForHeaders, i];
                excelCells.Font.Bold = true;
                excelCells.HorizontalAlignment = ExcelApp.Constants.xlCenter;
                excelCells.Borders.LineStyle = ExcelApp.XlLineStyle.xlContinuous;
                excelCells.WrapText = true;

                int x = _template.SearchValueIndex(i);
                if (x == -1)
                {
                    continue;
                }
                if (i == 1)
                    isFirstColumnOccupied = true;

                if (_template.ColumnWidths[x] != 0)
                    excelCells.ColumnWidth = _template.ColumnWidths[x];
                excelCells.Value2 = _template.ColumnHeaders[x];
            }

            Data.ProgressBarReporter.ReportPercentage = 30;

            #endregion

            #region Setting StatusBar increment values
            //setting to set status bar increment value for a single row
            float tempStatusBarIncrementValue = 65f / dgList.Count;
            float tempIncrementAccummulation = 0.0f;
            int statusBarIncrementValue = 0;
            if (tempStatusBarIncrementValue > 1)
                statusBarIncrementValue = 65 / dgList.Count;
            #endregion

            #region Filling table
            //Filling information into the table
            for (int excelrow = 1; excelrow <= dgList.Count; excelrow++)
            {
                Dg dg = dgList[excelrow - 1];
                for (int excelcol = 1; excelcol <= templateMaxColumn; excelcol++)
                {
                    string value = null;
                    excelCells = (ExcelApp.Range)excelWorkSheet.Cells[excelrow + rowForHeaders, excelcol];
                    excelCells.VerticalAlignment = ExcelApp.Constants.xlTop;

                    if (!isFirstColumnOccupied && excelcol == 1)
                        value = excelrow.ToString();
                    else
                        switch (_template.SearchValueIndex(excelcol))
                        {
                            case (int)ExcelDgTemplate.Columns.colLocation:
                                value = dg.Location;
                                excelCells.NumberFormat = "000000";
                                break;
                            case (int)ExcelDgTemplate.Columns.colContNr:
                                value = dg.DisplayContainerNumber;
                                excelCells.HorizontalAlignment = ExcelApp.Constants.xlLeft;
                                break;
                            case (int)ExcelDgTemplate.Columns.colPOL:
                                value = dg.POL;
                                excelCells.HorizontalAlignment = ExcelApp.Constants.xlRight;
                                break;
                            case (int)ExcelDgTemplate.Columns.colPOD:
                                value = dg.POD;
                                excelCells.HorizontalAlignment = ExcelApp.Constants.xlRight;
                                break;
                            case (int)ExcelDgTemplate.Columns.colUnno:
                                value = dg.Unno.ToString();
                                excelCells.NumberFormat = "0000";
                                break;
                            case (int)ExcelDgTemplate.Columns.colClass:
                                excelCells.NumberFormat = "@";
                                value = dg.DgClass.ToString(System.Globalization.CultureInfo.InvariantCulture);
                                excelCells.HorizontalAlignment = ExcelApp.Constants.xlRight;
                                break;
                            case (int)ExcelDgTemplate.Columns.colSubclass:
                                excelCells.NumberFormat = "@";
                                value = dg.DgSubClass;
                                excelCells.HorizontalAlignment = ExcelApp.Constants.xlRight;
                                break;
                            case (int)ExcelDgTemplate.Columns.colName:
                                value = dg.Name;
                                excelCells.WrapText = true;
                                break;
                            case (int)ExcelDgTemplate.Columns.colPkg:
                                value = dg.PackingGroup;
                                excelCells.HorizontalAlignment = ExcelApp.Constants.xlCenter;
                                break;
                            case (int)ExcelDgTemplate.Columns.colFP:
                                value = Math.Abs(dg.FlashPointAsDecimal - 9999) < 1 ? "" : dg.FlashPoint;
                                excelCells.NumberFormat = "0.0";
                                break;
                            case (int)ExcelDgTemplate.Columns.colMP:
                                value = dg.IsMp ? "P" : "";
                                break;
                            case (int)ExcelDgTemplate.Columns.colLQ:
                                value = dg.IsLq ? "LQ" : "";
                                break;
                            case (int)ExcelDgTemplate.Columns.colEms:
                                value = dg.DgEMS;
                                break;
                            case (int)ExcelDgTemplate.Columns.colRemarks:
                                if (dg.IsLiquid) value = "Liquid";
                                if (dg.IsFlammable) value += value == null ? "Flammable" : ", flammable";
                                if (dg.IsEmitFlammableVapours)
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
                            case (int)ExcelDgTemplate.Columns.colNetWt:
                                value = dg.DgNetWeight.ToString();
                                excelCells.NumberFormat = "0.000";
                                break;
                            case (int)ExcelDgTemplate.Columns.colTechName:
                                value = dg.TechnicalName;
                                excelCells.WrapText = true;
                                break;
                            case (int)ExcelDgTemplate.Columns.colPackage:
                                value = dg.NumberAndTypeOfPackages;
                                excelCells.WrapText = true;
                                break;
                            case (int)ExcelDgTemplate.Columns.colFinalDestination:
                                value = dg.FinalDestination;
                                excelCells.HorizontalAlignment = ExcelApp.Constants.xlRight;
                                break;
                            case (int)ExcelDgTemplate.Columns.colOperator:
                                value = dg.Carrier;
                                excelCells.HorizontalAlignment = ExcelApp.Constants.xlRight;
                                break;
                            case (int)ExcelDgTemplate.Columns.colEmergencyContact:
                                value = dg.EmergencyContacts;
                                break;
                            case -1:
                            default:
                                value = "";
                                break;
                        }

                    excelCells.Value2 = value;
                }

                #region Status bar update
                //Status bar update
                if (Data.ProgressBarReporter.ReportPercentage < 95)
                {
                    if (statusBarIncrementValue == 0)
                    {
                        tempIncrementAccummulation += tempStatusBarIncrementValue;
                        if (tempIncrementAccummulation < 1) continue;
                        Data.ProgressBarReporter.ReportPercentage++;
                        tempIncrementAccummulation--;
                    }
                    else
                        Data.ProgressBarReporter.ReportPercentage += statusBarIncrementValue;
                }
                #endregion

            }
            #endregion

            excelApp.Visible = true;
            Data.ProgressBarReporter.ReportPercentage = 100;
        }

    }
}
