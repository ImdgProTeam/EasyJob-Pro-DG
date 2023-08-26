using EasyJob_ProDG.UI.Settings;

namespace EasyJob_ProDG.UI.Services
{
    interface ISettingsService
    {
        void LoadSettigs();
        UserUISettings GetSettings();
        void SaveExcelTemplate(object parameter);
        void SaveReeferExcelTemplate(string template);
    }
}
