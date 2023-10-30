using EasyJob_ProDG.UI.Settings;

namespace EasyJob_ProDG.UI.Services
{
    public class SettingsService : ISettingsService
    {
        static IUserUISettings _uiSettings = new UserUISettings();

        public SettingsService()
        {
            //this._uiSettings = _uiSettings;
        }

        public UserUISettings GetSettings()
        {
            return _uiSettings.GetSettings();
        }

        public void LoadSettings()
        {
            _uiSettings.LoadSettings();
        }

        /// <summary>
        /// Saves ExcelTemplate in settings.settings
        /// </summary>
        /// <param name="templateSettingsName">Template property name in settings.settings</param>
        /// <param name="template">Template in string format</param>
        public void SaveExcelTemplate(string templateSettingsName, string template)
        {
            Properties.Settings.Default[templateSettingsName] = template;
        }

        /// <summary>
        /// Calls dialog to choose settings file to save settings.
        /// </summary>
        public void SaveSettingsToFile()
        {
            SettingsHandler.SaveSettings();
        }

        /// <summary>
        /// Calls dialog to choose settings file and restore settings from it.
        /// </summary>
        public void RestoreSettingsFromFile(object obj)
        {
            SettingsHandler.RestoreSettings();
        }
    }
}
