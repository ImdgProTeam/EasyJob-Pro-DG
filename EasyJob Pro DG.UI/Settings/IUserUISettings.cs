using EasyJob_ProDG.UI.Wrapper;
using System.Collections.ObjectModel;
using static EasyJob_ProDG.UI.Settings.UserUISettings;

namespace EasyJob_ProDG.UI.Settings
{
    public interface IUserUISettings
    {
        UserUISettings GetSettings();

        ObservableCollection<ExcelDgTemplateWrapper> ExcelDgTemplates { get; set; }
        ObservableCollection<ExcelReeferTemplateWrapper> ExcelReeferTemplates { get; set; }

        int SelectedDgTemplateIndex { get; set; }
        int SelectedReeferTemplateIndex { get; set; }
        void LoadSettings();
        DgSortOrderPattern DgSortPattern { get; set; }

        void SaveExcelTemplate(string templateSettingsName, string template);
        void SaveSelectedExcelTemplateIndeces();
    }
}
