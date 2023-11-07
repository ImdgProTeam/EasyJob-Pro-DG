using EasyJob_ProDG.UI.Services;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.ViewModel
{
    public class SettingsWindowVM : Observable, IDataErrorInfo
    {
        ISettingsService uiSettingsService = new SettingsService();

        private bool isFirstTimeDgTemplateValuesSet;
        private bool isFirstTimeReeferTemplateValuesSet;
        private bool isWindowOpened;

        /// <summary>
        /// Property contains all available excel column numbers.
        /// (Is used by View).
        /// </summary>
        public static List<char> Columns { get { return Model.IO.Excel.WithXl.Columns; } }

        private bool HasChangedIndex =>
            selectedExcelDgTemplateIndex != uiSettingsService.SelectedExcelDgTemplateIndex
            || selectedExcelReeferTemplateIndex != uiSettingsService.SelectedExcelReeferTemplateIndex;



        #region StartUp logic

        private void LoadCommands()
        {
            SaveChangesCommand = new DelegateCommand(SaveChanges, SaveChangesCanExecute);
            CancelChangesCommand = new DelegateCommand(CancelChanges);
            WindowLoaded = new DelegateCommand(OnWindowLoaded);
            WindowClosed = new DelegateCommand(OnWindowClosed);
            DgControlLoaded = new DelegateCommand(OnDgControlLoaded);
            DgControlUnloaded = new DelegateCommand(OnDgControlUnloaded);
            ReeferControlLoaded = new DelegateCommand(OnReeferControlLoaded);
            ReeferControlUnloaded = new DelegateCommand(OnReeferControlUnloaded);
        }



        #region Window and controls Loaded/Unloaded handlers

        //Logic with events created to avoid reset to 0 of combobox index on re-open of the control
        private void OnDgControlLoaded(object obj)
        {
            if (!isFirstTimeDgTemplateValuesSet)
                SetExcelDgTemplateValues();
        }

        private void OnDgControlUnloaded(object obj)
        {
            if (isWindowOpened)
                isFirstTimeDgTemplateValuesSet = true;
        }

        private void OnReeferControlUnloaded(object obj)
        {
            if (isWindowOpened)
                isFirstTimeReeferTemplateValuesSet = true;
        }

        private void OnReeferControlLoaded(object obj)
        {
            if (!isFirstTimeReeferTemplateValuesSet)
                SetExcelReeferTemplateValues();
        }

        private void OnWindowClosed(object obj)
        {
            isFirstTimeDgTemplateValuesSet = false;
            isFirstTimeReeferTemplateValuesSet = false;
            isWindowOpened = false;
        }

        private void OnWindowLoaded(object obj)
        {
            isWindowOpened = true;
        }

        #endregion

        #region Set Excel Template values

        private void SetExcelDgTemplateValues()
        {
            SelectedExcelDgTemplateIndex = uiSettingsService.SelectedExcelDgTemplateIndex;
            OnPropertyChanged(nameof(SelectedExcelDgTemplateIndex));
        }

        private void SetExcelReeferTemplateValues()
        {
            SelectedExcelReeferTemplateIndex = uiSettingsService.SelectedExcelReeferTemplateIndex;
            OnPropertyChanged(nameof(SelectedExcelReeferTemplateIndex));
        }

        /// <summary>
        /// Sets Templates Collection and selected template for Dg and Reefers from uiSettingsService
        /// </summary>
        internal void SetExcelTemplateValues()
        {
            ExcelDgTemplates = uiSettingsService.ExcelDgTemplates;
            ExcelReeferTemplates = uiSettingsService.ExcelReeferTemplates;

            SetExcelDgTemplateValues();
            SetExcelReeferTemplateValues();
        }

        #endregion

        #endregion



        #region Excel Dg list settings

        public ObservableCollection<ExcelDgTemplateWrapper> ExcelDgTemplates { get; set; }

        public ExcelDgTemplateWrapper SelectedExcelDgTemplate
        {
            get => selectedExcelDgTemplate;
            set
            {
                if (selectedExcelDgTemplate == value) return;

                selectedExcelDgTemplate = value;
                OnPropertyChanged();
            }
        }
        private ExcelDgTemplateWrapper selectedExcelDgTemplate;
        public int SelectedExcelDgTemplateIndex
        {
            get => selectedExcelDgTemplateIndex;
            set
            {
                if (selectedExcelDgTemplateIndex == value) return;

                selectedExcelDgTemplateIndex = value;
            }
        }
        private int selectedExcelDgTemplateIndex;

        public string ExcelDgNoticeText => "Values on this page will be used to read DG list from excel as well as to generate an excel DG list in required format.";

        #endregion


        #region Excel Reefers settings

        public ObservableCollection<ExcelReeferTemplateWrapper> ExcelReeferTemplates { get; set; }

        public ExcelReeferTemplateWrapper SelectedExcelReeferTemplate
        {
            get => selectedExcelReeferTemplate;
            set
            {
                if (selectedExcelReeferTemplate == value) return;

                selectedExcelReeferTemplate = value;
                OnPropertyChanged();
            }
        }
        private ExcelReeferTemplateWrapper selectedExcelReeferTemplate;

        public int SelectedExcelReeferTemplateIndex
        {
            get => selectedExcelReeferTemplateIndex;
            set
            {
                if (selectedExcelReeferTemplateIndex == value) return;

                selectedExcelReeferTemplateIndex = value;
            }
        }
        private int selectedExcelReeferTemplateIndex;

        public string ExcelReeferNoticeText =>
            "Values on this page will be used to read reefer manifest from excel.\n" +
            "Only first excel sheet in workbook will be checked.";

        #endregion


        #region Cances/Save logic

        /// <summary>
        /// Rejects any made changes in the window
        /// </summary>
        /// <param name="obj"></param>
        private void CancelChanges(object obj)
        {
            foreach (var template in ExcelDgTemplates)
            {
                if (!template.HasChanges) continue;
                template.ResetOriginalValues();
            }
            foreach (var template in ExcelReeferTemplates)
            {
                if (!template.HasChanges) continue;
                template.ResetOriginalValues();
            }
        }

        /// <summary>
        /// Saves all changes made in the window
        /// </summary>
        /// <param name="obj"></param>
        private void SaveChanges(object obj)
        {
            foreach (var template in ExcelDgTemplates)
            {
                if (!template.HasChanges) continue;
                template.SaveChanges();
                uiSettingsService.SaveExcelTemplate(template.TemplateNameInSettings, template.TemplateString);
            }
            foreach (var template in ExcelReeferTemplates)
            {
                if (!template.HasChanges) continue;
                template.SaveChanges();
                uiSettingsService.SaveExcelTemplate(template.TemplateNameInSettings, template.TemplateString);
            }

            uiSettingsService.SetSelectedExcelDgTemplateIndex(selectedExcelDgTemplateIndex);
            uiSettingsService.SetSelectedExcelReeferTemplateIndex(selectedExcelReeferTemplateIndex);
            uiSettingsService.SaveSelectedExcelTemplateIndeces();
        }

        private bool SaveChangesCanExecute(object obj)
        {
            return HasChangedIndex
                || ExcelDgTemplates.Any(t => t.HasChanges)
                || ExcelReeferTemplates.Any(t => t.HasChanges);
        }

        #endregion

        #region Constructor

        //---------------------- Constructor ---------------------------------------
        public SettingsWindowVM()
        {
            LoadCommands();
            SetExcelTemplateValues();
        }

        #endregion

        #region IDataErrors

        // --------- IDataErors ----------------------------
        public string this[string columnName] => throw new NotImplementedException();

        public string Error => throw new NotImplementedException();

        #endregion

        #region Commands

        // --------- Commands ------------------------------
        public ICommand SaveChangesCommand { get; set; }
        public ICommand CancelChangesCommand { get; set; }
        public ICommand WindowLoaded { get; set; }
        public ICommand WindowClosed { get; set; }
        public ICommand DgControlLoaded { get; set; }
        public ICommand DgControlUnloaded { get; set; }
        public ICommand ReeferControlLoaded { get; set; }
        public ICommand ReeferControlUnloaded { get; set; }

        #endregion
    }
}
