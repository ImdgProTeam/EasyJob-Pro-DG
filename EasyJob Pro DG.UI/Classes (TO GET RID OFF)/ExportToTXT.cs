using System.IO;
using System.Text;
using EasyJob_ProDG.Data;

namespace EasyJob_ProDG.UI.Classes
{
    class ExportToTxt1
    {
        public static void Export(StringBuilder textToExport)
        {
            string resultFileName = ProgramDefaultSettingValues.ProgramDirectory + "result.txt";
            //FileInfo resultFile = new FileInfo(resultFileName);
            StreamWriter writer = new StreamWriter(resultFileName);
            writer.Write(textToExport.ToString());
            writer.Close();
            System.Diagnostics.Process.Start(resultFileName);
        }
    }
}
