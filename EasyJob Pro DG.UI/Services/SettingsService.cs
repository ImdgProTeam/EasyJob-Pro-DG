using EasyJob_ProDG.UI.Settings;
using EasyJob_ProDG.Model.IO.Excel;

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

        public void LoadSettigs()
        {
            _uiSettings.LoadSettings();
        }

        public void SaveExcelTemplate(object template)
        {

            ExcelTemplate temp = template as ExcelTemplate;
            if (temp != null) ExcelTemplate.WriteTemplate(temp);
        }
    }
}
