using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.UI.Utility;

namespace EasyJob_ProDG.UI.View.DialogWindows
{
    /// <summary>
    /// PortValues accommodate loading and discharging values for a single port.
    /// </summary>
    public class PortValues : Observable
    {
        public string Port { get; set; }
        public bool IsSelected { get; set; }
        public bool IsSelectedSortable => !IsSelected;


        #region Public properties separated with Loading and Discharging
        public int LoadingContainers { get; set; }
        public int LoadingDgContainers { get; set; }
        public int LoadingRf { get; set; }
        public decimal LoadingNetWt { get; set; }
        public decimal LoadingMP { get; set; }
        public int DischargingContainers { get; set; }
        public int DischargingDgContainers { get; set; }
        public int DischargingRf { get; set; }
        public decimal DischargingNetWt { get; set; }
        public decimal DischargingMP { get; set; }
        #endregion


        #region Public properties for DisplayCargoValues

        // Values used for DisplayCargoValues
        public int Containers { get; set; }
        public int Rf { get; set; }
        public int DgContainers { get; set; }
        public decimal DgNetWt { get; set; }
        public decimal MP { get; set; }

        #endregion


        #region Constructors
        public PortValues(string portCode)
        {
            Port = portCode;
        }

        public PortValues(string portCode, CargoPlan cargoPlan)
        {
            Port = portCode;

            foreach (var container in cargoPlan.Containers)
            {
                if (string.Equals(container.POL, portCode))
                    AddLoadingPort(container);
                if (string.Equals(container.POD, portCode))
                    AddDischargingPort(container);
            }
        }

        #endregion


        #region Methods used in creation of PortValues (Add)
        internal void AddDischargingPort(Dg dg)
        {
            if (dg == null) return;
            DischargingNetWt += dg.DgNetWeight;
            DischargingMP += dg.IsMp ? dg.DgNetWeight : 0;
        }

        internal void AddDischargingPort(Container container)
        {
            if (container == null) return;
            DischargingContainers++;
            if (container.IsRf)
                DischargingRf++;
            if (container.DgCountInContainer > 0)
                DischargingDgContainers++;
        }

        internal void AddLoadingPort(Dg dg)
        {
            if (dg == null) return;
            LoadingNetWt += dg.DgNetWeight;
            LoadingMP += dg.IsMp ? dg.DgNetWeight : 0;
        }
        internal void AddLoadingPort(Container container)
        {
            if (container == null) return;
            LoadingContainers++;
            if (container.IsRf)
                LoadingRf++;
            if (container.DgCountInContainer > 0)
                LoadingDgContainers++;
        }
        #endregion


        public void SetLoadingOrDischargingValues(bool isLoading)
        {
            if (isLoading)
            {
                Containers = LoadingContainers;
                DgContainers = LoadingDgContainers;
                Rf = LoadingRf;
                DgNetWt = LoadingNetWt;
                MP = LoadingMP;
            }
            else
            {
                Containers = DischargingContainers;
                DgContainers = DischargingDgContainers;
                Rf = DischargingRf;
                DgNetWt = DischargingNetWt;
                MP = DischargingMP;
            }
            RefreshDisplayValues();
        }


        private void RefreshDisplayValues()
        {
            OnPropertyChanged(nameof(Containers));
            OnPropertyChanged(nameof(DgContainers));
            OnPropertyChanged(nameof(Rf));
            OnPropertyChanged(nameof(DgNetWt));
            OnPropertyChanged(nameof(MP));
        }
    }
}
