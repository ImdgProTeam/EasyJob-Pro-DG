using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.Model.Cargo;
using System.Collections.Generic;

namespace EasyJob_ProDG.UI.Data
{
    public class VentilationRequirements : Observable
    {
        // ---------------------- Properties --------------------------------------------

        private readonly List<byte> _ventHolds = new List<byte>();

        public string VentHoldsFullText
        {
            get
            {
                string result = "";
                if (_ventHolds.Count > 0)
                {
                    result = "Mechanical ventilation shall be started in: \n";
                    foreach (var hold in _ventHolds)
                    {
                        result += "- hold " + hold + "\n";
                    }
                }

                return result;
            }
        }


        // ---------------------- Methods -----------------------------------------------

        public void Clear()
        {
            _ventHolds.Clear();
            OnPropertyChanged();
        }

        public bool Contains(byte value)
        {
            return _ventHolds.Contains(value);
        }

        public void Add(byte value)
        {
            if (Contains(value)) return;

            _ventHolds.Add(value);
            OnPropertyChanged();
        }

        /// <summary>
        /// Method checks if there are any vent hold requirements in stowage and updates same in the class
        /// </summary>
        public void Check()
        {
            Clear();
            foreach (var hold in Stowage.SWgroups.VentHoldsList)
                Add(hold);
            OnPropertyChanged("VentHoldsFullText");
        }

    }
}
