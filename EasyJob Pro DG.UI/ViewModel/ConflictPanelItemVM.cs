using EasyJob_ProDG.Data.Info_data;
using EasyJob_ProDG.UI.Wrapper;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.Messages;

namespace EasyJob_ProDG.UI.ViewModel
{
    public class ConflictPanelItemViewModel : Observable
    {
        // --------------- Private fields ---------------------------------------

        private readonly DgWrapper _dgUnit;
        private readonly DgWrapper _dgB;
        private readonly bool _segrConflict;


        // --------------- Public properties ------------------------------------

        public bool IsSegregationConflict { get; set; }
        public bool IsStowageConflict { get; set; }
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
                        _dgUnit.ContainerNumber + " in " + _dgUnit.Location;
                }
                if (Code.StartsWith("SW22"))
                {
                    return
                        "SW22 For WASTE AEROSOLS: category C, clear of living quarters.\n" +
                        "Please check cargo documents of the unit " +
                        _dgUnit.ContainerNumber + " in " + _dgUnit.Location;
                }
                string result = "Unit " + _dgUnit.ContainerNumber + " (class " + _dgUnit.AllDgClasses + $" unno {_dgUnit.Unno:0000}) in " + _dgUnit.Location;
                if (_segrConflict)
                    result += " is in conflict with " + _dgB.ContainerNumber + " (class " + _dgB.AllDgClasses + (_dgB.AllDgClasses == "Reefer" ? "" : $" unno {_dgB.Unno:0000}") + ") in " + _dgB.Location;
                else result += ":";
                result += " " + Description;
                return result;
            }
            set
            {
                OnPropertyChanged();
            }
        } 
        public int Unno => _dgUnit.Unno;
        public string ContainerNumber => _dgUnit.ContainerNumber;
        public int DgID => _dgUnit.Model.ID;
        public string Location => _dgUnit.Location;
        public string Code { get; }
        public string GroupParam { get; set; }
        public int ConflictingDgUnno => _dgB?.Unno ?? 0;
        public string ConflictingDgNumber => _dgB?.ContainerNumber;
        public string ConflictingDgLocation => _dgB?.Location;
        public string Description
        {
            get
            {
                string result = Code + " ";
                if (Code.StartsWith("SSC") || Code.StartsWith("SGC") || Code.StartsWith("EXPL"))
                    result += CodesDictionary.ConflictCodes[Code];
                else result += " " + (_segrConflict ? CodesDictionary.Segregation[Code] : CodesDictionary.Stowage[Code]);
                
                return result;
            }
        }
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
        public bool ShowExpanded => false;

        // --------------- Public constructors ----------------------------------

        public ConflictPanelItemViewModel(DgWrapper dgUnit, string code, bool segrConflict = false, DgWrapper dgB = null)
        {
            _dgUnit = dgUnit;
            _dgB = dgB;
            Code = code;
            GroupParam = ContainerNumber;
            _segrConflict = segrConflict;
            if (segrConflict)
            {
                IsSegregationConflict = true;
                IsStowageConflict = false;
            }
            else
            {
                IsSegregationConflict = false;
                IsStowageConflict = true;
            }
        }

        /// <summary>
        /// Adds a record to DataMessenger
        /// </summary>
        internal void RegisterInMessenger()
        {
            DataMessenger.Default.Register<ConflictListToBeUpdatedMessage>(this, OnDgWrapperUpdated);
        }

        /// <summary>
        /// Removes record from DataMessenger
        /// </summary>
        internal void UnregisterInMessenger()
        {
            DataMessenger.Default.Unregister(this);
        }

        /// <summary>
        /// Calls OnPropertyChanged for its properties
        /// </summary>
        /// <param name="obj"></param>
        private void OnDgWrapperUpdated(object obj)
        {
            OnPropertyChanged("Text");
            OnPropertyChanged("ContainerNumber");
        }


        // ---------------- Overrided system methods ----------------------------

        public override string ToString()
        {
            string result = ContainerNumber + " in " + Location + " unno " + Unno + (IsSegregationConflict ? (" in conf with" + ConflictingDgNumber) : null);
            return result;
        }


    }
}
