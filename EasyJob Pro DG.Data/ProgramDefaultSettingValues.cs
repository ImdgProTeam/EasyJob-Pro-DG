using System;
using System.IO;
using System.Reflection;

namespace EasyJob_ProDG.Data
{
    public static class ProgramDefaultSettingValues
    {
        public const string ProgramTitle = "EasyJob ProDG Pro";
        public const string Copyright = "Copyright ©  2018 - 2025";
        public const string ShipProfileExtension = ".ini";
        public const string DefaultShipProfile = "ShipProfile.ini";
        public const string DgDataBaseFile = "dglist.xml";
        public static bool AlwaysOpenDefaultProfile = true;
        public static bool Multiprofile = false;
        public static DirectoryInfo ProgramDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
        public const string xmlDgListVersion = "2.24";
        public const string codeAmmendmentVersion = "42-24";
        public const string codeYear = "2024";
        public const string ConditionFileExtension = ".ejc";
        public const string NoNamePrefix = "+NONAME+";
        public const decimal DefaultFlashPointValue = -9999;

        public const byte lowestTierOnDeck = 72;


        /// <summary>
        /// Returns current program version
        /// </summary>
        public static string ReleaseVersion
        {
            get
            {
                Assembly thisAssem = typeof(ProgramDefaultSettingValues).Assembly;
                AssemblyName thisAssemName = thisAssem.GetName();
                Version ver = thisAssemName.Version ?? throw new ArgumentNullException("thisAssemName.Version");
                return ver.ToString();
            }
        }


    }
}
