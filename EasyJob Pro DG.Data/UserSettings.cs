namespace EasyJob_ProDG.Data
{
    public static class UserSettings
    {
        // ----- Reading baplie -----
        public static bool ReadLQfromBaplie { get; set; } = true;
        public static bool ReadMPfromBaplie { get; set; } = true;

        /// <summary>
        /// Sets if nonamers need to show Container Number as '+NoName+' in accordance with defaults or just blank.
        /// </summary>
        public static bool DisplayNoNameForNonamers { get; set; } = false;

        /// <summary>
        /// Converts ContainerNumber to format to be displayed in DataGrid
        /// </summary>
        /// <param name="containerNumber"></param>
        /// <returns></returns>
        public static string ContainerNumberToDisplay(string containerNumber)
        {
            var noname = string.IsNullOrEmpty(containerNumber) || containerNumber.StartsWith(ProgramDefaultSettingValues.NoNamePrefix);
            var result = noname ? (DisplayNoNameForNonamers ? containerNumber : string.Empty)
                                : (containerNumber.Length>4 ? containerNumber.Insert(4, " ") : containerNumber);
            return result;
        }

        /// <summary>
        /// Converts dispayed value to ContainerNumber used by IContainer interface
        /// </summary>
        /// <param name="containerNumber">Dispayed container number in data grid.</param>
        /// <returns></returns>
        public static string ContainerNumberFromDisplay(string containerNumber)
        {
            return containerNumber.Replace(" ", "").Replace("|", "").Replace("\\", "");
        }
    }
}
