using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.UI.Services;
using EasyJob_ProDG.UI.Services.DataServices;
using EasyJob_ProDG.UI.Services.DialogServices;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.Utility.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using static EasyJob_ProDG.UI.View.DialogWindows.Summaries.UpdateReportBlockCondition;

namespace EasyJob_ProDG.UI.View.DialogWindows.Summaries
{
    public class UpdateConditionSummaryViewModel : Observable, IDialogWindowRequestClose
    {
        /// <summary>
        /// Flag to avoid re-creation of the summary on each show up
        /// </summary>
        internal static bool ReportCreated { get; set; }
        public ICommand CloseCommand { get; private set; }

        #region Private members

        ISettingsService _settingsService;

        ICargoUpdatesDataService _cargoUpdatesDataService = CargoUpdatesDataService.GetCargoUpdatesDataService();
        private bool _showZeroValues;


        List<Container> LoadedContainers => _cargoUpdatesDataService.LoadedContainers;
        List<Container> DischargedContainers => _cargoUpdatesDataService.DischargedContainers;
        List<Container> CargoPlanContainers => _cargoUpdatesDataService.WorkingCargoPlan.Model.Containers;

        string PortOfLoading => _cargoUpdatesDataService.WorkingCargoPlan.VoyageInfo.PortOfDeparture;

        List<Container> dischargedWrongPODContainers;
        List<Container> cancelledContainers;
        List<Container> loadedWronPOLContainers;
        List<Container> restowedContainers;
        List<Container> changedPositionContainers;
        List<Container> changedPODContainers;
        List<Container> transitContainers;

        #endregion

        #region Public properties

        public bool HasUpdates => _cargoUpdatesDataService.HasUpdates;
        public string Title => "Update condition summary";

        /// <summary>
        /// Flags if to display UpdateConditionSummary on each Condition update.
        /// Linked to User settings.
        /// Bound to CheckBox.
        /// </summary>
        public bool ShowSummaryOnUpdateCondition
        {
            get => _settingsService.ShowSummaryOnUpdateCondition;
            set => _settingsService.ShowSummaryOnUpdateCondition = value;
        }

        /// <summary>
        /// Indicates if to show values with '0' value.
        /// Bound to CheckBox
        /// </summary>
        public bool ShowZeroValues
        {
            get => _showZeroValues;
            set
            {
                SetValueToAllBlockProperties(value);
                _showZeroValues = value;
            }
        }


        // Report blocks
        public UpdateReportBlockCondition BlockDischarged { get; private set; }
        public UpdateReportBlockCondition BlockDischargedWrongPOD { get; private set; }
        public UpdateReportBlockCondition BlockCancelled { get; private set; }
        public UpdateReportBlockCondition BlockLoaded { get; private set; }
        public UpdateReportBlockCondition BlockLoadedWrongPOL { get; private set; }
        public UpdateReportBlockCondition BlockRestows { get; private set; }
        public UpdateReportBlockCondition BlockChangedPosition { get; private set; }
        public UpdateReportBlockCondition BlockChangedPOD { get; private set; }
        public UpdateReportBlockCondition BlockTransit { get; private set; }

        public string UpdatedForText { get; private set; }

        #endregion

        /// <summary>
        /// Creates and returns Summary report, if not yet created.
        /// </summary>
        /// <returns></returns>
        public static UpdateConditionSummaryViewModel CreateReport()
        {
            _instance ??= new UpdateConditionSummaryViewModel();
            if (ReportCreated) return _instance;
            _instance._showZeroValues = false;
            if (!_instance.HasUpdates) return _instance.CreateBlankBlocks();

            _instance.CreateReportBlocks();

            ReportCreated = true;
            return _instance;
        }

        #region Private methods

