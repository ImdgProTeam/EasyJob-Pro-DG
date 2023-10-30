using EasyJob_ProDG.Model.IO.Excel;

namespace EasyJob_ProDG.UI.Settings
{
    public class UserUISettings : IUserUISettings
    {
        // Sorting lists
        public enum DgSortOrderPattern { ABC, CBA, LR, RL, LRsnake, RLsnake };
        public DgSortOrderPattern DgSortPattern { get; set; }
        public bool Combine2040BaysWhenSorting { get; private set; }
        public byte LowestTierOnDeck { get; private set; }

        #region ExcelTemplates
        //Excel
        private ExcelDgTemplate _excelDgTemplate;
        public ExcelDgTemplate ExcelDgTemplate
        {
            get { if (_excelDgTemplate == null) { _excelDgTemplate = new ExcelDgTemplate(); } return _excelDgTemplate; }
            set { _excelDgTemplate = value; }
        }

        private ExcelReeferTemplate _excelReeferTemplate;
        public ExcelReeferTemplate ExcelReeferTemplate
        {
            get { if (_excelReeferTemplate == null) { _excelReeferTemplate = new ExcelReeferTemplate(); } return _excelReeferTemplate; }
            set { _excelReeferTemplate = value; }
        }
        #endregion



        internal const string _FORMATDECIMAL = "# ### ##0.000";


        // -------------- Main methods ----------------------------------------------

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

            //read ExcelDgTemplate 
            TryCreateExcelTemplateFromSettings(ExcelDgTemplate, Properties.Settings.Default.SelectedExcelDgTemplate);

            //read ExcelReeferTemplate
            TryCreateExcelTemplateFromSettings(ExcelReeferTemplate, Properties.Settings.Default.SelectedExcelReeferTemplate);
        }

        /// <summary>
        /// Trys to read ExcelTemplate chosen from settings.settings and apply it.
        /// Applies default template in case of failure.
        /// </summary>
        /// <param name="template"><see cref="ExcelTemplate"/> derived class</param>
        /// <param name="selectedTemplateNumber">Number in the name of template in settings.</param>
        private void TryCreateExcelTemplateFromSettings(ExcelTemplate template, byte selectedTemplateNumber)
        {
            try
            {
                string templateBaseTitle = template is ExcelDgTemplate ? "ExcelDgTemplate" : "ExcelReeferTemplate";
                string propertyName = templateBaseTitle + selectedTemplateNumber;
                template.TemplateSettingsName = propertyName;
                
                var propertyValue = Properties.Settings.Default[propertyName];
                if (string.IsNullOrWhiteSpace(propertyValue.ToString())) 
                    throw new System.Exception($"Property {propertyName} returned unknown value");

                template.ApplyTemplate(propertyValue.ToString());
            }
            catch (System.Exception)
            {
                template.ApplyDefaultTemplate();
            }
        }
    }
}
