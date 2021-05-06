namespace EasyJob_ProDG.Model.IO.Excel
{
    partial class WithXl
    {
        /// <summary>
        /// Method interacts with user via Console to create and write excel template
        /// </summary>
        private static void WriteTemplate()
        {
            Output.ThrowMessage("Method not implemented");
            WithXl.template.WorkingSheet = "Dangerous goods";

            #region Writing new template
            //MessageBoxResult result = MessageBox.Show("Do you wish to save changes and update your template for further use? Y/N", String.Empty,MessageBoxButton.YesNo);
            //if (result == MessageBoxResult.Yes)
            //Output.ThrowMessage("Your changes will be saved and template updated");
            //    using (StreamWriter writer = new StreamWriter(File.Create(TemplateName)))
            //    {
            //        writer.WriteLine("***Template for excel import and export ***");
            //        writer.WriteLine("");
            //        writer.WriteLine("worksheet = " + _workingSheet);
            //        writer.WriteLine("startRow = " + _startRow);
            //        writer.WriteLine("container number = " + _colContNr);
            //        writer.WriteLine("container location = " + _colLocation);
            //        writer.WriteLine("unno = " + _colUnno);
            //        writer.WriteLine("pol = " + _colPOL);
            //        writer.WriteLine("pod = " + _colPOD);
            //        writer.WriteLine("class = " + _colClass);
            //        writer.WriteLine("subclass = " + _colSubclass);
            //        writer.WriteLine("psn = " + _colName);
            //        writer.WriteLine("pkg = " + _colPkg);
            //        writer.WriteLine("fp = " + _colFP);
            //        writer.WriteLine("mp = " + _colMP);
            //        writer.WriteLine("lq = " + _colLQ);
            //        writer.WriteLine("ems = " + _colEms);
            //        writer.WriteLine("remarks = " + _colRemarks);
            //        writer.WriteLine("");
            //        writer.WriteLine("Template version: 1.0");
            //    }
            #endregion
        }
    }
}
