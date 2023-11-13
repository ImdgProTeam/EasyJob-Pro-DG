using EasyJob_ProDG.Data;
using EasyJob_ProDG.Model.IO;
using EasyJob_ProDG.Model.Transport;
using System;
using System.IO;
using System.Xml.Linq;

namespace EasyJob_ProDG.Model
{
    public static class ProgramFiles
    {
        public static XDocument DgDataBase { get; private set; }
        public static ShipProfile OwnShipProfile => ShipProfile.Instance;


        private static bool hasDataBaseBeenSuccesfullyConnected = false;


        // ------------------ Methods supporting the run of the program -----------------------------------------------------------

        /// <summary>
        /// Method connects ShipProfile and dgDataBase to program.
        /// </summary>
        /// <returns>
        /// True, if DataBase has been succesfully connected.
        /// In case of problems with ShipProfile - default ship profile will be returned.
        /// In case of problems with dgDataBase - null will be returned.
        /// </returns>            
        public static bool Connect()
        {
            ConnectShipProfile();
            DgDataBase = GetXmlDoc(ProgramDefaultSettingValues.DgDataBaseFile);

            return hasDataBaseBeenSuccesfullyConnected;
        }

        /// <summary>
        /// Creates <see cref="ShipProfile"/> from ShipProfile.ini file.
        /// </summary>
        /// <returns>Created ShipProfile or a default ShipProfile in case of any error.</returns>
        private static void ConnectShipProfile()
        {
            var profileLines = ShipProfileIO.ReadFromFile(ProgramDefaultSettingValues.DefaultShipProfile, ProgramDefaultSettingValues.AlwaysOpenDefaultProfile);
            ShipProfileHandler.SetCreateShipProfile(profileLines);
            LogWriter.Write("Ship profile loaded");
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
            var profileText = Transport.ShipProfileHandler.CreateShipProfileFileText(_fileName, ship);
            ShipProfileIO.WriteToFile(_fileName, profileText);
        }
    }
}


