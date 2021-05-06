using EasyJob_ProDG.UI.Services;
using EasyJob_ProDG.UI.Settings;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.ViewModel
{
    public class SettingsWindowVM : Observable, IDataErrorInfo
    {
        ISettingsService uiSettingsService = new SettingsService();
        UserUISettings settings;

        bool changedExcelTemplate;

        public static List<char> Columns { get { return Model.IO.Excel.WithXl.Columns; } }

        private ExcelTemplateWrapper _excelTemplate;
        public ExcelTemplateWrapper ExcelTemplateDisplay
        {
            get
            {
                if (_excelTemplate == null)
                {
                    _excelTemplate = new ExcelTemplateWrapper(settings.ExcelTemplate);
                }
                return _excelTemplate;
            }
        }


        public SettingsWindowVM()
        {
            settings = uiSettingsService.GetSettings();
            LoadCommands();
        }

        private void LoadCommands()
        {
            SaveChangesCommand = new DelegateCommand(SaveChanges);
            CancelChangesCommand = new DelegateCommand(CancelChanges);
        }

        public void CancelChanges(object obj)
        {
            changedExcelTemplate = false;
            ExcelTemplateDisplay.CancelChanges();
        }

        private void SaveChanges(object obj)
        {
            CheckWhatChanged();

            if (changedExcelTemplate)
            {
                ExcelTemplateDisplay.UploadChangesFromColumnProperties();
                uiSettingsService.SaveExcelTemplate(settings.ExcelTemplate);
            }

            ResetChangeIndicators();
        }

        private void CheckWhatChanged()
        {
            changedExcelTemplate = ExcelTemplateDisplay.IsChanged;
        }
        private void ResetChangeIndicators()
        {
            changedExcelTemplate = false;
            ExcelTemplateDisplay.ResetAllChangeIndicators();
        }

        // --------- IDataErors ----------------------------
        public string this[string columnName] => throw new NotImplementedException();

        public string Error => throw new NotImplementedException();


        // --------- Commands ------------------------------
        public ICommand SaveChangesCommand { get; set; }
        public ICommand CancelChangesCommand { get; set; }
    }
}
