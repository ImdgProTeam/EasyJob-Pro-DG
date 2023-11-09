using EasyJob_ProDG.Data;
using System;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace EasyJob_ProDG.Model
{
    public static class ProgramFiles
    {
        public static StringBuilder TextToExport;
        internal static XDocument DgDataBase;

        private static bool hasDataBaseBeenSuccesfullyConnected = false;


        // ------------------ Methods supporting the run of the program -----------------------------------------------------------

        /// <summary>
        /// Method connects ShipProfile and dgDataBase to program.
        /// </summary>
        /// <param name="ownship">ShipProfile which can be used after it is connected from the file.</param>
        /// <param name="dgDataBase">Dg DataBase to be used after it is read from a file.</param>
        /// <returns>
        /// True, if DataBase has been succesfully connected.
        /// In case of problems with ShipProfile - default ship profile will be returned.
        /// In case of problems with dgDataBase - null will be returned.
        /// </returns>            
        public static bool Connect(out Transport.ShipProfile ownship, out XDocument dgDataBase)
        {
            ownship = Transport.ShipProfile.ReadShipProfile
                (ProgramDefaultSettingValues.DefaultShipProfile, ProgramDefaultSettingValues.AlwaysOpenDefaultProfile);
            LogWriter.Write("Ship profile loaded");

            DgDataBase = GetXmlDoc(ProgramDefaultSettingValues.DgDataBaseFile);
            dgDataBase = DgDataBase;

            return hasDataBaseBeenSuccesfullyConnected;
        }

        /// <summary>
        /// Method loads the given .xml file located in application folder directory.
        /// </summary>
        /// <param name="docName"></param>
        /// <returns></returns>
        private static XDocument GetXmlDoc(string docName)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load
                    (Path.Combine(ProgramDefaultSettingValues.ProgramDirectory.ToString()) + docName);

                //checking xml version
                LogWriter.Write("Checking dglist.xml version...");
                string xmlVersion = (string)xmlDoc.Root.Attribute("version");
                if (xmlVersion != ProgramDefaultSettingValues.xmlDgListVersion)
                {
                    LogWriter.Write("Wrong dglist.xml version is used");
                    throw new Exception("Dg database has wrong format or corrupt.");
                }

                hasDataBaseBeenSuccesfullyConnected = true;
                LogWriter.Write("Database connected");
                return xmlDoc;
            }
            catch (FileNotFoundException)
            {
                LogWriter.Write($"Database file {docName} not found.");
                return null;
            }
            catch (Exception ex)
            {
                LogWriter.Write($"Reading database file {docName} thrown exception: {ex.Message}.");
                return null;
            }
        }

        /// <summary>
        /// Method writes ship profile to selected file location
        /// </summary>
        /// <param name="ship"></param>
        /// <param name="ShipProfileName"></param>
        public static void SaveShipProfile(Transport.ShipProfile ship, string ShipProfileName = null)
        {
            string _fileName = ShipProfileName;
            if (_fileName == null || ProgramDefaultSettingValues.AlwaysOpenDefaultProfile)
                _fileName = ProgramDefaultSettingValues.DefaultShipProfile.Replace(ProgramDefaultSettingValues.ShipProfileExtension, "");
            Transport.ShipProfile.WriteShipProfile(_fileName, ship);
        }
    }
}


