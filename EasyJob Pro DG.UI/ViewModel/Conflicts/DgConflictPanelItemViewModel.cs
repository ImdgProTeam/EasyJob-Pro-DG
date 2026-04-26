using EasyJob_ProDG.Data.Info_data;
using EasyJob_ProDG.UI.Data;
using EasyJob_ProDG.UI.Wrapper;
using System.Linq;

namespace EasyJob_ProDG.UI.ViewModel.Conflicts
{
    public class DgConflictPanelItemViewModel : ConflictPanelItemViewModel
    {
        const string GROUP_TITLE_SW19 = "SW19";
        const string GROUP_TITLE_SW22 = "SW22";
        const string GROUP_TITLE_STOWAGE = "Stowage";
        const string GROUP_TITLE_SEGREGATION = "Segregation";
        const string GROUP_TITLE_HANDLING = "Handling";


        // --------------- Private fields ---------------------------------------

        private readonly DgWrapper _dgUnit;
        private readonly DgWrapper _dgB;

        #region Internal members

        // --------------- Internal properties ------------------------------------
        internal bool IsSegregationConflict => ConflictType == ConflictTypes.Segregation;
        internal bool IsStowageConflict => ConflictType == ConflictTypes.Stowage;


        internal int Unno => _dgUnit.Unno;
        internal string ContainerNumber => _dgUnit.ContainerNumber;
        internal int DgID => _dgUnit.Model.ID;
        internal string Location => _dgUnit.Location;

        internal int ConflictingDgUnno => _dgB?.Unno ?? 0;
        internal string ConflictingDgNumber => _dgB?.DisplayContainerNumber;
        internal string ConflictingDgLocation => _dgB?.Location;

        #endregion

        #region Display properties

        public override string DisplayConflictHeader => ContainerNumber;

        /// <summary>
        /// Titles for group spoilers in Conflict list
        /// </summary>
        public override string ConflictGroupTitle
        {
            get
            {
                if (Code.StartsWith("SW19")) return GROUP_TITLE_SW19;
                if (Code.StartsWith("SW22")) return GROUP_TITLE_SW22;
                if (ConflictType == ConflictTypes.Handling) return GROUP_TITLE_HANDLING;
                if (IsSegregationConflict) return GROUP_TITLE_SEGREGATION;
                else return GROUP_TITLE_STOWAGE;
            }
        }

        #endregion

        #region Protected override methods

        protected override string CreateDisplayText()
        {
            if (Code.StartsWith("vent"))
                return "Mechanical ventilation shall be started in cargo hold " + _dgUnit.HoldNr;
            if (Code.StartsWith("SW19"))
            {
                return
                    "SW19 For batteries transported in accordance with special provisions 376 or 377, " +
                    "category C, unless transported on a short international voyage.\n" +
                    "Please check cargo documents of the units " +
                    _dgUnit.DisplayContainerNumber + " in " + _dgUnit.Location;
            }
            if (Code.StartsWith("SW22"))
            {
                return
                    "SW22 For WASTE AEROSOLS: category C, clear of living quarters.\n" +
                    "Please check cargo documents of the unit " +
                    _dgUnit.DisplayContainerNumber + " in " + _dgUnit.Location;
            }

            string result = $"Unit {_dgUnit.DisplayContainerNumber} (class {_dgUnit.AllDgClasses} UNNo {_dgUnit.Unno:0000})\n" +
                $"Position: {_dgUnit.Location}";
            if (IsSegregationConflict)
                result += "\nis in conflict with\n" + _dgB.DisplayContainerNumber
                    + " (class " + _dgB.AllDgClasses + (_dgB.DgClass == "Reefer" ? "" : $" UNNo {_dgB.Unno:0000}") + ")\n"
                    + "in position: " + _dgB.Location;
            result += "\n" + Description;
            return result;
        }

        #endregion

        #region Private methods and properties

        private string Description
        {
            get
            {
                //case fishmeal protected from heat
                if (string.Equals(Code, "SSC3b"))
                    return CodesDictionary.ConflictCodes[Code] + "\n" + Surrounded;
                if (CodesDictionary.ConflictCodesPrefixes.Contains(CodesDictionary.GetCodePrefix(Code)))
                    return CodesDictionary.ConflictCodes[Code];
                else return Code + ": " + (IsSegregationConflict ? CodesDictionary.Segregation[Code]
                        : (CodesDictionary.Stowage[Code] + (Code == "SW1" ? "\n" + Surrounded : ""))
                        );
            }
        }

        /// <summary>
        /// Describes occupied container location around the unit.
        /// </summary>
        private string Surrounded => "Unit protected from: " + _dgUnit.Surrounded; 

        #endregion

        #region Override system methods
        // ---------------- Overrided system methods ----------------------------
        public override string ToString()
        {
            string result = DisplayConflictHeader + " in " + Location + " unno " + Unno + (IsSegregationConflict ? (" in conf with" + ConflictingDgNumber) : null);
            return result;
        }

        public override bool Equals(ConflictPanelItemViewModel conflict)
        {
            var dgConflict = conflict as DgConflictPanelItemViewModel;
            if (dgConflict is null) return false;

            return this.ContainerNumber == dgConflict.ContainerNumber
                && this.Location == dgConflict.Location
                && this.Unno == dgConflict.Unno
                && this._dgUnit.AllDgClasses.All(c => dgConflict._dgUnit.AllDgClasses.Any(x => string.Equals(x, c)))
                && Code == dgConflict.Code
                && DgID == dgConflict.DgID
                && ConflictingDgUnno == dgConflict.ConflictingDgUnno
                && ConflictingDgNumber == dgConflict.ConflictingDgNumber;
        }

        #endregion

        #region Constructor 

        // --------------- Public constructors ----------------------------------

        /// <summary>
        /// Constructor to create Stowage and Segregation conflicts
        /// </summary>
        /// <param name="dgUnit"></param>
        /// <param name="code">Conflict code</param>
        /// <param name="conflictType">Stowage (by default) or segregation</param>
        /// <param name="dgB">Null in case of stowage conflict</param>
        public DgConflictPanelItemViewModel(DgWrapper dgUnit, string code,
            ConflictTypes conflictType = ConflictTypes.Stowage, DgWrapper dgB = null)
        {
            _dgUnit = dgUnit;
            _dgB = dgB;
            Code = code;
            this.ConflictType = conflictType;
        }

        #endregion
    }
}
