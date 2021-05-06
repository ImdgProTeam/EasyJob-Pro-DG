using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EasyJob_Pro_DG
{
    public class VentilationRequirements : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private static ObservableCollection<byte> ventHolds = new ObservableCollection<byte>();
        

        public string VentHolds
        {
            get
            {
                string result = "";
                if (ventHolds.Count > 0)
                {
                    result = "Mechanical ventilation shall be started in \n";
                    foreach (var hold in ventHolds)
                    {
                        result += "hold " + hold + "\n";
                    }
                }

                return result;
            }
        }

        public void Clear()
        {
            ventHolds.Clear();
            OnPropertyChanged(new PropertyChangedEventArgs("VentHolds"));
        }

        public bool Contains(byte value)
        {
            return ventHolds.Contains(value);
        }

        public void Add(byte value)
        {
            ventHolds.Add(value);
            OnPropertyChanged(new PropertyChangedEventArgs("VentHolds"));
        }

        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
    }
}
