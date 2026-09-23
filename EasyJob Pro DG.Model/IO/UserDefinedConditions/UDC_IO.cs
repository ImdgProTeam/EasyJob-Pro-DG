using EasyJob_ProDG.Data;
using EasyJob_ProDG.Model.UserDefinedConflicts;
using EasyJob_ProDG.UI.Data.UserDefinedConflicts;
using System;
using System.IO;


namespace EasyJob_ProDG.Model.IO.UserDefinedConditions
{
    internal class UDC_IO
    {
        #region Defaults and constants

        private const string fileName = ProgramDefaultSettingValues.DefaultUserDefinedConditionsFile;
        private static readonly string filePath = OpenFile.GetFileFullPath(fileName);

        #endregion


        // ----- Public methods

        /// <summary>
        /// Creates an UDC file from the repository in default directory.
        /// </summary>
        /// <param name="repository"></param>
        /// <returns>True if created without errors.</returns>
        public static bool CreateUDCFile(UserDefinedConditionRepository repository)
        {
            //Saves UDC file
            if (SaveUDCFile(repository))
                return true;
            return false;
        }

        /// <summary>
        /// Reads UserDefiedConditions file.
        /// </summary>
        /// <returns>Repository of the <see cref="UserDefinedCondition"/>s</returns>
        public static UserDefinedConditionRepository ReadUDCFile()
        {
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
            {
                Data.LogWriter.Write($"UDC file {filePath} cannot be found.");
                return null;
            }
            Data.LogWriter.Write($"Reading {filePath}...");

            string[] _linesArray = ReadLinesFromFile();


            if (!UDC_Records.CheckUDCFileVersion(_linesArray))
            {
                return null;
            }

            var repository = UDC_Records.CreateRepositoryFromLines(_linesArray);

            Data.LogWriter.Write($"UDC file has been read.");
            return repository;
        }


        #region Private methods

        // ----- Read

        /// <summary>
        /// Reads UDC file.
        /// </summary>
        /// <returns>Array of lines from the file.</returns>
        private static string[] ReadLinesFromFile()
        {
            string[] linesArray;
            StreamReader reader = new StreamReader(filePath);
            {
                string text = reader.ReadToEnd();
                string[] lineSeparators = new[] { Environment.NewLine }; 
                linesArray = text.Split(lineSeparators, StringSplitOptions.RemoveEmptyEntries);

                reader.Close();
            }
            return linesArray;
        }


        // ----- Save

        /// <summary>
        /// Creates UDC file from the repository.
        /// </summary>
        /// <param name="repository"></param>
        private static bool SaveUDCFile(UserDefinedConditionRepository repository)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.Write(UDC_Records.CreateUDCFileFullText(repository));
                    writer.Close();
                }
            }
            catch (Exception ex)
            {
                Data.LogWriter.WriteError($"Saving UDC file {filePath} caused an exception {ex.Message}.");
                return false;
            }

            Data.LogWriter.Write($"Condition saved as {filePath}");
            return true;
        }

        #endregion
    }
}
