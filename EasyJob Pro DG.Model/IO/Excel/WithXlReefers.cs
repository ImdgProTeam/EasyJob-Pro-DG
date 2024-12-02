using EasyJob_ProDG.Model.Cargo;
using System.Collections.Generic;
using ExcelApp = Microsoft.Office.Interop.Excel;

namespace EasyJob_ProDG.Model.IO.Excel
{
    public static class WithXlReefers
    {
        #region Template

        static ExcelReeferTemplate _template;

        static int templateStartRow => _template.StartRow;
        static byte templateMaxColumn => _template.GetMaxColumnNumber();


        /// <summary>
        /// Sets template value to be used.
        /// </summary>
        /// <param name="template"></param>
        internal static void SetTemplate(ExcelReeferTemplate template)
        {
            _template = template;
        }

        #endregion

        /// <summary>
        /// Imports reefers info from excel file
        /// </summary>
        /// <param name="excelReefers">Blank list to transfer info to</param>
        /// <param name="workbook">Filepath</param>
        /// <returns></returns>
        internal static bool ImportReeferInfoFromExcel(this List<Container> excelReefers, string workbook)
        {
            bool isImported = false;

            //connecting xl file
            Data.LogWriter.Write($"Connecting excel file {workbook}");
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
                excelWorksheet = WithXl.ChooseCorrectSheet(activeWorkbook, _template.WorkingSheet);
                Data.LogWriter.Write($"Reading reefer data...");
                Data.ProgressBarReporter.ReportPercentage = 40;

                //Determine number of rows = number of reefers
                int rowscount = WithXl.CountRows(excelWorksheet, _template.StartRow, int.Parse(_template[3]));
                if (rowscount < 1) throw new System.Exception();

                #region StatusBar increment value setup
                //Setting StatusBar increment value
                float tempStatusBarIncrementValue = 30f / rowscount;
                float tempIncrementAccummulation = 0.0f;
                int statusBarIncrementValue = 0;
                if (tempStatusBarIncrementValue > 1)
                    statusBarIncrementValue = 30 / rowscount;
                #endregion

                //Read the list
                for (int line = templateStartRow; line < rowscount + templateStartRow; line++)
                {
                    Container cont = new Container();

                    for (byte col = 1; col <= templateMaxColumn; col++)
                    {
                        excelcells = excelWorksheet.Cells[line, col];
                        if (excelcells.Value2 == null) continue;
                        string value = excelcells.Value2.ToString();

                        if (col == byte.Parse(_template[3])) cont.ContainerNumber = value;
                        else if (col == byte.Parse(_template[4])) cont.Commodity = value;
                        else if (col == byte.Parse(_template[5])) cont.SetTemperature = decimal.Parse(value);
                        else if (col == byte.Parse(_template[6])) cont.VentSetting = value;
                        else if (col == byte.Parse(_template[7])) cont.ReeferSpecial = value;
                        else if (col == byte.Parse(_template[8])) cont.ReeferRemark = value;
                    }
                    if (!excelReefers.Contains(cont)) excelReefers.Add(cont);

                    #region Status bar update
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
                    #endregion
                }
                Data.LogWriter.Write($"Reefer manifest data read from excel.");
                isImported = true;
            }
            catch
            {
                Data.LogWriter.Write($"Reefer manifest data reading failed!");
            }
            finally
            {
                activeWorkbook.Close(null, null, null);
                if (workbooks != null) workbooks.Close();
                excelapp.Quit();

                WithXl.RunGarbageCollector();
                WithXl.FinalReleaseCOMObjects(excelcells, excelWorksheet, activeWorkbook, workbooks, excelapp);
                Data.LogWriter.Write($"Excel file disconnected.");
            }

            return isImported;
        }
    }
}
