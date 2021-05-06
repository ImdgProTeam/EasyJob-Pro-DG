using EasyJob_ProDG.Data;
using EasyJob_ProDG.Model.Transport;
using System;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace EasyJob_ProDG.Model
{
    public static class ProgramFiles
    {
        public static StreamWriter LogStreamWriter;
        public static StringBuilder TextToExport;
        internal static XDocument DgDataBase;

        // ----------------------- STATIC METHODS ---------------------------------------------------------------------------------

        // ------------------ Methods supporting the run of the program -----------------------------------------------------------

        /// <summary>
        /// Method connects ShipProfile and dgDataBase to program.
        /// </summary>
        /// <param name="ownship"></param>
        /// <param name="dgDataBase"></param>
        /// <returns></returns>            
        public static void Connect(out Transport.ShipProfile ownship, out XDocument dgDataBase)
        {
            ownship = Transport.ShipProfile.ReadShipProfile
                (ProgramDefaultSettingValues.DefaultShipProfile, ProgramDefaultSettingValues.AlwaysOpenDefaultProfile);
            EnterLog(LogStreamWriter, "Ship profile loaded");

            DgDataBase = GetXmlDoc("dglist.xml");
            dgDataBase = DgDataBase;
            EnterLog(LogStreamWriter, "Database connected");
        }

        /// <summary>
        /// Method loads the given .xml file located in application folder directory.
        /// </summary>
        /// <param name="docName"></param>
        /// <returns></returns>
        internal static XDocument GetXmlDoc(string docName)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load
                    (Path.Combine(ProgramDefaultSettingValues.ProgramDirectory.ToString()) + docName);

                //checking xml version
                string xmlVersion = (string)xmlDoc.Root.Attribute("version");
                if (xmlVersion != ProgramDefaultSettingValues.xmlDgListVersion)
                {
                    throw new Exception("Dg database has wrong format or corrupt.");
                }

                return xmlDoc;
            }
            catch (FileNotFoundException ex)
            {
                Output.ThrowMessage(ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                Output.ThrowMessage(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Method writes ship profile to selected file location
        /// </summary>
        /// <param name="ship"></param>
        /// <param name="ShipProfileName"></param>
        public static void SaveShipProfile(ShipProfile ship, string ShipProfileName = null)
        {
            string _fileName = ShipProfileName;
            if (_fileName == null || ProgramDefaultSettingValues.AlwaysOpenDefaultProfile)
                _fileName = ProgramDefaultSettingValues.DefaultShipProfile.Replace(ProgramDefaultSettingValues.ShipProfileExtension, "");
            ShipProfile.WriteShipProfile(_fileName, ship);
        }


        // ---------------- Static methods for creation and maintaining of the error log -------------------------------------------

        public static void EnterLog(StreamWriter stream, string message)
        {
            stream?.WriteLine("{0:O}\t{1}", DateTime.Now, message);
        }

        public static void CreateLog(StreamWriter stream, string arg = null)
        {
            stream?.WriteLine("Program activated @ {0:O}", DateTime.Now);
            stream?.WriteLine("activation method: {0}", arg ?? "executable");
            //stream?.WriteLine("Licence expire on: {0:R}", EasyJob_Pro_DG.UI.Licence.EndLicence);
            stream?.WriteLine();
        }

        private static void LogErrorMesage(Exception ex)
        {
            EnterLog(LogStreamWriter, "Exception caught");
            LogStreamWriter.WriteLine("exception data {0}", ex);
            LogStreamWriter.WriteLine("exception message {0}", ex.Message);
            LogStreamWriter.WriteLine("exception data {0}", ex.Data);
            LogStreamWriter.WriteLine("exception source {0}", ex.Source);
            LogStreamWriter.WriteLine("exception target {0}", ex.TargetSite);
            LogStreamWriter.WriteLine("exception inner {0}", ex.InnerException);
            string msg = "An error occurred when running Pro DG!" +
                         "\nPlease contact feedback@imdg.pro for further assistance" +
                         "\nExtremely sorry for inconvenience...";
            Output.ThrowMessage(msg);
        }
    }
}


