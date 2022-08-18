using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.Model.Transport;
using EasyJob_ProDG.UI.Services.DialogServices;
using EasyJob_ProDG.UI.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.View.DialogWindows
{
    public partial class CargoReportViewModel : Observable, IDialogWindowRequestClose
    {
        #region Private fields

        private readonly CollectionViewSource cargoValuesView = new CollectionViewSource();
        private List<PortValues> CargoValues { get; set; }
        private List<PortValues> fullCargoValues;
        private Voyage voyage;
        private CargoPlan plan;
        private bool isPortOfDepartureSet = false;
        private bool isPortOfDestinationSet = false; 

        #endregion

        #region Public properties

        /// <summary>
        /// Used for DataGrid binding 
        /// </summary>
        public System.ComponentModel.ICollectionView CargoValuesView => cargoValuesView?.View;

        /// <summary>
        /// Window title
        /// </summary>
        public string Title => "Cargo report";

        /// <summary>
        /// Defines if to display loading (true) or discharging (false) values.
        /// </summary>
        public bool IsLoading { get; set; } = true;

        /// <summary>
        /// Text to be displayed on loading/discharging switch button.
        /// </summary>
        public string IsLoadingButtonTitle { get { return IsLoading ? "Loading" : "Discharging"; } }

        #endregion

        #region Constructors
        //----------- Constructors -------------

        public CargoReportViewModel(Voyage voyage)
        {
            this.voyage = voyage;

            //Set commands
            SwitchLoadingCommand = new DelegateCommand(SwitchLoadingOnExecuted);
            ExportToExcelCommand = new DelegateCommand(ExportToExcelOnExecuted);

            //Set Filter event
            cargoValuesView.Filter += OnCargoValuesFiltered;
        }

        private void ExportToExcelOnExecuted(object obj)
        {
            throw new NotImplementedException();
        }

        #endregion



        #region Public Total Properties and methods

        public int TotalContainers { get; private set; }
        public int TotalRf { get; private set; }
        public int TotalDgContainers { get; private set; }
        public decimal TotalDgNetWt { get; private set; }
        public decimal TotalMPWeight { get; private set; }

        /// <summary>
        /// Sets correct totals display values.
        /// </summary>
        private void SetTotals()
        {
            ResetTotals();
            CountTotals();
            RefreshTotals();
        }

        /// <summary>
        /// Counts the totals values from the <see cref="CargoValuesView"/>
        /// </summary>
        private void CountTotals()
        {
            if (CargoValuesView == null) return;

            foreach (PortValues port in CargoValuesView)
            {
                TotalContainers += port.Containers;
                TotalRf += port.Rf;
                TotalDgContainers += port.DgContainers;
                TotalDgNetWt += port.DgNetWt;
                TotalMPWeight += port.MP;
            }
        }

        /// <summary>
        /// Resets all totals to 0
        /// </summary>
        private void ResetTotals()
        {
            TotalContainers = 0;
            TotalRf = 0;
            TotalDgContainers = 0;
            TotalDgNetWt = 0;
            TotalMPWeight = 0;
        }

        /// <summary>
        /// Calls OnPropertyChanged for each of totals properties
        /// </summary>
        private void RefreshTotals()
        {
            OnPropertyChanged(nameof(TotalContainers));
            OnPropertyChanged(nameof(TotalDgContainers));
            OnPropertyChanged(nameof(TotalRf));
            OnPropertyChanged(nameof(TotalDgNetWt));
            OnPropertyChanged(nameof(TotalMPWeight));
        }

        #endregion


        #region Public methods

        /// <summary>
        /// Creates report from CargoPlan.
        /// </summary>
        /// <param name="cargoPlan">CargoPlan.</param>
        public void CreateReport(CargoPlan cargoPlan)
        {
            plan = cargoPlan;

            CountReportValues(cargoPlan);
            SetDisplayValues();
            SaveFullCargoReport();
            SetDataView();
            SetTotals();
        }

        #endregion


        #region Filter Logic
        // ----------- Filter logic ----------------

        /// <summary>
        /// Options to be displayed to select port filter option
        /// </summary>
        public IEnumerable<string> PortOptions
        {
            get => new List<string>()
            {   "All ports",
                $"Port of departure: {voyage.PortOfDeparture}",
                $"Port of destination: {voyage.PortOfDestination}",
                "Only loading ports",
                "Only discharging ports",
                "Only selected" };
        }
        private List<string> portOptions;


        /// <summary>
        /// Selected Port option index.
        /// By default is 0.
        /// </summary>
        public int SelectedPortOptionIndex
        {
            get
            {
                return selectedPortOptionIndex;
            }
            set
            {
                selectedPortOptionIndex = value;
                SetCargoValueFilteredByPort(selectedPortOptionIndex);
                CargoValuesView?.Refresh();
                SetTotals();
            }
        }
        private int selectedPortOptionIndex = 0;

        public event EventHandler<DialogWindowCloseRequestedEventArgs> CloseRequested;

        /// <summary>
        /// Changes CargoValues in accordance with selected Port of departure or destination.
        /// If not selected - restores full values.
        /// </summary>
        /// <param name="selectedPortOptionIndex"></param>
        private void SetCargoValueFilteredByPort(int selectedPortOptionIndex)
        {
            switch (selectedPortOptionIndex)
            {
                //Port of departure
                case 1:
                    if (!isPortOfDepartureSet)
                        CreateReportWithSpecifiedPortOfDeparture(voyage.PortOfDeparture);
                    break;
                case 2:
                    if (!isPortOfDestinationSet)
                        CreateReportWithSpecifiedPortOfDestination(voyage.PortOfDestination);
                    break;

                //all other cases - restore full view
                default:
                    if (isPortOfDepartureSet || isPortOfDestinationSet) RestoreFullView();
                    break;
            }
        }


        /// <summary>
        /// Implements logic to filter content
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCargoValuesFiltered(object sender, FilterEventArgs e)
        {
            // Checks section

            if (!(e.Item is PortValues p) || p.Port is null)
            {
                e.Accepted = false;
                return;
            }

            //Logic section

            switch (selectedPortOptionIndex)
            {
                //Port of departure
                case 1:
                    if (IsLoading && p.Port != voyage.PortOfDeparture)
                        break;
                    if (!IsLoading && p.Port == voyage.PortOfDeparture)
                        break;
                    return;

                //Port of destination
                case 2:
                    if (IsLoading && p.Port == voyage.PortOfDestination)
                        break;
                    if (!IsLoading && p.Port != voyage.PortOfDestination)
                        break;
                    return;

                //Only loading ports
                case 3:
                    if (p.LoadingContainers > 0) return;
                    break;

                //Only discharging ports
                case 4:
                    if (p.DischargingContainers > 0) return;
                    break;

                //Only selected ports
                case 5:
                    if (p.IsSelected) return;
                    break;

                //no filter / all ports
                default:
                case 0:
                    return;
            }

            e.Accepted = false;
        }

        #endregion


        #region Private methods
        //----------- Private methods ----------

        /// <summary>
        /// Counts report values from CargoPlan.
        /// </summary>
        /// <param name="cargoPlan">CargoPlan.</param>
        private void CountReportValues(CargoPlan cargoPlan)
        {
            if (cargoPlan == null) return;

            CargoValues = new();

            foreach (var container in cargoPlan.Containers)
            {
                AddContainer(container);
            }

            foreach (var dg in cargoPlan.DgList)
            {
                AddDg(dg);
            }
        }

        /// <summary>
        /// Creates Report for units loaded in specified portOfDeparture only
        /// </summary>
        /// <param name="portOfDeparture"></param>
        private void CreateReportWithSpecifiedPortOfDeparture(string portOfDeparture)
        {
            isPortOfDepartureSet = true;

            CountReportValuesForSpecifiedPortOfDeparture(portOfDeparture);
            SetDisplayValues();

            isPortOfDestinationSet = false;
        }

        /// <summary>
        /// Counts values of units loaded in portOfDeparture only from plan and updates CargoValues.
        /// </summary>
        /// <param name="portOfDeparture">Specified port of departure.</param>
        private void CountReportValuesForSpecifiedPortOfDeparture(string portOfDeparture)
        {
            if (plan == null || string.IsNullOrEmpty(portOfDeparture)) return;

            if (CargoValues.Count > 0) CargoValues.Clear();
            foreach (var container in plan.Containers)
            {
                if (!string.Equals(container.POL, portOfDeparture)) continue;

                AddContainer(container);
            }

            foreach (var dg in plan.DgList)
            {
                if (!string.Equals(dg.POL, portOfDeparture)) continue;
                AddDg(dg);
            }
        }


        /// <summary>
        /// Creates Report for units discharging in specified portOfDestination only
        /// </summary>
        /// <param name="portOfDestination"></param>
        private void CreateReportWithSpecifiedPortOfDestination(string portOfDestination)
        {
            isPortOfDestinationSet = true;

            CountReportValuesForSpecifiedPortOfDestination(portOfDestination);
            SetDisplayValues();

            isPortOfDepartureSet = false;
        }

        /// <summary>
        /// Counts values of units discharging in portOfDestination only from plan and updates CargoValues.
        /// </summary>
        /// <param name="portOfDestination">Specified port of destination.</param>
        private void CountReportValuesForSpecifiedPortOfDestination(string portOfDestination)
        {
            if (plan == null || string.IsNullOrEmpty(portOfDestination)) return;

            if (CargoValues.Count > 0) CargoValues.Clear();
            foreach (var container in plan.Containers)
            {
                if (!string.Equals(container.POD, portOfDestination)) continue;

                AddContainer(container);
            }

            foreach (var dg in plan.DgList)
            {
                if (!string.Equals(dg.POD, portOfDestination)) continue;
                AddDg(dg);
            }
        }

        /// <summary>
        /// Adds Dg values to respective ports (POL and POD).
        /// </summary>
        /// <param name="dg"></param>
        private void AddDg(Dg dg)
        {
            var port = CargoValues.FirstOrDefault(x => string.Equals(x.Port, dg.POL));

            if (port == null)
            {
                CargoValues.Add(port = new PortValues(dg.POL));
            }
            port.AddLoadingPort(dg);

            port = CargoValues.FirstOrDefault(x => string.Equals(x.Port, dg.POD));

            if (port == null)
            {
                CargoValues.Add(port = new PortValues(dg.POD));
            }
            port.AddDischargingPort(dg);
        }

        /// <summary>
        /// Adds Container counts ro respective ports (POL and POD).
        /// </summary>
        /// <param name="container"></param>
        private void AddContainer(Container container)
        {
            var port = CargoValues.FirstOrDefault(x => string.Equals(x.Port, container.POL));

            if (port == null)
            {
                CargoValues.Add(port = new PortValues(container.POL));
            }
            port.AddLoadingPort(container);

            port = CargoValues.FirstOrDefault(x => string.Equals(x.Port, container.POD));

            if (port == null)
            {
                CargoValues.Add(port = new PortValues(container.POD));
            }
            port.AddDischargingPort(container);
        }

        /// <summary>
        /// Sets display values in accordance with selected IsLoading (loading or discharging).
        /// </summary>
        private void SetDisplayValues()
        {
            foreach (var port in CargoValues)
            {
                port.SetLoadingOrDischargingValues(IsLoading);
            }
            if (isPortOfDepartureSet || isPortOfDestinationSet)
                CargoValuesView?.Refresh();
            SetTotals();
        }

        /// <summary>
        /// Sets data source to View property
        /// </summary>
        private void SetDataView()
        {
            cargoValuesView.Source = CargoValues;
        }

        /// <summary>
        /// Switches between loading and discharging values
        /// </summary>
        /// <param name="obj"></param>
        private void SwitchLoadingOnExecuted(object obj)
        {
            IsLoading = !IsLoading;
            OnPropertyChanged(nameof(IsLoadingButtonTitle));
            SetDisplayValues();
        }

        /// <summary>
        /// Saves full CargoValues in a field.
        /// </summary>
        private void SaveFullCargoReport()
        {
            fullCargoValues = CargoValues.ToList();
        }

        /// <summary>
        /// Restores modified (filtered by POL/POD) CargoValues to initial full values
        /// </summary>
        private void RestoreFullView()
        {
            if(CargoValues.Count > 0) CargoValues.Clear();

            foreach(var item in fullCargoValues)
            {
                CargoValues.Add(item);
            }
            SetDisplayValues();

            isPortOfDepartureSet = false;
            isPortOfDestinationSet = false;
        }

        #endregion

        public ICommand SwitchLoadingCommand { get; private set; }
        public ICommand ExportToExcelCommand { get; private set; }
    }
}
