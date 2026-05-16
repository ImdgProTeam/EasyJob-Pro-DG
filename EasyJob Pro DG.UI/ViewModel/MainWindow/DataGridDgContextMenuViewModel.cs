using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.Wrapper;

namespace EasyJob_ProDG.UI.ViewModel
{
    public class DataGridDgContextMenuViewModel : Observable
    {
        #region Name constants

        const string PSN_LTDQTY = "LIMITED QUANTITY";
        const string PSN_MAX1L = "MAXIMUM 1 LITRE";
        const string PSN_STABILIZED = "STABILIZED";
        const string PSN_WASTE = "WASTE";
        const string PSN_EMPTY_UNCLEANED = "EMPTY UNCLEANED";
        const string PSN_COOLANT = "AS COOLANT";
        const string PSN_CONDITIONER = "AS CONDITIONER";

        #endregion

        #region Private fields

        private DgWrapper SelectedDg { get; set; } 

        #endregion

        #region Header titles

        public string HeaderTitleLTDQTY => PSN_LTDQTY;
        public string HeaderTitleMAX1L => PSN_MAX1L;
        public string HeaderTitleSTABILIZED => PSN_STABILIZED;
        public string HeaderTitleWASTE => PSN_WASTE;
        public string HeaderTitleEMPTY_UNCLEANED => PSN_EMPTY_UNCLEANED;
        public string HeaderTitleCOOLANT => PSN_COOLANT;
        public string HeaderTitleCONDITIONER => PSN_CONDITIONER;

        #endregion

        #region Public properties

        /// <summary>
        /// Property used for context menu of selected row
        /// </summary>
        public bool IsTechnicalNameOfSelectedDgIncluded => SelectedDg?.IsTechnicalNameIncluded ?? false;
        public bool IsToNameIncludedLimitedQuantity => SelectedDg?.Name.Contains(PSN_LTDQTY) ?? false;
        public bool IsToNameIncludedMax1l => SelectedDg?.Name.Contains(PSN_MAX1L) ?? false;
        public bool IsToNameIncludedStabilized => SelectedDg?.Name.Contains(PSN_STABILIZED) ?? false;
        public bool IsToNameIncludedWaste => SelectedDg?.Name.Contains(PSN_WASTE) ?? false;
        public bool IsToNameIncludedEmptyUncleaned => SelectedDg?.Name.Contains(PSN_EMPTY_UNCLEANED) ?? false;
        public bool IsToNameIncludedAsCoolant => SelectedDg?.Name.Contains(PSN_COOLANT) ?? false;
        public bool IsToNameIncludedAsConditioner => SelectedDg?.Name.Contains(PSN_CONDITIONER) ?? false; 

        #endregion

        #region Internal Methods

        /// <summary>
        /// Calls OnPropertyChanged() method on all public properties
        /// </summary>
        internal void RefreshAllIsChecked()
        {
            OnPropertyChanged(null);
        }

        /// <summary>
        /// Sets <see cref="SelectedDg"/>
        /// </summary>
        /// <param name="selectedDg"></param>
        internal void SetSelectedDg(DgWrapper selectedDg)
        {
            SelectedDg = selectedDg;
        } 


        /// <summary>
        /// Appends <see cref="SelectedDg"/> Name with text provided in obj in required format.
        /// </summary>
        /// <param name="obj">Command parameter = text to add.</param>
        internal void AppendProperShippingName(object obj)
        {
            bool isIncluded = false;

            switch ((string)obj)
            {
                case PSN_LTDQTY:
                    isIncluded = IsToNameIncludedLimitedQuantity;
                    SelectedDg.IsLq = !isIncluded;
                    break;
                case PSN_MAX1L:
                    isIncluded = IsToNameIncludedMax1l;
                    break;
                case PSN_STABILIZED:
                    isIncluded = IsToNameIncludedStabilized;
                    break;
                case PSN_WASTE:
                    isIncluded = IsToNameIncludedWaste;
                    break;
                case PSN_EMPTY_UNCLEANED:
                    isIncluded = IsToNameIncludedEmptyUncleaned;
                    break;
                case PSN_COOLANT:
                    isIncluded = IsToNameIncludedAsCoolant;
                    break;
                case PSN_CONDITIONER:
                    isIncluded = IsToNameIncludedAsConditioner;
                    break;
                default:
                    break;
            }

            if (!isIncluded)
            {
                SelectedDg.Name += FormattedNameAppendix((string)obj);
            }
            else
            {
                SelectedDg.Name = SelectedDg.Name.Replace(FormattedNameAppendix((string)obj), "").Trim();
            }
        }

        #endregion

        #region Private methods

        private string FormattedNameAppendix(string textToAdd)
        {
            return ", " + textToAdd;
        } 

        #endregion

    }    
}      
