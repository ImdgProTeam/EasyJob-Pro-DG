using EasyJob_ProDG.Data.Info_data;
using EasyJob_ProDG.UI.Wrapper;
using EasyJob_ProDG.UI.Utility;
using System.Linq;

namespace EasyJob_ProDG.UI.ViewModel
{
    public class ConflictPanelItemViewModel : Observable
    {
        // --------------- Private fields ---------------------------------------

        private readonly DgWrapper _dgUnit;
        private readonly DgWrapper _dgB;
        private readonly bool _segrConflict;


        // --------------- Public properties ------------------------------------

        public bool IsSegregationConflict => _segrConflict;
        public bool IsStowageConflict => !_segrConflict;

        /// <summary>
        /// Textual description of the conflict to be displayed in Conflict item.
        /// </summary>
        public string Text
        {
            get
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
                if (_segrConflict)
                    result += "\nis in conflict with\n" + _dgB.DisplayContainerNumber 
                        + " (class " + _dgB.AllDgClasses + (_dgB.DgClass == "Reefer" ? "Reefer" : $" unno {_dgB.Unno:0000}") + ")\n"
                        + "in position: " + _dgB.Location;
                result += "\n" + Description;
                return result;
            }
            set
            {
                OnPropertyChanged();
            }
        }
        public int Unno => _dgUnit.Unno;
        public string ContainerNumber => _dgUnit.ContainerNumber;
        public string DisplayContainerNumber => _dgUnit.DisplayContainerNumber;
        public int DgID => _dgUnit.Model.ID;
        public string Location => _dgUnit.Location;
        public string Code { get; }
        public string GroupParam { get; set; }
        public int ConflictingDgUnno => _dgB?.Unno ?? 0;
        public string ConflictingDgNumber => _dgB?.DisplayContainerNumber;
        public string ConflictingDgLocation => _dgB?.Location;
        public string Description
        {
            get
            {
                //case fishmeal protected from heat
                if(string.Equals(Code, "SSC3b"))
                    return CodesDictionary.ConflictCodes[Code] + "\n" + Surrounded;
                if (CodesDictionary.ConflictCodesPrefixes.Contains(CodesDictionary.GetCodePrefix(Code)))
                    return CodesDictionary.ConflictCodes[Code];
                else return Code + ": " + (_segrConflict ? CodesDictionary.Segregation[Code] 
                        : (CodesDictionary.Stowage[Code] + (Code == "SW1" ? "\n" + Surrounded : ""))
                        );
            }
        }

        /// <summary>
        /// Describes occupied container location around the unit.
        /// </summary>
        public string Surrounded => "Unit protected from: " + _dgUnit.Surrounded;

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


        // --------------- Public constructors ----------------------------------

        public ConflictPanelItemViewModel(DgWrapper dgUnit, string code, bool segrConflict = false, DgWrapper dgB = null)
        {
            _dgUnit = dgUnit;
            _dgB = dgB;
            Code = code;
            GroupParam = ContainerNumber;
            _segrConflict = segrConflict;
        }

        /// <summary>
        /// Calls OnPropertyChanged for its properties
        /// </summary>
        /// <param name="obj"></param>
        internal void RefreshConflictText()
        {
            OnPropertyChanged(nameof(Text));
            OnPropertyChanged(nameof(ContainerNumber));
            OnPropertyChanged(nameof(DisplayContainerNumber));
        }


        // ---------------- Overrided system methods ----------------------------

        public override string ToString()
        {
            string result = DisplayContainerNumber + " in " + Location + " unno " + Unno + (IsSegregationConflict ? (" in conf with" + ConflictingDgNumber) : null);
            return result;
        }

        public bool Equals(ConflictPanelItemViewModel conflict)
        {
            return this.ContainerNumber == conflict.ContainerNumber
                && this.Location == conflict.Location
                && this.Unno == conflict.Unno
                && this._dgUnit.AllDgClasses.All(c => conflict._dgUnit.AllDgClasses.Any(x => string.Equals(x, c)))
                && Code == conflict.Code
                && DgID == conflict.DgID;
        }
    }
}
