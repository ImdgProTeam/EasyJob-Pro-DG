using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using Excel = Microsoft.Office.Interop.Excel;

namespace EasyJob_Pro_DG
{

    static partial class WithXl
    {

        #region Class fields and their default values
        static readonly List<char> columns = new List<char>() {'0', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'};

        static readonly string templateName = ShipProfile.dir + "template.xlt";
        static string workingSheet = "Dangerous cargo";
        static int startRow = 2;
        static int colContNr = 2;
        static int colLocation = 3;
        static int colUnno = 4;
        static int colPOL = 5;
        static int colPOD = 6;
        static int colClass = 7;
        static int colSubclass = 8;
        static int colName = 9;
        static int colPkg = 10;
        static int colFP = 11;
        static int colMP = 12;
        static int colLQ = 13;
        static int colEMS = 14;
        static int colRemarks = 15;
#endregion


        //------------------- Import - Export methods -------------------------------------------------------------------------

        /// <summary>
        /// Method will export the list of Dg into Excel in default format
        /// </summary>
        /// <param name="dglist"></param>
        public static void Export(List<Dg> dglist)
        {
            #region Reading template
            //reading xl template
            if (!File.Exists(templateName))
            {
                MessageBoxResult result = MessageBox.Show("Template file not found. Do you wish to create it? Y/N",String.Empty,MessageBoxButton.YesNo);
                if(result == MessageBoxResult.Yes)
                    WriteTemplate();
            }
            else
            using (var reader = new StreamReader(templateName))
            {
                while (!reader.EndOfStream)
                {
                    try
                    {
                        var line = reader.ReadLine();
                        if (string.IsNullOrEmpty(line) || !line.Contains("=")) continue;
                        var descriptor = line.Remove(line.IndexOf("=", StringComparison.Ordinal)).Trim();
                        var value = line.Substring(line.IndexOf("=", StringComparison.Ordinal) + 1).Trim();
                        switch (descriptor)
                        {
                            case "worksheet":
                                value = value==""? "Dangerous goods" : value.Replace("\"", "");
                                workingSheet = value;
                                break;
                            case "startRow":
                                startRow = int.Parse(value);
                                break;
                            case "container number":
                                colContNr = int.Parse(value);
                                break;
                            case "container location":
                                colLocation = int.Parse(value);
                                break;
                            case "unno":
                                colUnno = int.Parse(value);
                                break;
                            case "pol":
                                colPOL = int.Parse(value);
                                break;
                            case "pod":
                                colPOD = int.Parse(value);
                                break;
                            case "class":
                                colClass = int.Parse(value);
                                break;
                            case "subclass":
                                colSubclass = int.Parse(value);
                                break;
                            case "psn":
                                colName = int.Parse(value);
                                break;
                            case "pkg":
                                colPkg = int.Parse(value);
                                break;
                            case "fp":
                                colFP = int.Parse(value);
                                break;
                            case "mp":
                                colMP = int.Parse(value);
                                break;
                            case "lq":
                                colLQ = int.Parse(value);
                                break;
                            case "ems":
                                colEMS = int.Parse(value);
                                break;
                            case "remarks":
                                colRemarks = int.Parse(value);
                                break;
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Template file is damaged or modified. Please create a new template");
                    }
                }
            }
            #endregion

            //Change template or use default
            if (File.Exists(templateName))
            {
                MessageBox.Show(
                    "Template will be used to create dg list in excel.");
            }

            //Creating workbook
            Excel.Application excelapp = new Excel.Application {Visible = false, SheetsInNewWorkbook = 1};
            excelapp.Workbooks.Add(Type.Missing);
            Excel.Worksheet excelworksheet = excelapp.ActiveWorkbook.ActiveSheet;
            excelworksheet.Name = workingSheet;
            Excel.Range excelcells;

            //List of headings
            List<string> titles = new List<string>()
            {
                "","NN","Position","Container number","POL","POD","UNNO","Class","Subclass","Proper shipping name","PKG","FP","MP","LQ","EMS","Remarks"
            };

            //List of column widths
            List<int> columnWidth = new List<int>()
            {
                0,6,0,18,0,0,0,0,0,45,5,5,5,5,0,30

            };

            #region Headings row
            //Creating heading titles
            for (int i = 1; i <= titles.Count; i++)
            {
                excelcells = (Excel.Range) excelworksheet.Cells[startRow-1, i];
                excelcells.Font.Bold = true;
                excelcells.HorizontalAlignment = Excel.Constants.xlCenter;
                //excelcells.BorderAround();
                excelcells.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
                int x = GetColumnNr(i);
                if (columnWidth[x] != 0) excelcells.ColumnWidth = columnWidth[x];
                excelcells.Value2 = titles[x];
            }
            #endregion

            #region Filling table
            //Filling information into the table
            for (int excelrow = 1; excelrow <= dglist.Count; excelrow++)
            {
                Dg dg = dglist[excelrow - 1];
                for (int excelcol = 1; excelcol <= titles.Count; excelcol++)
                {
                    string value = null;
                    excelcells = (Excel.Range) excelworksheet.Cells[excelrow + startRow - 1, excelcol];
                    excelcells.VerticalAlignment = Excel.Constants.xlTop;
                    switch (GetColumnNr(excelcol))
                    {
                        case 1:
                            value = excelrow.ToString();
                            //excelcells.ColumnWidth = 6;
                            break;
                        case 2:
                            value = dg.cntrLocation;
                            excelcells.NumberFormat = "000000";
                            break;
                        case 3:
                            value = dg.cntrNr;
                            //excelcells.ColumnWidth = 18;
                            excelcells.HorizontalAlignment = Excel.Constants.xlLeft;
                            break;
                        case 4:
                            value = dg.cnPOL;
                            excelcells.HorizontalAlignment = Excel.Constants.xlRight;
                            break;
                        case 5:
                            value = dg.cnPOD;
                            excelcells.HorizontalAlignment = Excel.Constants.xlRight;
                            break;
                        case 6:
                            value = dg.unno.ToString();
                            excelcells.NumberFormat = "0000";
                            break;
                        case 7:
                            excelcells.NumberFormat = "@";
                            value = dg.dgclass.ToString(CultureInfo.InvariantCulture);
                            excelcells.HorizontalAlignment = Excel.Constants.xlRight;
                            break;
                        case 8:
                            excelcells.NumberFormat = "@";
                            value = dg.Dgsubclass;
                            excelcells.HorizontalAlignment = Excel.Constants.xlRight;
                            break;
                        case 9:
                            value = dg.name;
                            break;
                        case 10:
                            value = dg.PKG;
                            break;
                        case 11:
                            value = Math.Abs(dg.dgfp - 9999) < 1 ? "" : dg.dgfp.ToString(CultureInfo.InvariantCulture);
                            excelcells.NumberFormat = "0.0";
                            break;
                        case 12:
                            value = dg.mp ? "P" : "";
                            break;
                        case 13:
                            value = dg.LQ ? "LQ" : "";
                            break;
                        case 14:
                            value = dg.dgems;
                            break;
                        case 15:
                            if (dg.liquid) value = "Liquid";
                            if (dg.flammable) value += value == null ? "Flammable" : ", flammable";
                            if (dg.emitFlamVapours)
                                value += value == null ? "Emitting flammable vapours" : ", emitting flammable vapours";
                            if (dg.segregationGroup.Count == 0) continue;
                            foreach (var x in dg.segregationGroup)
                            {
                                if (value != null) value += ", ";
                                value += Segregation.segregationGroups[x];
                            }

                            if (!dg.closed) value += value == null ? "Open" : ", open";
                            break;
                        default:
                            value = "";
                            break;
                    }

                    excelcells.Value2 = value;
                }
            }
            #endregion

            excelcells = excelworksheet.Range["O:O"];
            excelcells.WrapText = true;
            excelcells = excelworksheet.Range["I:I"];
            excelcells.WrapText = true;
            excelapp.Visible = true;
        }

        /// <summary>
        /// Method imports Dg list from Excel sheet with all the fields according to Template.
        /// Returns Dg List.
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="ship"></param>
        /// <param name="containers"></param>
        /// <returns></returns>
        public static List<Dg> Import(string workbook, ShipProfile ship, out ObservableCollection<Container> containers)
        {
            List<Dg> result = new List<Dg>();
            containers = new ObservableCollection<Container>();


            #region Reading template

            //reading xl template
            if (!File.Exists(templateName))
            {
                MessageBoxResult res = MessageBox.Show("Template file not found. Do you wish to create it? Y/N", String.Empty, MessageBoxButton.YesNo);
                if (res == MessageBoxResult.Yes)
                    WriteTemplate();
            }
            else
                using (var reader = new StreamReader(templateName))
                {
                    while (!reader.EndOfStream)
                    {
                        try
                        {
                            var line = reader.ReadLine();
                            if (string.IsNullOrEmpty(line) || !line.Contains("=")) continue;
                            var descriptor = line.Remove(line.IndexOf("=", StringComparison.Ordinal)).Trim();
                            var value = line.Substring(line.IndexOf("=", StringComparison.Ordinal) + 1).Trim();
                            switch (descriptor)
                            {
                                case "worksheet":
                                    value = value.Replace("\"", "");
                                    workingSheet = value;
                                    break;
                                case "startRow":
                                    startRow = int.Parse(value);
                                    break;
                                case "container number":
                                    colContNr = int.Parse(value);
                                    break;
                                case "container location":
                                    colLocation = int.Parse(value);
                                    break;
                                case "unno":
                                    colUnno = int.Parse(value);
                                    break;
                                case "pol":
                                    colPOL = int.Parse(value);
                                    break;
                                case "pod":
                                    colPOD = int.Parse(value);
                                    break;
                                case "class":
                                    colClass = int.Parse(value);
                                    break;
                                case "subclass":
                                    colSubclass = int.Parse(value);
                                    break;
                                case "psn":
                                    colName = int.Parse(value);
                                    break;
                                case "pkg":
                                    colPkg = int.Parse(value);
                                    break;
                                case "fp":
                                    colFP = int.Parse(value);
                                    break;
                                case "mp":
                                    colMP = int.Parse(value);
                                    break;
                                case "lq":
                                    colLQ = int.Parse(value);
                                    break;
                                case "ems":
                                    colEMS = int.Parse(value);
                                    break;
                                case "remarks":
                                    colRemarks = int.Parse(value);
                                    break;
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Template file is damaged or modified. Please create a new template");
                        }
                    }
                }

            #endregion

            if (File.Exists(templateName))
            {
                MessageBox.Show(
                    "Template will be used to read excel file. Press any key to continue. \nTo change the templete press '1'");
            }

            //connecting xl file
            Excel.Range excelcells;
            Excel.Application excelapp = new Excel.Application {Visible = false};
            excelapp.Workbooks.Open(workbook, Excel.XlUpdateLinks.xlUpdateLinksNever);
            Excel.Worksheet excelWorksheet = ChooseCorrectSheet(excelapp.ActiveWorkbook, workingSheet);

            //Determine number of rows = number of dg
            bool stop = false;
            int rowscount = 0;
            while (!stop)
            {
                excelcells = excelWorksheet.Cells[startRow + rowscount, colContNr];
                stop = excelcells.Value2 == null;
                if (!stop) rowscount++;
            }

            //Create dg list & container list
            for (int line = 0; line < rowscount; line++)
            {
                int row = startRow + line;
                Dg unit = new Dg();
                Container cont = new Container();
                for (int col = 1; col <= 15; col++)
                {
                    excelcells = excelWorksheet.Cells[row, col];
                    if (excelcells.Value2 == null) continue;
                    if (col == colContNr) cont.cntrNr = excelcells.Value2;
                    else if(col == colLocation) cont.Location = Convert.ToString(excelcells.Value2);
                    else if (col == colPOL) cont.cnPOL = excelcells.Value2;
                    else if(col == colPOD) cont.cnPOD = excelcells.Value2;
                    else if (col == colUnno)
                    {
                        unit.unno = Convert.ToInt16(excelcells.Value2);
                        unit.AssignSegregationGroup();
                    }
                    else if (col == colClass)
                    {
                        unit.Dgclass = WithXLAssistToRead.DGclass(excelcells, unit, excelapp);
                        if(!string.IsNullOrEmpty(unit.Dgclass)) unit.AssignRowNumber();
                    }
                    else if (col == colSubclass)
                    {
                        unit.DgsubclassArray = WithXLAssistToRead.DGsubclass(excelcells, unit, excelapp);
                    }
                    else if (col == colName) unit.name = excelcells.Value2;
                    else if (col == colPkg) unit.PKG = excelcells.Value2.ToString();
                    else if (col == colMP)
                    {
                        if (excelcells.Value2 == "true" || excelcells.Value2 == "Y" || excelcells.Value2 == "P")
                            unit.mp = true;
                        if (excelcells.Value2 != null) unit.mpDetermined = true;
                    }
                    else if (col == colLQ)
                    {
                        if (excelcells.Value2.ToString().ToLower() == "true" ||
                            excelcells.Value2.ToString().ToLower() == "y" ||
                            excelcells.Value2.ToString().ToLower() == "lq")
                            unit.LQ = true;
                    }
                    else if(col == colFP) unit.dgfp = (float) Convert.ToDouble(excelcells.Value2);
                    else if (col == colEMS) unit.dgems = excelcells.Value2;
                    //the following parses 'Remark' column and reads properties and segregation groups, if any
                    else if (col == colRemarks) ParseRemarkColumn(excelcells.Value2, unit, cont);
                    }

                cont.holdNr = ship.DefineCargoHoldNumber(cont.bay);
                unit.CopyContainerInfo(cont);
                result.Add(unit);
                if (!containers.Contains(cont)) containers.Add(cont);
            }

            excelapp.Windows[1].Close(false);
            excelapp.Quit();
            return result;
        }


        //------------------- Assisting methods ------------------------------------------------------------------------------

        /// <summary>
        /// Method deals with multiple sheets in excel workbook
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="templatename"></param>
        /// <returns></returns>
        private static Excel.Worksheet ChooseCorrectSheet(Excel.Workbook workbook, string templatename)
        {
            ///Choosing worksheet
            Excel.Worksheet excelWorksheet = null;
            ///First check if there is only one sheet
            if (workbook.Sheets.Count == 1) excelWorksheet = workbook.Sheets[1];
            ///Then try to use given name in template
            else
            {
                try
                {
                    excelWorksheet = workbook.Sheets[templatename];
                }
                ///...if not matching create a list of sheets and offer a choise
                catch (Exception)
                {

                    MessageBox.Show("Not implemented");
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
        /// Parsing string value in Remark column and updates the properties of dg unit and the 'closed' property of container.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="unit"></param>
        /// <param name="cont"></param>
        private static void ParseRemarkColumn(string value, Dg unit, Container cont)
        {
            bool matched = false;
            string tempValue = Convert.ToString(value).ToLower().Replace("\t", ",").Replace("\n", ",")
                .Replace(", ", ",").Replace("\\", ",").Replace("/", ",").Trim();
            string[] temparray = tempValue.Split(',');
            foreach (string x in temparray)
            {
                switch (x)
                {
                    case "flammable":
                        unit.flammable = true;
                        matched = true;
                        break;
                    case "liquid":
                        unit.liquid = true;
                        matched = true;
                        break;
                    case "lq":
                        unit.LQ = true;
                        matched = true;
                        break;
                    case "limited quantity":
                        unit.LQ = true;
                        matched = true;
                        break;
                    case "ltd qty":
                        unit.LQ = true;
                        matched = true;
                        break;
                    case "emitting flammable vapours":
                        unit.emitFlamVapours = true;
                        matched = true;
                        break;
                    case "rf":
                        cont.RF = true;
                        break;
                    default:
                        break;
                }

                if (x.Contains("open"))
                {
                    cont.closed = false;
                    matched = true;
                }

                if (Segregation.segregationGroups.Contains(x))
                {
                    unit.SegregationGroup = x;
                    matched = true;
                }

                if (Segregation.segregationGroups.Contains(x + 's'))
                {
                    unit.SegregationGroup = x + 's';
                    matched = true;
                }

                if (Segregation.segregationGroups.Contains(x + "es"))
                {
                    unit.SegregationGroup = x + "es";
                    matched = true;
                }
            }

            if (matched) return;

            //Presumed separation with spaces
            temparray = tempValue.Split(' ');
            int i = 0;
            foreach (string x in temparray)
            {
                switch (x)
                {
                    case "flammable":
                        if (temparray[i].Contains("emit")) continue;
                        unit.flammable = true;
                        continue;
                    case "liquid":
                        unit.liquid = true;
                        continue;
                    case "lq":
                        unit.LQ = true;
                        continue;
                    case "limited":
                        unit.LQ = true;
                        continue;
                    case "ltd":
                        unit.LQ = true;
                        continue;
                    case "emitting":
                        unit.emitFlamVapours = true;
                        continue;
                    case "emit":
                        unit.emitFlamVapours = true;
                        continue;
                    default:
                        break;
                }

                if (x.Contains("open"))
                {
                    cont.closed = false;
                    continue;
                }
                if (Segregation.segregationGroups.Contains(x))
                {
                    unit.SegregationGroup = x;
                    continue;
                }

                if (Segregation.segregationGroups.Contains(x + 's'))
                {
                    unit.SegregationGroup = x + 's';
                    continue;
                }

                if (Segregation.segregationGroups.Contains(x + "es"))
                {
                    unit.SegregationGroup = x + "es";
                    continue;
                }
                switch (x)
                {
                    case "ammonium":
                        unit.SegregationGroup = "ammonium compounds";
                        continue;
                    case "heavy":
                        unit.SegregationGroup = "heavy metals and their salts";
                        continue;
                    case "lead":
                        unit.SegregationGroup = "lead and its compounds";
                        continue;
                    case "halogenated":
                        unit.SegregationGroup = "liquid halogenated hydrocarbons";
                        continue;
                    case "mercury":
                        unit.SegregationGroup = "mercury and mercury compounds";
                        continue;
                    case "nitrites":
                        unit.SegregationGroup = "nitrites and their mixtures";
                        continue;
                    case "powdered":
                        unit.SegregationGroup = "powdered metals";
                        continue;
                    case "strong":
                        unit.SegregationGroup = "strong acids";
                        continue;
                    default:
                        break;
                }
                i++;
            }
        }

        /// <summary>
        /// Assisting class to establish which information is assigned to the column i.
        /// Literally translates default order into order according to the template
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private static int GetColumnNr(int i)
        {
            if (i == colLocation) return 2;
            else if (i == colContNr) return 3;
            else if (i == colPOL) return 4;
            else if (i == colPOD) return 5;
            else if (i == colUnno) return 6;
            else if (i == colClass) return 7;
            else if (i == colSubclass) return 8;
            else if (i == colName) return 9;
            else if (i == colPkg) return 10;
            else if (i == colFP) return 11;
            else if (i == colMP) return 12;
            else if (i == colLQ) return 13;
            else if (i == colEMS) return 14;
            else if (i == colRemarks) return 15;
            else return 0;

        }

    }
}
