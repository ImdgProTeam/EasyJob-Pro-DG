using EasyJob_ProDG.Model.Cargo;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ExcelApp = Microsoft.Office.Interop.Excel;

namespace EasyJob_ProDG.Model.IO.Excel
{
    public static class WithXlReefers
    {

        static List<byte> template = new List<byte>() { 1, 2, 3, 4, 5, 6 };
        static int templateStartRow = 1;
        static bool isTemplateRead = false;

        /// <summary>
        /// Imports manifest info from excel file
        /// </summary>
        /// <param name="reefers">Reefers list to update info in</param>
        /// <param name="file">Excel file path from which to import manifest info</param>
        /// <returns></returns>
        public static bool ImportReeferManifestInfoFromExcel(this IEnumerable<Container> reefers, string file)
        {
            List<Container> tempList = new List<Container>();

            if (!tempList.ImportReeferInfoFromExcel(file)) return false;

            reefers.UpdateReeferManifestInfo(tempList);

            return true;
        }

        /// <summary>
        /// Updates List of Reefers with reefer info from manifestInfoReefers
        /// </summary>
        /// <param name="reefers">Reefers to be updated</param>
        /// <param name="manifestInfoReefers">Reefers to update info from</param>
        private static void UpdateReeferManifestInfo(this IEnumerable<Container> reefers, IEnumerable<Container> manifestInfoReefers)
        {
            Container reefer;

            foreach (var unit in manifestInfoReefers)
            {
                reefer = reefers.FirstOrDefault(x => x.ContainerNumber == unit.ContainerNumber);
                if (reefer != null)
                {
                    reefer.Commodity = unit.Commodity;
                    if(unit.SetTemperature != 0) reefer.SetTemperature = unit.SetTemperature;
                    reefer.VentSetting = unit.VentSetting;
                    reefer.ReeferSpecial = unit.ReeferSpecial;
                    reefer.ReeferRemark = unit.ReeferRemark;
                }
            }
        }

        /// <summary>
        /// Imports reefers info from excel file
        /// </summary>
        /// <param name="excelReefers">Blank list to transfer info to</param>
        /// <param name="workbook">Filepath</param>
        /// <returns></returns>
        private static bool ImportReeferInfoFromExcel(this List<Container> excelReefers, string workbook)
        {
            //bool isTemplateRead = false;// template.ReadTemplate();
            bool isImported = false;

            if (!isTemplateRead)
            {
                Output.ThrowMessage(
                    "ProDG was unable to read excel file. Default template will be used");
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
                excelWorksheet = activeWorkbook.Sheets[1];
                Debug.WriteLine("---> Reading reefer data...");

                //Determine number of rows = number of reefers
                int rowscount = WithXl.CountRows(ref excelcells, excelWorksheet, templateStartRow);


                //Read the list
                for (int line = templateStartRow; line < rowscount + templateStartRow; line++)
                {
                    Container cont = new Container();
                    for (byte col = 1; col <= template.Max(); col++)
                    {
                        excelcells = excelWorksheet.Cells[line, col];
                        if (excelcells.Value2 == null) continue;
                        string value = excelcells.Value2.ToString();

                        if (col == template[0]) cont.ContainerNumber = value;
                        else if (col == template[1]) cont.Commodity = value;
                        else if (col == template[2]) cont.SetTemperature = double.Parse(value);
                        else if (col == template[3]) cont.VentSetting = value;
                        else if (col == template[4]) cont.ReeferSpecial = value;
                        else if(col == template[5]) cont.ReeferRemark = value;
                    }
                    if (!excelReefers.Contains(cont)) excelReefers.Add(cont);
                }
                isImported = true;
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
            }

            return isImported;
        }

    }
}
