using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.UI.Services;
using EasyJob_ProDG.UI.Services.DataServices;
using EasyJob_ProDG.UI.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace EasyJob_ProDG.UI.View.DialogWindows.Summaries
{
    public class UpdateConditionSummaryViewModel : Observable
    {
        /// <summary>
        /// Flag to avoid re-creation of the summary on each show up
        /// </summary>
        internal static bool ReportCreated {get; set;}

        #region Private members
        ISettingsService _settingsService;

        ICargoUpdatesDataService _cargoUpdatesDataService = new CargoUpdatesDataService();
        private bool _showZeroValues; 


        List<Container> LoadedContainers => _cargoUpdatesDataService.LoadedContainers;
        List<Container> DischargedContainers => _cargoUpdatesDataService.DischargedContainers;
        List<Container> CargoPlanContainers => _cargoUpdatesDataService.WorkingCargoPlan.Model.Containers;

        string PortOfLoading => _cargoUpdatesDataService.WorkingCargoPlan.VoyageInfo.PortOfDeparture;

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
            BlockDischarged.OnShowContainersExectued -= ShowContainers;
            BlockDischarged.OnShowContainersExectued += ShowContainers;


            var dischargedWrongPODTotal = DischargedContainers.Where(c => c.POD != PortOfLoading);

            // Discharged wrong POD
            BlockDischargedWrongPOD = CreateBlock(dischargedWrongPODTotal.Where(c => c.POL != PortOfLoading));

            // Cancelled loading
            BlockCancelled = CreateBlock(dischargedWrongPODTotal.Where(c => c.POL == PortOfLoading));

            // Loaded
            BlockLoaded = CreateBlock(LoadedContainers);

            // Added wrong POL
            BlockLoadedWrongPOL = CreateBlock(LoadedContainers.Where(c => c.POL != PortOfLoading));

            // Restows
            BlockRestows = CreateBlock(CargoPlanContainers.Where(c => c.HasLocationChanged && c.POL != PortOfLoading));
            
            // Changed position
            BlockChangedPosition = CreateBlock(CargoPlanContainers.Where(c => c.HasLocationChanged && c.POL == PortOfLoading));

            // Changed POD
            BlockChangedPOD = CreateBlock(CargoPlanContainers.Where(c => c.HasPodChanged));

            // Transit cargo
            BlockTransit = CreateBlock(CargoPlanContainers.Where(c => !LoadedContainers.Contains(c)));
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
            block.OnShowContainersExectued -= ShowContainers;
            block.OnShowContainersExectued += ShowContainers;

            return block;
        }

        private void ShowContainers(object sender, EventArgs e)
        {
            throw new NotImplementedException();
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
        private void SetValueToAllBlockProperties(bool value, [CallerMemberName]string propertyName = null)
        {
            foreach (var property in typeof(UpdateConditionSummaryViewModel).GetProperties())
            {
                if (property.PropertyType != typeof(UpdateReportBlockCondition)) continue;
                PropertyInfo propertyTop = typeof(UpdateConditionSummaryViewModel).GetProperty(property.Name);
                PropertyInfo propertyDeep = propertyTop.PropertyType.GetProperty(propertyName);
                propertyDeep.SetValue(propertyTop.GetValue(this), value, null);
            }
        }

        #endregion

        #region Show commands



        #endregion

        #region Constructor and singleton instance
        private static UpdateConditionSummaryViewModel _instance;
        public UpdateConditionSummaryViewModel()
        {
            _settingsService = ServicesHandler.GetServicesAccess().SettingsServiceAccess;
        } 
        #endregion
    }
}
