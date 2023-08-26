using EasyJob_ProDG.Data.Info_data;
using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.Model.Transport;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.View.DialogWindows.Summaries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Data;

namespace EasyJob_ProDG.UI.View.DialogWindows
{
    internal class DgSummaryReportViewModel : Observable
    {
        #region Private fields and constants

        internal const string _TOTAL = "Total";
        internal const string _MP = "MP";


        /// <summary>
        /// List of all displayable dg classes
        /// </summary>
        private List<string> AllDgClasses = IMDGCode.AllValidDgClasses;


        private List<SingleClassReportValue> displayClasses;
        private List<SingleClassReportValue> displayClassesOfCurrentPOL;
        private readonly CollectionViewSource displayValuesView = new CollectionViewSource();
        private SingleClassReportValue totalClass;
        private SingleClassReportValue totalClassOfCurrentPOL;
        private SingleClassReportValue MPclass;
        private SingleClassReportValue MPclassOfCurrentPOL;
        private List<string> portsOfDischarging;

        /// <summary>
        /// Table to contain the display data in required order.
        /// </summary>
        private DataTable TableDataFull;

        /// <summary>
        /// Table contains full data for all classes where total is more than 0.
        /// </summary>
        private DataTable TableDataOnlyWithValues;

        /// <summary>
        /// Table contains all Dg data for selected POL only.
        /// </summary>
        private DataTable TableDataSelectedPOL;

        /// <summary>
        /// Table contains all Dg data for selected POL only and where total is more than 0.
        /// </summary>
        private DataTable TableDataSelectedPOLOnlyWithValues;

        private CargoPlan cargoPlan;
        private Voyage voyage;

        #endregion

        #region Public properties

        /// <summary>
        /// Used to bind DataGrid
        /// </summary>
        public System.ComponentModel.ICollectionView CollectionView => displayValuesView?.View;


        /// <summary>
        /// Window title
        /// </summary>
        public string Title => "Dangerous cargo summary report";

        #endregion


        #region Constructors
        //----------- Constructors -------------

