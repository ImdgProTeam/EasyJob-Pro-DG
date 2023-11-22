using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.UI.View.DialogWindows.Summaries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Data;

namespace EasyJob_ProDG.UI.View.DialogWindows
{
    public class PortToPortReportViewModel
    {
        #region Private fields and constants

        internal const string _TOTAL = "Total";
        internal const string _POL = "POL";

        private List<ReportDisplayPort> displayPorts;
        private readonly CollectionViewSource displayValuesView = new CollectionViewSource();
        private ReportDisplayPort totalPorts;
        private List<string> portOfDischarging;
        private DataTable TableData;

        #endregion

        #region Public properties

        /// <summary>
        /// The property which will be bound to DataGrid.
        /// </summary>
        public System.ComponentModel.ICollectionView CollectionView => displayValuesView?.View;

        /// <summary>
        /// Contains the list of ReportDisplayValues.
        /// </summary>
        public List<ReportDisplayPort> DisplayValues { get; set; }

        /// <summary>
        /// Window title
        /// </summary>
        public string Title => "Port to port report";

        #endregion


        #region Constructors
        //----------- Constructors -------------

        /// <summary>
        /// Default constructor
        /// </summary>
        public PortToPortReportViewModel()
        {
        }

        #endregion


        #region Public methods

        /// <summary>
        /// Creates report from WorkingCargoPlan.
        /// </summary>
        /// <param name="cargoPlan">WorkingCargoPlan.</param>
        public void CreateReport(CargoPlan cargoPlan)
        {
            displayPorts = new List<ReportDisplayPort>();
            totalPorts = new ReportDisplayPort(_TOTAL);
            portOfDischarging = new();

            CountReportValues(cargoPlan);
            SetDisplayValuesBasedOnSelectedOption((byte)ReportOptions.Containers);

            SetTableData();
            SetDataView();
        }

        #endregion


        #region DataTable build up

        /// <summary>
        /// Fills DataTable with generated data in SinglePortReportValue types.
        /// </summary>
        private void SetTableData()
        {
            TableData = new();

            //default columns
            TableData.AddColumnWithStringValues(_POL, 0);
            TableData.AddColumn<SinglePortReportValue>(_TOTAL, 1);

            foreach (var port in portOfDischarging)
            {
                TableData.AddColumn<SinglePortReportValue>(port);
            }

            //filling the table
            foreach (var port in displayPorts)
            {
                //individual rows
                List<SinglePortReportValue> values = new List<SinglePortReportValue>();
                foreach (DataColumn header in TableData.Columns)
                {
                    //POL column
                    if (string.Equals(header.ColumnName, _POL))
                    {
                        values.Add(new SinglePortReportValue(_POL) { DisplayValue = port.Port }); ;
                        continue;
                    }

                    //other columns
                    var nothingFound = true;
                    foreach (var singlePort in port.DisplayValues)
                    {
                        if (string.Equals(singlePort.Port, header.ColumnName))
                        {
                            values.Add(singlePort);
                            nothingFound = false;
                            break;
                        }
                    }
                    if (nothingFound)
                    {
                        var newBlankPort = new SinglePortReportValue(header.ColumnName);
                        port.AddPort(newBlankPort);
                        values.Add(newBlankPort);
                    }
                }

                TableData.AddRow(values.ToArray());
            }
        }

        #endregion


        #region Display option Logic

        // ----- Display option logic ----------------

        /// <summary>
        /// Report options to be displayed in filter combobox
        /// </summary>
        public IEnumerable<string> DisplayReportOptions
        {
            get => new List<string>()
            {
                "Containers count",
                "Contianiers total (20/40)",
                "20/40",
                "On deck / below deck",
                "Containers Loaded",
                "Containers Empty",
                "Containers Loaded / Empty",
                "Reefers",
                "Cargo Weight",
                "Containers count and weight",
                "Dg containers count",
                "Dg net weight",
                "Dg and MP net weight",
                "Dg containers / net weight",
                "Dg containers / Dg and MP Wt"
            };
        }

        /// <summary>
        /// Report options enumerator
        /// </summary>
        public enum ReportOptions : byte
        {
            Containers = 0,
            ContainersTotal20And40,
            Containers20And40,
            ContainersOnUnderDeck,
            ContainersLoaded,
            ContainersEmpty,
            ContainersLoadedAndEmpty,
            Reefers,
            Weight,
            ContainersAndWeight,
            DgCount,
            DgNetWeight,
            DgNetWeightAndMP,
            DgCountAndDgNetWeight,
            DgCountAndDgNetWeightAndMP,
        }


