using System;
using System.Windows;
using EasyJob_ProDG.Data;

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
                ExtensionRegistryHelper.SetFileAssociation(ProgramDefaultSettingValues.ConditionFileExtension,
                    ProgramDefaultSettingValues.ProgramTitle + "." + "Condition file");
            }
            catch (Exception e)
            {
                MessageBox.Show("Cannot register program extension");
            }

        }
    }
}
