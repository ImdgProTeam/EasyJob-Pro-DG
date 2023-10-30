using System;
using System.Collections.Generic;
using ExcelApp = Microsoft.Office.Interop.Excel;

namespace EasyJob_ProDG.Model.IO.Excel
{

    public static partial class WithXl
    {
        /// <summary>
        /// Excel columns enumeration
        /// </summary>
        public static readonly List<char> Columns = new List<char>() { '0', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };


        //------------------- Assisting methods ------------------------------------------------------------------------------

        /// <summary>
        /// Counts number of rows containing Dg data in excel sheet.
        /// </summary>
        /// <param name="excelWorksheet"></param>
        /// <returns></returns>
        internal static int CountRows(ExcelApp.Worksheet excelWorksheet, ExcelTemplate template)
        {
            bool stop = false;
            int rowsCount = 0;
            ExcelApp.Range excelCells = null;

            //choose reference column: for Dg - unno, for others - ContainerNumber
            int checkColumn = int.Parse(template is ExcelDgTemplate ? template[5] : template[3]);

            while (!stop)
            {
                excelCells = excelWorksheet.Cells[template.StartRow + rowsCount, checkColumn];
                stop = excelCells.Value2 == null;
                if (!stop) rowsCount++;
            }

            excelCells = null;
            return rowsCount;
        }

        /// <summary>
        /// Method deals with multiple sheets in excel workbook
        /// </summary>
        /// <param name="workbook"></param>
        /// <param name="templateName"></param>
        /// <returns></returns>
        internal static ExcelApp.Worksheet ChooseCorrectSheet(ExcelApp.Workbook workbook, string templateName)
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
        /// Runs implicitly garbage collector.
        /// </summary>
        internal static void RunGarbageCollector()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