        /// <summary>
        /// Carries out all calculation of values for the report.
        /// </summary>
        private void CreateReportBlocks()
        {
            UpdatedForText = $"Updated for {PortOfLoading}";

            // Discharged
            BlockDischarged = CreateBlock(DischargedContainers);

            var dischargedWrongPODTotal = DischargedContainers.Where(c => c.POD != PortOfLoading);

            // Discharged wrong POD
            dischargedWrongPODContainers = dischargedWrongPODTotal.Where(c => c.POL != PortOfLoading).ToList();
            BlockDischargedWrongPOD = CreateBlock(dischargedWrongPODContainers);

            // Cancelled loading
            cancelledContainers = dischargedWrongPODTotal.Where(c => c.POL == PortOfLoading).ToList();
            BlockCancelled = CreateBlock(cancelledContainers);

            // Loaded
            BlockLoaded = CreateBlock(LoadedContainers);

            // Added wrong POL
            loadedWronPOLContainers = LoadedContainers.Where(c => c.POL != PortOfLoading).ToList();
            BlockLoadedWrongPOL = CreateBlock(loadedWronPOLContainers);

            // Restows
            restowedContainers = CargoPlanContainers.Where(c => c.HasLocationChanged && c.POL != PortOfLoading).ToList();
            BlockRestows = CreateBlock(restowedContainers);

            // Changed position
            changedPositionContainers = CargoPlanContainers.Where(c => c.HasLocationChanged && c.POL == PortOfLoading).ToList();
            BlockChangedPosition = CreateBlock(changedPositionContainers);

            // Changed POD
            changedPODContainers = CargoPlanContainers.Where(c => c.HasPodChanged).ToList();
            BlockChangedPOD = CreateBlock(changedPODContainers);

            // Transit cargo
            transitContainers = CargoPlanContainers.Where(c => !LoadedContainers.Contains(c)).ToList();
            BlockTransit = CreateBlock(transitContainers);

            SubscribeBlocksToEvents();
        }

        /// <summary>
        /// Subscribes each block OnShowContainersExecuted event to respective respective
        /// </summary>
        private void SubscribeBlocksToEvents()
        {
            UnsubscribeEvents();
            BlockDischarged.OnShowContainersExectued += ShowDischargedContainers;
            BlockDischargedWrongPOD.OnShowContainersExectued += ShowDischargedWrongContainers;
            BlockCancelled.OnShowContainersExectued += ShowCancelledContainers;
            BlockLoaded.OnShowContainersExectued += ShowLoadedContainers;
            BlockLoadedWrongPOL.OnShowContainersExectued += ShowLoadedWrongContainers;
            BlockRestows.OnShowContainersExectued += ShowRestowedContainers;
            BlockChangedPosition.OnShowContainersExectued += ShowChangedPositionContainers;
            BlockChangedPOD.OnShowContainersExectued += ShowChangedPODContainers;
            BlockTransit.OnShowContainersExectued += ShowTransitContainers;
        }


        /// <summary>
        /// Creates <see cref="UpdateReportBlockCondition"/> with values from the listOfContainers.
        /// </summary>
        /// <param name="block">Block to be created/updated</param>
        /// <param name="listOfContainers">List of containers to be counted</param>
        private UpdateReportBlockCondition CreateBlock(IEnumerable<Container> listOfContainers)
        {
            int containersCount = listOfContainers.Count();
            int reefersCount = listOfContainers.Count(c => c.IsRf);
            int dgCount = listOfContainers.Count(c => c.ContainsDgCargo);
            var block = new UpdateReportBlockCondition(containersCount, reefersCount, dgCount);

            return block;
        }

        /// <summary>
        /// Sets all 'Block' properties (of type <see cref="UpdateReportBlockCondition"/>) to be with all '0'.
        /// </summary>
        /// <returns><see cref="UpdateConditionSummaryViewModel"/> with all '0' in 'Block' properties.</returns>
        private UpdateConditionSummaryViewModel CreateBlankBlocks()
        {
            foreach (var property in typeof(UpdateConditionSummaryViewModel).GetProperties())
            {
                if (property.PropertyType != typeof(UpdateReportBlockCondition)) continue;
                property.SetValue(this, new UpdateReportBlockCondition(0, 0, 0));
            }
            return this;
        }

