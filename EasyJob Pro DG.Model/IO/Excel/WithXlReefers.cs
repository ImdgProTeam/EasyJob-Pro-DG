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

                //Determine number of rows = number of reefers
                int rowscount = WithXl.CountRows(excelWorksheet, _template);

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
                        else if (col == byte.Parse(_template[5])) cont.SetTemperature = double.Parse(value);
                        else if (col == byte.Parse(_template[6])) cont.VentSetting = value;
                        else if (col == byte.Parse(_template[7])) cont.ReeferSpecial = value;
                        else if (col == byte.Parse(_template[8])) cont.ReeferRemark = value;
                    }
                    if (!excelReefers.Contains(cont)) excelReefers.Add(cont);
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
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excelcells);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excelWorksheet);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(activeWorkbook);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(workbooks);
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(excelapp);
                Data.LogWriter.Write($"Excel file disconnected.");
            }

            return isImported;
        }
    }
}
