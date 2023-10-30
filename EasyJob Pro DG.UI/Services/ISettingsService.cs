using EasyJob_ProDG.UI.Settings;

namespace EasyJob_ProDG.UI.Services
{
    interface ISettingsService
    {
        void LoadSettings();
        UserUISettings GetSettings();
        void SaveExcelTemplate(string templateNameInSettings, string templateString);

    }
}
