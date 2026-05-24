using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.Utility.Messages;
using EasyJob_ProDG.UI.Wrapper;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.ViewModel
{
    public class DataGridUpdatesViewModel : DataGridViewModelBase
    {
        public Visibility Visible { get; set; }
        ObservableCollection<ContainerWrapper> UpdatedContainers { get; set; }
        public ContainerWrapper SelectedUnit { get; set; }
        public ICommand CloseUpdatesDataGridCommand { get; private set; }


        #region Constructor
        // ---------- Constructor ---------------
        public DataGridUpdatesViewModel() : base()
        {
            UpdatedContainers = new ObservableCollection<ContainerWrapper>();
            Visible = Visibility.Collapsed;
        }

        #endregion

        #region Startup logic

        /// <summary>
        /// Sets data source to View property
        /// </summary>
        protected override void SetDataView()
        {
            SetPlanViewSource(UpdatedContainers);
        }

        /// <summary>
        /// Assigns handler methods for commands
        /// </summary>
        protected override void LoadCommands()
        {
            CloseUpdatesDataGridCommand = new DelegateCommand(CloseUpdatesDataGridCommandOnExecuted);
        }

        /// <summary>
        /// Registers for messages in DataMessenger
        /// </summary>
        protected override void RegisterInDataMessenger()
        {
            DataMessenger.Default.Unregister(this);
            DataMessenger.Default.Register<ShowUpdatesMessage>(this, OnShowUpdatesMessageReceived);
        }

        #endregion

        /// <summary>
        /// Sets UpdatedContainers with respective list of containers
        /// </summary>
        /// <param name="message"></param>
        private void OnShowUpdatesMessageReceived(ShowUpdatesMessage message)
        {
            if (message is null) return;
            string unitType = string.Empty;

            if (!HandleReeferModeChanged(message, ref unitType))
                SetUpdatedContainersAndUnitType(message, ref unitType);

            SetDataView();
            OnPropertyChanged(nameof(UnitsPlanView));

            SetStatusBarOnMessageReceived(message, unitType);

            Visible = Visibility.Visible;
            OnPropertyChanged(nameof(Visible));
        }

        private void SetUpdatedContainersAndUnitType(ShowUpdatesMessage message, ref string unitType)
        {
            switch (message.Units)
            {
                case View.Units.Containers:
                    UpdatedContainers = [.. message.ContainersToShow.Select(c => new ContainerWrapper(c))];
                    unitType = UpdatedContainers.Count > 0 ? "containers" : "container";
                    break;
                case View.Units.Reefers:
                    UpdatedContainers = [.. message.ContainersToShow
                        .Where(c => c.IsRf)
                        .Select(c => new ContainerWrapper(c))];
                    unitType = UpdatedContainers.Count > 0 ? "reefers" : "reefer";
                    break;
                case View.Units.DgContainers:
                    UpdatedContainers = [.. message.ContainersToShow
                        .Where(c => c.ContainsDgCargo)
                        .Select(c => new ContainerWrapper(c))];
                    unitType = UpdatedContainers.Count > 0 ? "dg containers" : "dg container";
                    break;
                default:
                    break;
            }
        }

        private bool HandleReeferModeChanged(ShowUpdatesMessage message, ref string unitType)
        {
            if (!string.Equals(message.DisplayText, "Operating mode changed")) return false;

            switch (message.Units)
            {
                case View.Units.Containers:
                    UpdatedContainers = [.. message.ContainersToShow.Where(c => !c.IsRf).Select(c => new ContainerWrapper(c))];
                    unitType = UpdatedContainers.Count > 0 ? "inactive reefers" : "inactive reefer";
                    break;
                case View.Units.Reefers:
                    UpdatedContainers = [.. message.ContainersToShow
                        .Where(c => c.IsRf)
                        .Select(c => new ContainerWrapper(c))];
                    unitType = UpdatedContainers.Count > 0 ? "live reefers" : "live reefer";
                    break;
            }
            return true;
        }

        private void SetStatusBarOnMessageReceived(ShowUpdatesMessage message, string unitType)
        {
            SelectedUnit = null;
            StatusBarText = $"Updates: {message.DisplayText}: {UpdatedContainers.Count} {unitType}";
            OnPropertyChanged(nameof(StatusBarText));
        }

        private void CloseUpdatesDataGridCommandOnExecuted(object obj)
        {
            HideUpdatesDataGrid();
            DataMessenger.Default.Send(new ChangeSelectionMessage(), "updates data grid closed");
        }

        internal void HideUpdatesDataGrid()
        {
            Visible = Visibility.Collapsed;
            OnPropertyChanged(nameof(Visible));
        }

        #region Implementation of abstract class methods

        protected override void OnAddNewUnit(object obj)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        #region Commands


        #endregion
    }
}
