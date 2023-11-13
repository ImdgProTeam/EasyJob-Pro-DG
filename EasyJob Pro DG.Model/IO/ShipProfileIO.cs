using EasyJob_ProDG.Data;
using System.Collections.Generic;
using System.IO;

namespace EasyJob_ProDG.Model.IO
{
    public class ShipProfileIO
    {
        /// <summary>
        /// Reads ShipProfile.ini file and returns resulted List of strings as read.
        /// </summary>
        /// <param name="fileName">Profile name.</param>
        /// <param name="openDefault"></param>
        /// <returns></returns>
        public static List<string> ReadFromFile(string fileName, bool openDefault)
        {
            var fullPath = GetFullFilePath(fileName, openDefault);

            if (!File.Exists(fullPath))
            {
                ////Case file not found
                Output.ThrowMessage("Ship configuration file not found.\nA default ship profile configuration will be loaded. That will affect the accuracy of stowage and segregation check.");
                LogWriter.Write($"File {fileName} does not exist.");
                LogWriter.Write($"Default Ship profile will be loaded");
                return null;
            }

            List<string> resultArray = new();
            using (var reader = new StreamReader(fullPath))
            {
                while (!reader.EndOfStream)
                {
                    resultArray.Add(reader.ReadLine());
                }
            }
            LogWriter.Write($"File {fullPath} succesfully read.");

            return resultArray;
        }

        /// <summary>
        /// Returns full file path of the chosen ship profile.
        /// Only default is implemented.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="openDefault">Weather to open the default profile or not</param>
        /// <returns>Full path of the selected profile file.</returns>
        private static string GetFullFilePath(string fileName, bool openDefault)
        {
            //return default ship profile if option set
            if (openDefault) return ProgramDefaultSettingValues.ProgramDirectory + fileName;
            return null;
        }

        /// <summary>
        /// Creates or updates ShipProfile file.
        /// </summary>
        /// <param name="profileName">Profile name to be used in saved file name.</param>
        /// <param name="shipProfileText"></param>
        public static void WriteToFile(string profileName, string shipProfileText)
        {
            string fname = ProgramDefaultSettingValues.ProgramDirectory + profileName + ProgramDefaultSettingValues.ShipProfileExtension;

            using (StreamWriter writer = new StreamWriter(File.Create(fname)))
            {
                writer.Write(shipProfileText);
            }

            LogWriter.Write($"Ship profile successfully written to {fname}");
        }
    }
}
