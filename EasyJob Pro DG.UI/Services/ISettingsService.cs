using EasyJob_ProDG.UI.Wrapper;
using System.Collections.ObjectModel;

namespace EasyJob_ProDG.UI.Services
{
    interface ISettingsService
    {
        void LoadSettings();

        int SelectedExcelDgTemplateIndex { get; }
        int SelectedExcelReeferTemplateIndex { get; }

        ObservableCollection<ExcelDgTemplateWrapper> ExcelDgTemplates { get; set; }
        ObservableCollection<ExcelReeferTemplateWrapper> ExcelReeferTemplates { get; set; }

        void SetSelectedExcelDgTemplateIndex(int selectedExcelDgTemplateIndex);

        void SetSelectedExcelReeferTemplateIndex(int selectedExcelReeferTemplateIndex);

        void SaveSelectedExcelTemplateIndeces();

        void SaveExcelTemplate(string templateNameInSettings, string templateString);

    }
}
