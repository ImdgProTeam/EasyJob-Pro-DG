using EasyJob_ProDG.UI.Services;
using EasyJob_ProDG.UI.Utility;

namespace EasyJob_ProDG.UI.Settings
{
    /// <summary>
    /// Class calls all methods to apply all user settings from settings.settings
    /// </summary>
    internal static class UserSettingsApplicator
    {
        internal static void ApplyUserSettings()
        {
            //restore colflicts column width
            ((View.UI.MainWindow)System.Windows.Application.Current.MainWindow)?.RestoreConflictColumnWidth();

            //restore datagrids column settings
            ((View.UI.MainWindow)System.Windows.Application.Current.MainWindow)?.MainDgDataGrid.LoadColumnSettings();
            ((View.UI.MainWindow)System.Windows.Application.Current.MainWindow)?.MainContainerDataGrid.LoadColumnSettings();
            ((View.UI.MainWindow)System.Windows.Application.Current.MainWindow)?.MainReeferDataGrid.LoadColumnSettings();

            //restore excel templates
            SettingsService settingsService = new SettingsService();
            settingsService.LoadSettings();
            ViewModelLocator.SettingsWindowVM.SetExcelTemplateValues();
        }
    }
}
