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
            }
            catch (Exception)
            {
                MessageBox.Show("Cannot register program extension.\nIt will not affect program performance.\nPlease acknowledge and continue with ");
            }

            //Restore user settings
            try
            {
                RestoreUserSettings();
            }
            catch (Exception ex)
            {

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
