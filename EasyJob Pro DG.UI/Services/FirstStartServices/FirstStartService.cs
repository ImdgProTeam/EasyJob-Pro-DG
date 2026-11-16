using EasyJob_ProDG.Data;
using System;
using System.Diagnostics;
using System.Windows;

namespace EasyJob_ProDG.UI.Services
{
    internal class FirstStartService
    {
        /// <summary>
        /// Indicates if the first start actions have been attempted during the start of the program.
        /// Can be reffered from anywhere of the program.
        /// </summary>
        public static bool IsTheFirstStart { get; private set; } = false;

        public static void DoFirstStart()
        {
            IsTheFirstStart = true;

            //AssociateFileExtension
            try
            {
                string exePath = Process.GetCurrentProcess().MainModule.FileName;
                ExtensionRegistryHelper.Register(ProgramDefaultSettingValues.ConditionFileExtension, "EasyJob.ProDGPro.ejc", "ProDG Pro condition file", exePath);
                LogWriter.Write($"Program extension has been registered.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot register program extension.\nIt will not affect program performance.\nPlease acknowledge and continue with ");
                LogWriter.WriteError($"Could not register program extension: {ex.Message}.");
            }

            //Restore user settings
            try
            {
                RestoreUserSettings();
                LogWriter.Write("Properties have been upgraded from the previous version.");
            }
            catch (Exception ex)
            {
                LogWriter.WriteError($"Could not resotre Properties.Settings: {ex.Message}.");
            }

        }

        /// <summary>
        /// Restores user <see cref="Properties.Settings"/> from the previous version.
        /// </summary>
        private static void RestoreUserSettings()
        {
            Properties.Settings.Default.Upgrade();
            Properties.Settings.Default.Save();
        }
    }
}
