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

        //Excel
        private ExcelTemplate _excelTemplate;
        public ExcelTemplate ExcelTemplate
        {
            get { if (_excelTemplate == null) { _excelTemplate = new ExcelTemplate(); } return _excelTemplate; }
            set { _excelTemplate = value; }
        }


        // DgTable view settings
        public bool IncludeTechnicalNameToProperShippingName { get; set; }


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
            ExcelTemplate.ReadTemplate();
        }
    }
}
