using EasyJob_ProDG.Model.IO.Excel;
using EasyJob_ProDG.UI.Wrapper;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;


namespace EasyJob_ProDG.UI.Settings
{
    /// <summary>
    /// Class is responsible for handling User defined preferences related to UI/display of information.
    /// </summary>
    public class UserUISettings : IUserUISettings
    {
        #region Settings constants

        internal const string _FORMATDECIMAL = "# ### ##0.000";
        internal const string EXCEL_DG_TEMPLATE_PREFIX = "ExcelDgTemplate";
        internal const string EXCEL_REEFER_TEMPLATE_PREFIX = "ExcelReeferTemplate";
        internal const int NumberOfStoredTemplates = 5;

        #endregion

        #region Private locals
        
        private bool _showSummaryOnUpdateCondition;

        #endregion

        #region UI Settings properties

        // Sorting lists
        public enum DgSortOrderPattern { ABC, CBA, LR, RL, LRsnake, RLsnake };
        public DgSortOrderPattern DgSortPattern { get; set; }
        public bool Combine2040BaysWhenSorting { get; private set; }
        public byte LowestTierOnDeck { get; private set; }

        public bool ShowSummaryOnUpdateCondition
        {
            get => _showSummaryOnUpdateCondition;
            set
            {
                _showSummaryOnUpdateCondition = value;
                UpdatePropertiesSettingsDefault(value);
            }
        }

        #endregion

        #region ExcelTemplates
        //Excel

        //Dg
        public ObservableCollection<ExcelDgTemplateWrapper> ExcelDgTemplates { get; set; }

        /// <summary>
        /// Selected Index to be used with collections and in xaml.
        /// Automatically converts value obtained from settings.settings (+/- 1)
        /// </summary>
        public int SelectedDgTemplateIndex
        {
            get => _selectedDgTemplateIndexNumberInSettings - 1;
            set => _selectedDgTemplateIndexNumberInSettings = (byte)(value + 1);
        }

        /// <summary>
        /// Value recorded in settings.settings for the index of chosen template.
        /// Starting from 1...
        /// </summary>
        private byte _selectedDgTemplateIndexNumberInSettings;

        internal void SetExcelDgTemplate()
        {
            ExcelTemplateSetter.SetExcelDgTemplate(ExcelDgTemplates[SelectedDgTemplateIndex].Model);
        }


        //Reefer
        public ObservableCollection<ExcelReeferTemplateWrapper> ExcelReeferTemplates { get; set; }
        public int SelectedReeferTemplateIndex
        {
            get => _selectedReeferTemplateIndexNumberInSettings - 1;
            set => _selectedReeferTemplateIndexNumberInSettings = (byte)(value + 1);
        }

        /// <summary>
        /// Value recorded in settings.settings for the index of chosen template.
        /// Starting from 1...
        /// </summary>
        private byte _selectedReeferTemplateIndexNumberInSettings;

        internal void SetExcelReeferTemplate()
        {
            ExcelTemplateSetter.SetExcelReeferTemplate(ExcelReeferTemplates[SelectedReeferTemplateIndex].Model);
        }

        #endregion


        // -------------- Main methods ----------------------------------------------
        #region Public methods

        /// <summary>
        /// Public accessor to settings
        /// </summary>
        /// <returns></returns>
        public UserUISettings GetSettings()
        {
            return this;
        }

        //TODO: Change hard-coded settings to be dynamically loaded
        public void LoadSettings()
        {
            DgSortPattern = DgSortOrderPattern.LRsnake;
            Combine2040BaysWhenSorting = false;
            LowestTierOnDeck = 72;

            ReadSettingsFromSettings();

            SetExcelTemplates();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Sets SelectedTemplates to Excel Dg and Reefer templates
        /// </summary>
        private void SetExcelTemplates()
        {
            SetExcelDgTemplate();
            SetExcelReeferTemplate();
        }

        /// <summary>
        /// Reads Excel settings from settings.settings
        /// </summary>
        private void ReadSettingsFromSettings()
        {
            _selectedDgTemplateIndexNumberInSettings = Properties.Settings.Default.SelectedExcelDgTemplate;
            _selectedReeferTemplateIndexNumberInSettings = Properties.Settings.Default.SelectedExcelReeferTemplate;

            _showSummaryOnUpdateCondition = Properties.Settings.Default.ShowSummaryOnUpdateCondition;

            CreateTemplateCollections();
        }

        /// <summary>
        /// Creates collections of ExcelTemplates for dg and reefers from settings.settings
        /// </summary>
        private void CreateTemplateCollections()
        {
            ExcelDgTemplates = new ObservableCollection<ExcelDgTemplateWrapper>();
            ExcelReeferTemplates = new ObservableCollection<ExcelReeferTemplateWrapper>();

            for (int i = 1; i <= NumberOfStoredTemplates; i++)
            {
                //DG
                var templateName = EXCEL_DG_TEMPLATE_PREFIX + i;
                var property = Properties.Settings.Default[templateName];
                if (property != null)
                {
                    var template = new ExcelDgTemplate();
                    template.CreateTemplate(property.ToString());
                    template.TemplateSettingsName = templateName;

                    ExcelDgTemplates.Add(new ExcelDgTemplateWrapper(template));
                }

                //Reefers
                templateName = EXCEL_REEFER_TEMPLATE_PREFIX + i;
                property = Properties.Settings.Default[templateName];
                if (property != null)
                {
                    var template = new ExcelReeferTemplate();
                    template.CreateTemplate(property.ToString());
                    template.TemplateSettingsName = templateName;

                    ExcelReeferTemplates.Add(new ExcelReeferTemplateWrapper(template));
                }
            }
        }

        #endregion


        /// <summary>
        /// Saves ExcelTemplate in settings.settings
        /// </summary>
        /// <param name="templateSettingsName">Template property name in settings.settings</param>
        /// <param name="template">Template in string format</param>
        public void SaveExcelTemplate(string templateSettingsName, string template)
        {
            Properties.Settings.Default[templateSettingsName] = template;
        }

        /// <summary>
        /// Saves selected excel template indeces for dg and reefer in settings.settings
        /// </summary>
        public void SaveSelectedExcelTemplateIndeces()
        {
            Properties.Settings.Default.SelectedExcelDgTemplate = _selectedDgTemplateIndexNumberInSettings;
            SetExcelDgTemplate();

            Properties.Settings.Default.SelectedExcelReeferTemplate = _selectedReeferTemplateIndexNumberInSettings;
            SetExcelReeferTemplate();
        }

        /// <summary>
        /// Sets value to property in Properties.Settings.Default
        /// </summary>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        private void UpdatePropertiesSettingsDefault(object value, [CallerMemberName] string propertyName = null)
        {
            string Value = value.ToString();
            var propertyType = Properties.Settings.Default.Properties[propertyName]?.PropertyType;
            if (propertyType is null) return;

            if (propertyType.Equals(typeof(string)))
                Properties.Settings.Default[propertyName] = Value;
            else if (propertyType.Equals(typeof(byte)))
                Properties.Settings.Default[propertyName] = byte.Parse(Value);
            else if (propertyType.Equals(typeof(double)))
                Properties.Settings.Default[propertyName] = double.Parse(Value);
            else if (propertyType.Equals(typeof(bool)))
                Properties.Settings.Default[propertyName] = bool.Parse(Value);
        }

    }
}
