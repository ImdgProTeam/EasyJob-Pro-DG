using System;
using System.Windows;
using EasyJob_ProDG.Data;

namespace EasyJob_ProDG.UI.Services.FirstStartServices
{
    internal class FirstStartService
    {
        public static void DoFirstStart()
        {
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

            //SetUp column settings

            //Setup ship profile

            //Setup default settings
        }
    }
}
