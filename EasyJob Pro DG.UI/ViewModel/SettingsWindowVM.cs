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

        /// <summary>
        /// Property contains all available excel column numbers.
        /// (Is used by View).
        /// </summary>
        public static List<char> Columns { get { return Model.IO.Excel.WithXl.Columns; } }

        //---------------------- Constructor ---------------------------------------
        public SettingsWindowVM()
        {
            settings = uiSettingsService.GetSettings();
            LoadCommands();
        }

        #region StartUp logic

        private void LoadCommands()
        {
            SaveChangesCommand = new DelegateCommand(SaveChanges);
            CancelChangesCommand = new DelegateCommand(CancelChanges);
        }

        #endregion



        #region Excel Dg list settings

        bool changedExcelTemplate;

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

        #endregion


        #region Excel Reefers settings

        bool changedReeferExcelTemplate;

        private ExcelReeferTemplateWrapper _excelReeferTemplate;
        public ExcelReeferTemplateWrapper ExcelReeferTemplateDisplay
        {
            get
            {
                if (_excelReeferTemplate == null)
                {
                    _excelReeferTemplate = new ExcelReeferTemplateWrapper(settings.ExcelReeferTemplate);
                }
                return _excelReeferTemplate;
            }
        }


        #endregion


        #region Cances/Save logic
        public void CancelChanges(object obj)
        {
            changedExcelTemplate = false;
            ExcelTemplateDisplay.CancelChanges();
            ExcelReeferTemplateDisplay.CancelChanges();
        }

        private void SaveChanges(object obj)
        {
            CheckWhatChanged();

            if (changedExcelTemplate)
            {
                ExcelTemplateDisplay.UploadTemplateChanges();
                uiSettingsService.SaveExcelTemplate(settings.ExcelTemplate);
            }
            if (changedReeferExcelTemplate)
            {

                ExcelReeferTemplateDisplay.UploadTemplateChanges();
                uiSettingsService.SaveReeferExcelTemplate(ExcelReeferTemplateDisplay.GetTemplateString());
            }


            ResetChangeIndicators();
        }

        private void CheckWhatChanged()
        {
            changedExcelTemplate = ExcelTemplateDisplay.IsChanged;
            changedReeferExcelTemplate = ExcelReeferTemplateDisplay.IsChanged;
        }

        private void ResetChangeIndicators()
        {
            changedExcelTemplate = false;
            ExcelTemplateDisplay.ResetAllChangeIndicators();
            changedReeferExcelTemplate= false;
            ExcelReeferTemplateDisplay.ResetAllChangeIndicators();
        }

        #endregion


        // --------- IDataErors ----------------------------
        public string this[string columnName] => throw new NotImplementedException();

        public string Error => throw new NotImplementedException();


        // --------- Commands ------------------------------
        public ICommand SaveChangesCommand { get; set; }
        public ICommand CancelChangesCommand { get; set; }
    }
}
