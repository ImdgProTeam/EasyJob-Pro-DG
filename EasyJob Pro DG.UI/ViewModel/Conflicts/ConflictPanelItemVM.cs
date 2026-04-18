using EasyJob_ProDG.Data.Info_data;
using EasyJob_ProDG.UI.Wrapper;
using EasyJob_ProDG.UI.Utility;
using System.Linq;
using EasyJob_ProDG.UI.Data;

namespace EasyJob_ProDG.UI.ViewModel
{
    public class ConflictPanelItemViewModel : Observable
    {
        // --------------- Private fields ---------------------------------------

        private readonly DgWrapper _dgUnit;
        private readonly DgWrapper _dgB;
        private bool isVisible;
        private int _conflictID;


        #region Internal members

        // --------------- Internal properties ------------------------------------

        internal ConflictTypes ConflictType { get; private set; }
        internal bool IsSegregationConflict => ConflictType == ConflictTypes.Segregation;
        internal bool IsStowageConflict => ConflictType == ConflictTypes.Stowage;


        internal int Unno => _dgUnit.Unno;
        internal string ContainerNumber => _dgUnit.ContainerNumber;
        internal int DgID => _dgUnit.Model.ID;
        internal string Location => _dgUnit.Location;
        internal string Code { get; }

        internal int ConflictingDgUnno => _dgB?.Unno ?? 0;
        internal string ConflictingDgNumber => _dgB?.DisplayContainerNumber;
        internal string ConflictingDgLocation => _dgB?.Location;

        #endregion


        #region Display properties

        /// <summary>
        /// Textual description of the conflict to be displayed in Conflict item.
        /// </summary>
        public string DisplayText
        {
            get
            {
                return CreateDisplayText();
            }
            set
            {
                OnPropertyChanged();
            }
        }

        public string DisplayConflictHeader => _dgUnit.DisplayContainerNumber;

        /// <summary>
        /// Titles for group spoilers in Conflict list
        /// </summary>
        public string ConflictGroupTitle
        {
            get
            {
                if (Code.StartsWith("vent")) return "Hold ventilation";
                if (Code.StartsWith("SW19")) return "SW19";
                if (Code.StartsWith("SW22")) return "SW22";
                if (IsSegregationConflict) return "Segregation";
                else return "Stowage";
            }
        }

        

        public bool IsVisible 
        { get => isVisible;
            set
            {
                isVisible = value;
                OnPropertyChanged();
            }
        }

        #endregion


        private string CreateDisplayText()
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

        #region Methods

        /// <summary>
        /// Calls OnPropertyChanged for its properties
        /// </summary>
        /// <param name="obj"></param>
        internal void RefreshConflictText()
        {
            OnPropertyChanged(nameof(DisplayText));
            OnPropertyChanged(nameof(ContainerNumber));
            OnPropertyChanged(nameof(DisplayConflictHeader));
        }

        #endregion

        #region Override system methods
        // ---------------- Overrided system methods ----------------------------

        public override string ToString()
        {
            string result = DisplayConflictHeader + " in " + Location + " unno " + Unno + (IsSegregationConflict ? (" in conf with" + ConflictingDgNumber) : null);
            return result;
        }

        public bool Equals(ConflictPanelItemViewModel conflict)
        {
            return this.ContainerNumber == conflict.ContainerNumber
                && this.Location == conflict.Location
                && this.Unno == conflict.Unno
                && this._dgUnit.AllDgClasses.All(c => conflict._dgUnit.AllDgClasses.Any(x => string.Equals(x, c)))
                && Code == conflict.Code
                && DgID == conflict.DgID
                && ConflictingDgUnno == conflict.ConflictingDgUnno
                && ConflictingDgNumber == conflict.ConflictingDgNumber;
        }
        #endregion

        #region Constructor 

        // --------------- Public constructors ----------------------------------

        public ConflictPanelItemViewModel(DgWrapper dgUnit, string code,
            ConflictTypes conflictType = ConflictTypes.Stowage, DgWrapper dgB = null)
        {
            _dgUnit = dgUnit;
            _dgB = dgB;
            Code = code;
            ConflictType = conflictType;
            isVisible = true;
        }
        #endregion
    }
}