        /// <summary>
        /// Sets value to propertyName of all 'Block' properties.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        private void SetValueToAllBlockProperties(bool value, [CallerMemberName] string propertyName = null)
        {
            foreach (var property in typeof(UpdateConditionSummaryViewModel).GetProperties())
            {
                if (property.PropertyType != typeof(UpdateReportBlockCondition)) continue;
                PropertyInfo propertyTop = typeof(UpdateConditionSummaryViewModel).GetProperty(property.Name);
                PropertyInfo propertyDeep = propertyTop.PropertyType.GetProperty(propertyName);
                propertyDeep.SetValue(propertyTop.GetValue(this), value, null);
            }
        }

        /// <summary>
        /// Unsubscribes all Blocks from subscribed events
        /// </summary>
        private void UnsubscribeEvents()
        {
            BlockDischarged.OnShowContainersExectued -= ShowDischargedContainers;
            BlockDischargedWrongPOD.OnShowContainersExectued -= ShowDischargedWrongContainers;
            BlockCancelled.OnShowContainersExectued -= ShowCancelledContainers;
            BlockLoaded.OnShowContainersExectued -= ShowLoadedContainers;
            BlockLoadedWrongPOL.OnShowContainersExectued -= ShowLoadedWrongContainers;
            BlockRestows.OnShowContainersExectued -= ShowRestowedContainers;
            BlockChangedPosition.OnShowContainersExectued -= ShowChangedPositionContainers;
            BlockChangedPOD.OnShowContainersExectued -= ShowChangedPODContainers;
            BlockTransit.OnShowContainersExectued -= ShowTransitContainers;
        }

        private void CloseCommandOnExecuted(object obj)
        {
            CloseRequested?.Invoke(this, new DialogWindowCloseRequestedEventArgs(true));
        }

        #endregion

        #region Show containers methods

        /// <summary>
        /// Sends a message containing list and type of containers to display.
        /// Closes dialog window.
        /// </summary>
        /// <param name="containers"></param>
        /// <param name="e"></param>
        private void SendMessageAndCloseWindow(List<Container> containers, EventArgs e)
        {
            var args = e as ShowContainersEventArgs;
            if (args is null) return;

            DataMessenger.Default.Send(new ShowUpdatesMessage(containers, args.UnitsToShow));
            CloseRequested?.Invoke(this, new DialogWindowCloseRequestedEventArgs(true));
        }

        private void ShowDischargedContainers(object sender, EventArgs e)
        {
            SendMessageAndCloseWindow(DischargedContainers, e);
        }
        private void ShowTransitContainers(object sender, EventArgs e)
        {
            SendMessageAndCloseWindow(transitContainers, e);
        }
        private void ShowChangedPODContainers(object sender, EventArgs e)
        {
            SendMessageAndCloseWindow(changedPODContainers, e);
        }
        private void ShowChangedPositionContainers(object sender, EventArgs e)
        {
            SendMessageAndCloseWindow(changedPositionContainers, e);
        }
        private void ShowRestowedContainers(object sender, EventArgs e)
        {
            SendMessageAndCloseWindow(restowedContainers, e);
        }
        private void ShowLoadedWrongContainers(object sender, EventArgs e)
        {
            SendMessageAndCloseWindow(loadedWronPOLContainers, e);
        }
        private void ShowLoadedContainers(object sender, EventArgs e)
        {
            SendMessageAndCloseWindow(LoadedContainers, e);
        }
        private void ShowCancelledContainers(object sender, EventArgs e)
        {
            SendMessageAndCloseWindow(cancelledContainers, e);
        }
        private void ShowDischargedWrongContainers(object sender, EventArgs e)
        {
            SendMessageAndCloseWindow(dischargedWrongPODContainers, e);
        }

        #endregion


        #region IDialogWindowRequestClose

        public event EventHandler<DialogWindowCloseRequestedEventArgs> CloseRequested;

        #endregion

        #region Constructor, destructor and singleton instance

        private static UpdateConditionSummaryViewModel _instance;


        public UpdateConditionSummaryViewModel()
        {
            _settingsService = ServicesHandler.GetServicesAccess().SettingsServiceAccess;
            CloseCommand = new DelegateCommand(CloseCommandOnExecuted);
        }

        ~UpdateConditionSummaryViewModel()
        {
            UnsubscribeEvents();
        }

        #endregion
    }
}
