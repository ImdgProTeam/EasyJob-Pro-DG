using System;
using System.IO;
using System.Windows;

namespace EasyJob_Pro_DG
{
    static partial class WithXl
    {
        /// <summary>
        /// Method interacts with user via Console to create and write excel template
        /// </summary>
        private static void WriteTemplate()
        {
            MessageBox.Show("Method not implemented");
            workingSheet = "Dangerous goods";

            

            #region Writing new template
            MessageBoxResult result = MessageBox.Show("Do you wish to save changes and update your template for further use? Y/N", String.Empty,MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
                using (StreamWriter writer = new StreamWriter(File.Create(templateName)))
                {
                    writer.WriteLine("***Template for excel import and export ***");
                    writer.WriteLine("");
                    writer.WriteLine("worksheet = " + workingSheet);
                    writer.WriteLine("startRow = " + startRow);
                    writer.WriteLine("container number = " + colContNr);
                    writer.WriteLine("container location = " + colLocation);
                    writer.WriteLine("unno = " + colUnno);
                    writer.WriteLine("pol = " + colPOL);
                    writer.WriteLine("pod = " + colPOD);
                    writer.WriteLine("class = " + colClass);
                    writer.WriteLine("subclass = " + colSubclass);
                    writer.WriteLine("psn = " + colName);
                    writer.WriteLine("pkg = " + colPkg);
                    writer.WriteLine("fp = " + colFP);
                    writer.WriteLine("mp = " + colMP);
                    writer.WriteLine("lq = " + colLQ);
                    writer.WriteLine("ems = " + colEMS);
                    writer.WriteLine("remarks = " + colRemarks);
                    writer.WriteLine("");
                    writer.WriteLine("Template version: 1.0");
                }
            #endregion

        }

        
    }
}