        /// <summary>
        /// Selected Port option index.
        /// By default is 0.
        /// </summary>
        public int SelectedDisplayOptionIndex
        {
            get
            {
                return selectedDisplayOptionIndex;
            }
            set
            {
                selectedDisplayOptionIndex = value;
                SetDisplayValuesBasedOnSelectedOption((byte)selectedDisplayOptionIndex);
                CollectionView?.Refresh();
            }
        }
        private int selectedDisplayOptionIndex = 0;

        /// <summary>
        /// Changes CargoValues in accordance with selected Port of departure or destination.
        /// If not selected - restores full values.
        /// </summary>
        /// <param name="selectedPortOptionIndex"></param>
        private void SetDisplayValuesBasedOnSelectedOption(byte selectedDisplayOptionIndex = 0)
        {
            bool isComplexOption = false;
            string propertyName;
            switch (selectedDisplayOptionIndex)
            {
                case (byte)ReportOptions.ContainersLoaded:
                    propertyName = nameof(SinglePortReportValue.ContainersLoadedCount);
                    break;
                case (byte)ReportOptions.ContainersEmpty:
                    propertyName = nameof(SinglePortReportValue.ContainersEmptyCount);
                    break;
                case (byte)ReportOptions.Reefers:
                    propertyName = nameof(SinglePortReportValue.ReefersCount);
                    break;
                case (byte)ReportOptions.Weight:
                    propertyName = nameof(SinglePortReportValue.ContainersWeight);
                    break;
                case (byte)ReportOptions.DgCount:
                    propertyName = nameof(SinglePortReportValue.DgContainersCount);
                    break;
                case (byte)ReportOptions.DgNetWeight:
                    propertyName = nameof(SinglePortReportValue.DgNetWeight);
                    break;
                case (byte)ReportOptions.ContainersTotal20And40:
                case (byte)ReportOptions.Containers20And40:
                case (byte)ReportOptions.ContainersOnUnderDeck:
                case (byte)ReportOptions.ContainersLoadedAndEmpty:
                case (byte)ReportOptions.ContainersAndWeight:
                case (byte)ReportOptions.DgNetWeightAndMP:
                case (byte)ReportOptions.DgCountAndDgNetWeight:
                case (byte)ReportOptions.DgCountAndDgNetWeightAndMP:
                    propertyName = string.Empty;
                    isComplexOption = true;
                    break;
                case (byte)ReportOptions.Containers:
                default:
                    propertyName = nameof(SinglePortReportValue.ContainersCount);
                    break;

            }

            foreach (var port in displayPorts)
            {
                if (isComplexOption)
                {
                    port.SetDisplayValues((ReportOptions)selectedDisplayOptionIndex);
                }
                else
                {
                    port.SetDisplayValues(propertyName);
                }
            }

            CollectionView?.Refresh();
        }

        #endregion


        #region Private methods
        //----------- Private methods ----------

        /// <summary>
        /// Counts report values from WorkingCargoPlan.
        /// </summary>
        /// <param name="cargoPlan">WorkingCargoPlan.</param>
        private void CountReportValues(CargoPlan cargoPlan)
        {
            if (cargoPlan == null) return;

            foreach (var container in cargoPlan.Containers)
            {
                AddContainer(container);
                if (!portOfDischarging.Contains(container.POD))
                    portOfDischarging.Add(container.POD);
            }

            foreach (var dg in cargoPlan.DgList)
            {
                AddDg(dg);
            }

            displayPorts.Sort();
            displayPorts.Insert(0, totalPorts);
        }


        /// <summary>
        /// Adds Dg counts ro respective <see cref="ReportDisplayPort"/>.
        /// </summary>
        /// <param name="dg">Dg to add.</param>
        private void AddDg(Dg dg)
        {
            var port = displayPorts.FirstOrDefault(x => string.Equals(x.Port, dg.POL));

            if (port == null)
            {
                throw new ArgumentNullException("Port record for Dg cannot be created for a port which does not exist in container list.");
            }
            port.AddDg(dg);

            //totals
            totalPorts.AddDg(dg);
        }

        /// <summary>
        /// Adds Container counts ro respective <see cref="ReportDisplayPort"/>.
        /// </summary>
        /// <param name="container">Container to add.</param>
        private void AddContainer(Container container)
        {
            var port = displayPorts.FirstOrDefault(x => string.Equals(x.Port, container.POL));

            if (port == null)
            {
                displayPorts.Add(port = new ReportDisplayPort(container.POL));
            }
            port.AddContainer(container);

            //totals
            totalPorts.AddContainer(container);
        }

        /// <summary>
        /// Sets data source to View property
        /// </summary>
        private void SetDataView()
        {
            displayValuesView.Source = TableData;
        }

        #endregion

    }
}