        public DgSummaryReportViewModel(Voyage voyage)
        {
            this.voyage = voyage;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public DgSummaryReportViewModel()
        {

        }


        #endregion


        #region Public methods

        /// <summary>
        /// Creates report from CargoPlan.
        /// </summary>
        /// <param name="cargoPlan">CargoPlan.</param>
        public void CreateReport(CargoPlan cargoPlan)
        {
            //store cargoPlan in the field
            this.cargoPlan = cargoPlan;

            //creating list of classes (further rows)
            displayClasses = new List<SingleClassReportValue>();
            foreach (var c in AllDgClasses)
            {
                displayClasses.Add(new SingleClassReportValue(c));
            }

            MPclass = new SingleClassReportValue(_MP);
            totalClass = new SingleClassReportValue(_TOTAL);
            displayClasses.Add(MPclass);
            displayClasses.Add(totalClass);


            //list of POD (columns) will be created during Count().
            portsOfDischarging = new();

            CountReportValues(cargoPlan);
            SetDisplayValuesBasedOnSelectedProperty((byte)selectedPropertyDisplayOptionIndex);

            SetTableData(ref TableDataFull);
            SetDataView();

        }

        #endregion


        #region TableData logic

        /// <summary>
        /// Fills DataTable with generated data in string format.
        /// </summary>
        private void SetTableData(ref DataTable dataTable, bool isPOLselected = false)
        {
            dataTable = new();

            //Creating columns
            //default columns
            dataTable.AddColumnWithStringValues("Class", 0);
            dataTable.AddColumn<SinglePortClassReportValue>(_TOTAL, 1);

            foreach (var port in portsOfDischarging)
            {
                dataTable.AddColumn<SinglePortClassReportValue>(port);
            }

            //MP column
            dataTable.AddColumn<SinglePortClassReportValue>(_MP);


            //filling the table
            foreach (var line in isPOLselected ? displayClassesOfCurrentPOL : displayClasses)
            {
                //individual rows
                List<SinglePortClassReportValue> values = new List<SinglePortClassReportValue>();
                foreach (DataColumn header in dataTable.Columns)
                {
                    //Class column
                    if (string.Equals(header.ColumnName, "Class"))
                    {
                        values.Add(new SinglePortClassReportValue("Class") { DisplayValue = line.Class });
                        continue;
                    }

                    //other columns
                    if (line.ClassCountValues.ContainsKey(header.ColumnName))
                    {
                        values.Add(line.ClassCountValues[header.ColumnName]);
                    }
                    else
                        //to display meaningless zeros as well
                        //{
                        //    var newBlancSinglePort = new SinglePortClassReportValue(header.ColumnName);
                        //    values.Add(newBlancSinglePort);
                        //    line.ClassCountValues.Add(header.ColumnName, newBlancSinglePort);
                        //}
                        //otherwise:
                        values.Add(new SinglePortClassReportValue(header.ColumnName));
                }

                dataTable.AddRow(values.ToArray());
            }
        }

        #endregion



        #region Filter Logic
        // ----------- Filter logic ----------------

        // ----- Options enumerations

        /// <summary>
        /// Report options to be displayed in filter combobox
        /// </summary>
        public IEnumerable<string> ClassDisplayReportOptions
        {
            get => new List<string>()
            {
                "All classes",
                "Only with values",
                "Current POL",
                "Current POL only with values"
            };
        }

        /// <summary>
        /// Report options enumerator
        /// </summary>
        public enum ClassReportOptions : byte
        {
            All = 0,
            OnlyWithValues = 1,
            CurrentPOL = 2,
            CurrentPOLOnlyWithValues
        }

        /// <summary>
        /// Report options to be displayed in filter combobox
        /// </summary>
        public IEnumerable<string> PropertyDisplayReportOptions
        {
            get => new List<string>()
            {
                "Containers count",
                "Dg net weight",
                "Containers count / dg net weight"
            };
        }

        /// <summary>
        /// Report options enumerator
        /// </summary>
        public enum PropertyReportOptions : byte
        {
            ContainersCount = 0,
            DgNetWight = 1,
            ContainersCoundAndDgNetWeight = 2
        }


        // ----- Properties -----

        /// <summary>
        /// Selected Display option index.
        /// By default is 0.
        /// </summary>
        public byte SelectedClassDisplayOptionIndex
        {
            get
            {
                return selectedClassDisplayOptionIndex;
            }
            set
            {
                selectedClassDisplayOptionIndex = value;
                SetDisplayValuesBasedOnSelectedOption(selectedClassDisplayOptionIndex);
                OnPropertyChanged(nameof(CollectionView));
            }
        }
        private byte selectedClassDisplayOptionIndex = 0;

        /// <summary>
        /// Selected Display property index.
        /// By default is 0.
        /// </summary>
        public byte SelectedPropertyDisplayOptionIndex
        {
            get
            {
                return selectedPropertyDisplayOptionIndex;
            }
            set
            {
                selectedPropertyDisplayOptionIndex = value;
                SetDisplayValuesBasedOnSelectedProperty((byte)selectedPropertyDisplayOptionIndex);
                OnPropertyChanged(nameof(CollectionView));
            }
        }
        private byte selectedPropertyDisplayOptionIndex = 0;


        // ----- Mehtods -----

        /// <summary>
        /// Sets/changes display values to all cells in the <see cref="displayClasses"/> in accordance with <see cref="SelectedPropertyDisplayOptionIndex"/>
        /// </summary>
        /// <param name="selectedPropertyDisplayIndex"></param>
        private void SetDisplayValuesBasedOnSelectedProperty(byte selectedPropertyDisplayIndex)
        {
            foreach (var dgClass in displayClasses)
            {
                dgClass.SetDisplayValues((PropertyReportOptions)selectedPropertyDisplayIndex);
            }
            if (displayClassesOfCurrentPOL != null)
                foreach (var dgClass in displayClassesOfCurrentPOL)
                {
                    dgClass.SetDisplayValues((PropertyReportOptions)selectedPropertyDisplayIndex);
                }

            CollectionView?.Refresh();
        }

        /// <summary>
        /// Changes displayed values in accordance with selected <see cref="SelectedClassDisplayOptionIndex"/>.
        /// </summary>
        /// <param name="selectedDisplayOptionIndex">Index in accordance with enum <see cref="ClassDisplayReportOptions"/></param>
        private void SetDisplayValuesBasedOnSelectedOption(byte selectedDisplayOptionIndex = 0)
        {
            switch (selectedDisplayOptionIndex)
            {
                case (byte)ClassReportOptions.All:
                    displayValuesView.Source = TableDataFull;
                    break;
                case (byte)ClassReportOptions.OnlyWithValues:
                    CreateTableDataOnlyWithValues(ref TableDataOnlyWithValues, TableDataFull);
                    displayValuesView.Source = TableDataOnlyWithValues;
                    break;
                case (byte)ClassReportOptions.CurrentPOL:
                    CreateTableDataForSelectedPOL(voyage.PortOfDeparture);
                    displayValuesView.Source = TableDataSelectedPOL;
                    break;
                case (byte)ClassReportOptions.CurrentPOLOnlyWithValues:
                    CreateTableDataForSelectedPOL(voyage.PortOfDeparture);
                    CreateTableDataOnlyWithValues(ref TableDataSelectedPOLOnlyWithValues, TableDataSelectedPOL);
                    displayValuesView.Source = TableDataSelectedPOLOnlyWithValues;
                    break;
                default:
                    break;

            }
        }

        /// <summary>
        /// Creates TableDataSelectedPOL
        /// </summary>
        /// <param name="selectedPOL">Selected POL name.</param>
        private void CreateTableDataForSelectedPOL(string selectedPOL)
        {
            if (displayClassesOfCurrentPOL != null) return;

            //creating list of classes (further rows)
            displayClassesOfCurrentPOL = new List<SingleClassReportValue>();
            foreach (var c in AllDgClasses)
            {
                displayClassesOfCurrentPOL.Add(new SingleClassReportValue(c));
            }

            MPclassOfCurrentPOL = new SingleClassReportValue(_MP);
            totalClassOfCurrentPOL = new SingleClassReportValue(_TOTAL);
            displayClassesOfCurrentPOL.Add(MPclassOfCurrentPOL);
            displayClassesOfCurrentPOL.Add(totalClassOfCurrentPOL);


            //list of POD (columns) will be created during Count().
            portsOfDischarging = new();

            CountReportValues(cargoPlan, selectedPOL);
            SetDisplayValuesBasedOnSelectedProperty((byte)selectedPropertyDisplayOptionIndex);

            SetTableData(ref TableDataSelectedPOL, isPOLselected: true);
        }

        /// <summary>
        /// Copies from this TableData only rows where total is not 0.
        /// No action if resultDataTable exists (!= null).
        /// </summary>
        private void CreateTableDataOnlyWithValues(ref DataTable resultDataTable, DataTable sourceDataTable)
        {
            if (resultDataTable == null)
            {
                IEnumerable<DataRow> query =
                    from row in sourceDataTable.AsEnumerable()
                    where row.Field<SinglePortClassReportValue>(_TOTAL)?.ContainersCount > 0
                    select row;

                resultDataTable = new DataTable();
                resultDataTable = query.CopyToDataTable();
            }
        }

        #endregion


        #region Private methods
        //----------- Private methods ----------

        /// <summary>
        /// Counts report values from CargoPlan.
        /// </summary>
        /// <param name="cargoPlan">CargoPlan.</param>
        /// <param name="selectedPOL">In case the values are to be calculated for a selected port only.</param>
        private void CountReportValues(CargoPlan cargoPlan, string selectedPOL = null)
        {
            if (cargoPlan == null) return;

            foreach (var dg in cargoPlan.DgList)
            {
                //skip if the count is for the selected port only
                if (!string.IsNullOrEmpty(selectedPOL) && !string.Equals(selectedPOL, dg.POL))
                    continue;


                if (string.IsNullOrEmpty(selectedPOL))
                    AddDg(dg);
                else
                    AddDgOfCurrentPOL(dg);
                if (!portsOfDischarging.Contains(dg.POD))
                    portsOfDischarging.Add(dg.POD);
            }

            portsOfDischarging.Sort();
        }


        /// <summary>
        /// Adds Dg counts ro respective <see cref="SingleClassReportValue"/>.
        /// Used for full report.
        /// </summary>
        /// <param name="dg">Dg to add.</param>
        private void AddDg(Dg dg)
        {
            var dgClass = displayClasses.FirstOrDefault(x => dg.DgClass.StartsWith(x.Class));

            AddDgToDgClass(dg, dgClass);

            //totals
            totalClass.AddDg(dg);

            //MP
            if (dg.IsMp)
                MPclass.AddDg(dg);
        }

        /// <summary>
        /// Adds Dg counts ro respective <see cref="SingleClassReportValue"/> of <see cref="displayClassesOfCurrentPOL"/>.
        /// Used for selectedPOL report.
        /// </summary>
        /// <param name="dg">Dg to add.</param>
        private void AddDgOfCurrentPOL(Dg dg)
        {
            var dgClass = displayClassesOfCurrentPOL.FirstOrDefault(x => dg.DgClass.StartsWith(x.Class));

            AddDgToDgClass(dg, dgClass);

            //totals
            totalClassOfCurrentPOL.AddDg(dg);

            //MP
            if (dg.IsMp)
                MPclassOfCurrentPOL.AddDg(dg);
        }

        /// <summary>
        /// Adds dg to respective <see cref="SinglePortClassReportValue"/> of defined dgClass.
        /// </summary>
        /// <param name="dg"></param>
        /// <param name="dgClass">Defined dg class.</param>
        private void AddDgToDgClass(Dg dg, SingleClassReportValue? dgClass)
        {
            if (dgClass == null)
            {
                throw new ArgumentNullException($"Dg class {dg.DgClass} is not a valid class for the report.");
            }
            dgClass.AddDg(dg);
        }


        /// <summary>
        /// Sets data source to View property
        /// </summary>
        private void SetDataView()
        {
            displayValuesView.Source = TableDataFull;
        }

        #endregion

    }
}


