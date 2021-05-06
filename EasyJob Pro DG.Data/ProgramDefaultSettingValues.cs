using System;
using System.IO;
using System.Reflection;

namespace EasyJob_ProDG.Data
{
    public static class ProgramDefaultSettingValues
    {
        public const string ProgramTitle = "EasyJob ProDG";
        public const string ShipProfileExtension = ".ini";
        public const string DefaultShipProfile = "ShipProfile.ini";
        public static bool AlwaysOpenDefaultProfile = true;
        public static bool Multiprofile = false;
        public static DirectoryInfo ProgramDirectory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
        public const string xmlDgListVersion = "2.0";
        public const string codeAmmendmentVersion = "39-18";
        public const string codeYear = "2018";
        public const string ConditionFileExtension = ".ejc";

        public const byte lowestTier = 72;

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
