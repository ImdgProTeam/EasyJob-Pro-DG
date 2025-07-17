using EasyJob_ProDG.UI.Settings;
using EasyJob_ProDG.UI.Wrapper;
using System.Collections.ObjectModel;

namespace EasyJob_ProDG.UI.Services
{
    /// <summary>
    /// Class is used to access <see cref="UserUISettings"/> values and methods.
    /// </summary>
    public class SettingsService : ISettingsService
    {
        static IUserUISettings _uiSettings = new UserUISettings();

        public void LoadSettings()
        {
            _uiSettings.LoadSettings();
        }
        public IUserUISettings GetSettings()
        {
            return _uiSettings.GetSettings();
        }


        /// <summary>
        /// Calls dialog to choose settings file to save settings.
        /// </summary>
        public void SaveSettingsToFile()
        {
            UserSettingsApplicator.SaveUserUISettings();
            UserSettingsFileHandler.SaveSettings();
        }

        /// <summary>
        /// Calls dialog to choose settings file and restore settings from it.
        /// </summary>
        public void RestoreSettingsFromFile(object obj)
        {
            UserSettingsFileHandler.RestoreSettings();
            UserSettingsApplicator.ApplyUserSettings();
        }

        #region Excel templates

        public int SelectedExcelDgTemplateIndex => _uiSettings.SelectedDgTemplateIndex;

        public int SelectedExcelReeferTemplateIndex => _uiSettings.SelectedReeferTemplateIndex;

        public ObservableCollection<ExcelDgTemplateWrapper> ExcelDgTemplates
        {
            get => _uiSettings.ExcelDgTemplates;
            set => _uiSettings.ExcelDgTemplates = value;
        }
        public ObservableCollection<ExcelReeferTemplateWrapper> ExcelReeferTemplates
        {
            get => _uiSettings.ExcelReeferTemplates;
            set => _uiSettings.ExcelReeferTemplates = value;
        }


        /// <summary>
        /// Saves templated string to settings store location
        /// </summary>
        /// <param name="templateNameInSettings"></param>
        /// <param name="templateString"></param>
        public void SaveExcelTemplate(string templateNameInSettings, string templateString)
        {
            _uiSettings.SaveExcelTemplate(templateNameInSettings, templateString);
        }

        public void SetSelectedExcelDgTemplateIndex(int selectedExcelDgTemplateIndex)
        {
            _uiSettings.SelectedDgTemplateIndex = selectedExcelDgTemplateIndex;
        }

        public void SetSelectedExcelReeferTemplateIndex(int selectedExcelReeferTemplateIndex)
        {
            _uiSettings.SelectedReeferTemplateIndex = selectedExcelReeferTemplateIndex;
        }

        /// <summary>
        /// Saves current template indeces in settings.
        /// </summary>
        public void SaveSelectedExcelTemplateIndeces()
        {
            _uiSettings.SaveSelectedExcelTemplateIndeces();
        } 

        #endregion
    }
}
