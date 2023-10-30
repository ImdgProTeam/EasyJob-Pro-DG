using EasyJob_ProDG.UI.Services;
using EasyJob_ProDG.UI.Settings;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
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
            GenerateTempateTitlesLists();
        }

        #region StartUp logic

        /// <summary>
        /// Creates and updates TemplateTitles lists with template titles obtained from settings.settings
        /// </summary>
        private void GenerateTempateTitlesLists()
        {
            DgTemplateTitles = new List<string>();
            ReeferTemplateTitles = new List<string>();

            foreach(SettingsProperty property in Properties.Settings.Default.Properties)
            {
                if (property.Name.StartsWith("ExcelDgTemplate"))
                {
                    DgTemplateTitles.Add(property.DefaultValue.ToString().Split(',')[0]);
                }
                if (property.Name.StartsWith("ExcelReeferTemplate"))
                {
                    ReeferTemplateTitles.Add(property.DefaultValue.ToString().Split(',')[0]);
                }
            }
        }

        private void LoadCommands()
        {
            SaveChangesCommand = new DelegateCommand(SaveChanges);
            CancelChangesCommand = new DelegateCommand(CancelChanges);
        }

        #endregion



        #region Excel Dg list settings

        bool changedDgExcelTemplate;

        public int SelectedDgTemplate { get; set; }
        public List<string> DgTemplateTitles { get; set; }


        private ExcelDgTemplateWrapper _excelDgTemplate;
        public ExcelDgTemplateWrapper ExcelDgTemplateDisplay
        {
            get
            {
                if (_excelDgTemplate == null)
                {
                    _excelDgTemplate = new ExcelDgTemplateWrapper(settings.ExcelDgTemplate);
                }
                return _excelDgTemplate;
            }
        }

        #endregion


        #region Excel Reefers settings

        bool changedReeferExcelTemplate;

        public int SelectedReeferTemplate { get; set; }
        public List<string> ReeferTemplateTitles { get; set; }


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

        /// <summary>
        /// Rejects any made changes in the window
        /// </summary>
        /// <param name="obj"></param>
        public void CancelChanges(object obj)
        {
            changedDgExcelTemplate = false;
            changedReeferExcelTemplate = false;

            ExcelDgTemplateDisplay.CancelChanges();
            ExcelReeferTemplateDisplay.CancelChanges();
        }

        /// <summary>
        /// Saves all changes made in the window
        /// </summary>
        /// <param name="obj"></param>
        private void SaveChanges(object obj)
        {
            CheckWhatChanged();

            if (changedDgExcelTemplate)
            {
                ExcelDgTemplateDisplay.UploadTemplateChanges();
                uiSettingsService.SaveExcelTemplate(ExcelDgTemplateDisplay.TemplateNameInSettings, ExcelDgTemplateDisplay.TemplateString);
            }
            if (changedReeferExcelTemplate)
            {
                ExcelReeferTemplateDisplay.UploadTemplateChanges();
                uiSettingsService.SaveExcelTemplate(ExcelReeferTemplateDisplay.TemplateNameInSettings, ExcelReeferTemplateDisplay.TemplateString);
            }

            ResetChangeIndicators();
        }

        private void CheckWhatChanged()
        {
            changedDgExcelTemplate = ExcelDgTemplateDisplay.IsChanged;
            changedReeferExcelTemplate = ExcelReeferTemplateDisplay.IsChanged;
        }

        private void ResetChangeIndicators()
        {
            changedDgExcelTemplate = false;
            ExcelDgTemplateDisplay.ResetAllChangeIndicators();
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
