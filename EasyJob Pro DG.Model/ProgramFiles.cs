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
            LogWriter.Write("Ship profile loaded");

            DgDataBase = GetXmlDoc(ProgramDefaultSettingValues.DgDataBaseFile);
            dgDataBase = DgDataBase;
            LogWriter.Write("Database connected");
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
                LogWriter.Write("Checking dglist.xml version...");
                string xmlVersion = (string)xmlDoc.Root.Attribute("version");
                if (xmlVersion != ProgramDefaultSettingValues.xmlDgListVersion)
                {
                    LogWriter.Write("Wrong dglist.xml version is used");
                    throw new Exception("Dg database has wrong format or corrupt.");
                }
                return xmlDoc;
            }
            catch (FileNotFoundException ex)
            {
                //TODO: Implement warning when no or old database connected
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
    }
}


