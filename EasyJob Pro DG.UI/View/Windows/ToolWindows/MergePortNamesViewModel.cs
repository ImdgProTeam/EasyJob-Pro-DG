using EasyJob_ProDG.UI.Services.DataServices;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.Wrapper.Cargo;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.View.DialogWindows
{
    public class MergePortNamesViewModel : Observable
    {
        private ICargoDataService _cargoDataService;

        #region Public properties

        public ICommand ApplyMergePortNamesCommand { get; private set; }

        public List<string> ListOfPorts { get; private set; }

        public string SelectedPort1
        {
            get => selectedPort1;
            set
            {
                selectedPort1 = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(MergePortsText));
            }
        }
        private string selectedPort1;
        public string SelectedPort2
        {
            get => selectedPort2;
            set
            {
                selectedPort2 = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(MergePortsText));
            }
        }
        private string selectedPort2;
        public string SelectedPort3
        {
            get => selectedPort3;
            set
            {
                selectedPort3 = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(MergePortsText));
            }
        }
        private string selectedPort3;

        public string MergePortsText => CombineMergePortsText();
        public string MergeResultingPort { get; set; }

        #endregion

        #region Private methods

        private bool ApplyMergePortNamesCanExecute(object obj)
        {
            return GetSelectedPortsCount() > 0 && !string.IsNullOrWhiteSpace(MergeResultingPort);
        }

        private void ApplyMergePortNamesOnExecuted(object obj)
        {
            foreach (var container in _cargoDataService.WorkingCargoPlan.Containers)
            {
                if (container == null) continue;
                if (!string.IsNullOrWhiteSpace(container.POL) &&
                    (string.Equals(container.POL, selectedPort1)
                    || string.Equals(container.POL, selectedPort2)
                    || string.Equals(container.POL, selectedPort3)))
                {
                    container.SetPOL(MergeResultingPort);
                }
                if (!string.IsNullOrWhiteSpace(container.POD) &&
                    (string.Equals(container.POD, selectedPort1)
                    || string.Equals(container.POD, selectedPort2)
                    || string.Equals(container.POD, selectedPort3)))
                {
                    container.SetPOD(MergeResultingPort);
                }
                if (!string.IsNullOrWhiteSpace(container.FinalDestination) &&
                    (string.Equals(container.FinalDestination, selectedPort1)
                    || string.Equals(container.FinalDestination, selectedPort2)
                    || string.Equals(container.FinalDestination, selectedPort3)))
                {
                    container.SetFinalDestination(MergeResultingPort);
                }
            }

            ResetSelections();
        }

        private string CombineMergePortsText()
        {
            string result;
            int mergePortsCount = GetSelectedPortsCount();

            if (mergePortsCount == 0) return string.Empty;
            if (mergePortsCount == 1)
                result = $"Change port name {selectedPort1 + selectedPort2 + selectedPort3} to:";
            else
            {
                result = $"Merge "
                    + (!string.IsNullOrEmpty(selectedPort1) ? (selectedPort1 + (mergePortsCount == 2 ? " and " : ", ")) : "")
                    + (!string.IsNullOrEmpty(selectedPort2)
                            ? (selectedPort2 + (mergePortsCount == 3 ? " and "
                            : (string.IsNullOrEmpty(selectedPort1) ? " and " : ""))) : "")
                    + (!string.IsNullOrEmpty(selectedPort3) ? selectedPort3 : "")
                    + " to:";
            }

            return result;
        }

        private int GetSelectedPortsCount()
        {
            int mergePortsCount = 0;
            if (!string.IsNullOrWhiteSpace(selectedPort1)) mergePortsCount += 1;
            if (!string.IsNullOrWhiteSpace(selectedPort2)) mergePortsCount += 1;
            if (!string.IsNullOrWhiteSpace(selectedPort3)) mergePortsCount += 1;
            return mergePortsCount;
        }

        private void ResetSelections()
        {
            SelectedPort1 = null;
            SelectedPort2 = null;
            SelectedPort3 = null;
            MergeResultingPort = null;
            OnPropertyChanged(nameof(MergeResultingPort));
        }

        private void CreateListOfPorts()
        {
            ListOfPorts =
            [
                .. _cargoDataService.WorkingCargoPlan.Containers.Select(c => c.POL).Distinct().OrderBy(s => s),
                .. _cargoDataService.WorkingCargoPlan.Containers.Select(c => c.POD).Distinct().OrderBy(s => s),
                .. _cargoDataService.WorkingCargoPlan.Containers.Select(c => c.FinalDestination).Distinct().OrderBy(s => s),
            ];
            ListOfPorts = ListOfPorts.Distinct().OrderBy(p => p).ToList();
        } 

        #endregion

        #region Constructor

        public MergePortNamesViewModel()
        {
            _cargoDataService = Services.ServicesHandler.GetServicesAccess().CargoDataServiceAccess;
            ApplyMergePortNamesCommand = new DelegateCommand(ApplyMergePortNamesOnExecuted, ApplyMergePortNamesCanExecute);
            CreateListOfPorts();

        }

        #endregion
    }
}
