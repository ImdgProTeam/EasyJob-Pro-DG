using EasyJob_ProDG.UI.Data;

namespace EasyJob_ProDG.UI.ViewModel.Conflicts
{
    public class GeneralConflictPanelItemViewModel : ConflictPanelItemViewModel
    {
        // --------------- Private fields ---------------------------------------
        private string _title;
        private string _description;

        // --------------- Internal properties ------------------------------------


        #region Display properties

        public override string DisplayConflictHeader => _title;

        /// <summary>
        /// Titles for group spoilers in Conflict list
        /// </summary>
        public override string ConflictGroupTitle
        {
            get
            {
                if (ConflictType == ConflictTypes.VentRequirement) return "Hold ventilation";
                if (Code.StartsWith("SW19")) return "SW19";
                if (Code.StartsWith("SW22")) return "SW22";
                else return "Stowage";
            }
        }

        #endregion

        private string Description
        {
            get
            {
                return _description;
            }
        }


        protected override string CreateDisplayText()
        {
            string result = Description;
            return result;
        }

        #region Constructor 

        // --------------- Public constructors ----------------------------------

        /// <summary>
        /// Constructor to create General conflict
        /// </summary>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="code">Conflict code</param>
        /// <param name="conflictType">Info (by default)</param>
        public GeneralConflictPanelItemViewModel(string title, string description, string code,
            ConflictTypes conflictType = ConflictTypes.Info)
        {
            _title = title;
            _description = description;
            Code = code;
            ConflictType = conflictType;
        }

        #endregion
    }
}
