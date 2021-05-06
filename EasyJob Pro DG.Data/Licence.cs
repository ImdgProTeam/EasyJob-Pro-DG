//Class to be fully converted
//Shall be done independent of any other project in the solution

using System;
using System.Threading.Tasks;
using System.Windows;
//using EasyJob_ProDG.Model;

namespace EasyJob_ProDG.Data
{
    static class Licence
    {
        private static readonly DateTime Endlicence = new DateTime(2018, 12, 31, 23, 59, 59);

        public static DateTime EndLicence => Endlicence;

        public static void LicenceCheck()
        {
            if (DateTime.Now > Endlicence)
            {
                //MessageBox.Show("Your licence has expired");
                //ProgramFiles.EnterLog(ProgramFiles.LogStreamWriter, "Licence checking failed");
                Environment.Exit(0);
            }
            //Style.QuestionStyle("The licence will expire on " + endlicence.ToString("dd/MMM/yyyy HH:mm"));
            //ProgramFiles.EnterLog(ProgramFiles.LogStreamWriter, "Licence checked");
            var t = Task.Delay(1000);
            t.Wait();
        }
    }
}
