//****************************************************//
//                                                    //
//   Public class to work with cargo and ship data.   //
//   * Gets access to ship profile                    //
//   * Gets access to cargo and conflict data         //
//   * Loads that data and works with changes in that //
//                                                    //
//****************************************************//

using EasyJob_ProDG.Model;
using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.Model.IO;
using EasyJob_ProDG.Model.Transport;
using EasyJob_ProDG.UI.Services.DataServices;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.Wrapper;


namespace EasyJob_ProDG.UI.Data
{
    public class CurrentProgramData : ICurrentProgramData
    {
        #region Private fields
        // ----------- Private fields --------- ----------------------------------------

        private static CargoPlan _workingCargoPlan;
        private static System.Xml.Linq.XDocument _dgDataBase;
        private static IShipProfileDataService _shipProfileDataService; 

        #endregion

        #region Public program files
        // ----------- Public use program files ----------------------------------------

        public static ShipProfile OwnShip;
        #endregion

        #region Public properties
        // ----------- Public properties to be used in view ----------------------------

        public static ConflictsList Conflicts = new();
        public static VentilationRequirements Vents = new();
        public static CargoPlanWrapper WorkingCargoPlan;

        /// <summary>
        /// Current working condition file name
        /// </summary>
        public string ConditionFileName { get; private set; } 

        public ShipProfileDataService ShipProfileDataService
        {
            get => (ShipProfileDataService)_shipProfileDataService;
            set => _shipProfileDataService = value;
        } 

        #endregion


        #region Public methods
        // ----------- Public methods --------------------------------------------------

        /// <summary>
        /// Connects DgDataBase, ShipProfile. Initiates EventSupervisor.
        /// </summary>
        public bool ConnectProgramFiles()
        {
            //Initiate EventSupervisor
            EventSupervisor evS = new();

            ////Connect program files
            return ProgramFiles.Connect(out OwnShip, out _dgDataBase);
        }

        /// <summary>
        /// Loads default cargo plan, checks it and generates wrappers and conflicts.
        /// </summary>
        public void LoadData()
        {
            string openPath = ((View.UI.MainWindow)System.Windows.Application.Current.MainWindow)?.StartupFilePath;

            if (!CreateWorkingCargoPlan(openPath ?? Properties.Settings.Default.WorkingCargoPlanFile))
            {
                EasyJob_ProDG.Data.LogWriter.Write("Blank condition will be created.");
                _workingCargoPlan = new CargoPlan();
                WorkingCargoPlan = new CargoPlanWrapper(_workingCargoPlan);
                ConditionFileName = "Blank";
            }
        }

        /// <summary>
        /// Clears conflicts, checks CargoPlanWrapper, creates conflicts.
        /// </summary>
        public void ReCheckDgWrapperList()
        {
            if (WorkingCargoPlan != null)
            {
                ReCheckDgList();

                ////Display info
                Conflicts.CreateConflictList(WorkingCargoPlan.DgList);
                Vents.Check();
            }
        }

        /// <summary>
        /// ReChecks and updates stowage conflicts in the selected unit
        /// </summary>
        /// <param name="unit"></param>
        public void ReCheckDgWrapperStowage(DgWrapper unit)
        {
            if (unit == null) return;
            ReCheckDgStowage(unit.Model, WorkingCargoPlan.Model);

            Conflicts.UpdateDgWrapperStowageConfilicts(unit);
        }

        /// <summary>
        /// Checks stowage of a selected dg and updates its StowageConflictList
        /// </summary>
        /// <param name="dg"></param>
        /// <param name="cargoPlan"></param>
        private void ReCheckDgStowage(Dg dg, CargoPlan cargoPlan)
        {
            dg.Conflicts?.ClearStowageConflicts();
            Stowage.CheckUnitStowage(dg, OwnShip, cargoPlan.Containers);
        }

        /// <summary>
        /// Updates cargo hold number, checks stowage and segregation and generates conflicts.
        /// Used when save ShipProfile.
        /// </summary>
        public void FullDataReCheck()
        {
            OnShipProfileSavedUpdates();
        }

        public void LoadBlankCargoPlan()
        {
            CreateBlankCargoPlan();
        }

        /// <summary>
        /// Opens new condition from a readable file on disk.
        /// </summary>
        /// <param name="file">Readable file with full path.</param>
        /// <param name="openOption">Open option enumeration: Open, Update, Import.</param>
        /// <param name="importOnlySelected">For import: Import only selected for import items.</param>
        /// <param name="currentPort">Port of loading for selected data import.</param>
        /// <returns>True if opened successfully</returns>
        public bool OpenNewFile(string file, OpenFile.OpenOption openOption, bool importOnlySelected = false, string currentPort = null)
        {
            return OpenOnExecuted(file, openOption, importOnlySelected, currentPort);
        }

        /// <summary>
        /// Saves condition to a file on disk with dialog window.
        /// </summary>
        /// <param name="fileName">Saving file name</param>
        public void SaveFile(string fileName)
        {
            SaveOnExecuted(fileName);
        }

        public bool ImportReeferManifestInfo(string file, bool importOnlySelected = false, string currentPort = null)
        {
            return ImportReeferManifestInfoOnExecuted(file, importOnlySelected, currentPort);
        }

        /// <summary>
        /// Exports CargoPlanWrapper to Excel
        /// </summary>
        /// <param name="cargoPlan">CargoPlan wrapper</param>
        public void ExportDgListToExcel(CargoPlanWrapper cargoPlan)
        {
            Model.IO.Excel.WithXl.Export(cargoPlan.ExtractPocoDgList());
        } 

        #endregion


        #region Private methods
        // ----------- Private methods --------------------------------------------------

        /// <summary>
        /// Creates new CargoPlan with no cargo in it.
        /// </summary>
        private void CreateBlankCargoPlan()
        {
            _workingCargoPlan = new CargoPlan();
            WorkingCargoPlan?.Destructor();
            WorkingCargoPlan = new CargoPlanWrapper(_workingCargoPlan);
            Conflicts.Clear();
        }

        /// <summary>
        /// Reads a file and creates cargo plan, wrappers, checks them and generates conflicts.
        /// Called on startup load, when opening any condition file.
        /// </summary>
        /// <param name="fileName">Full path of file to open</param>
        /// <param name="openOption">Select from enumeration weather to Open, Update or Import data</param>
        /// <returns>True if CargoPlan created successfully</returns>
        private bool CreateWorkingCargoPlan(string fileName, OpenFile.OpenOption openOption = OpenFile.OpenOption.Open, bool importOnlySelected = false, string currentPort = null)
        {
            var tempCargoPlan = new CargoPlan().CreateCargoPlan(fileName, OwnShip, _dgDataBase, openOption, _workingCargoPlan, importOnlySelected, currentPort);
            if (tempCargoPlan == null || tempCargoPlan.IsEmpty) return false;

            _workingCargoPlan = tempCargoPlan;
            ReCheckDgList(_workingCargoPlan);

            ////Creating wrapper
            WorkingCargoPlan?.Destructor();
            WorkingCargoPlan = new CargoPlanWrapper(_workingCargoPlan);

            ////Display info
            Conflicts.CreateConflictList(WorkingCargoPlan.DgList);
            Vents.Check();

            ConditionFileName = OpenFile.FileName;

            return true;
        }

        /// <summary>
        /// Imports manifest info from excel file and updates Reefers with its data..
        /// </summary>
        /// <param name="file">Excel file path containing manifest info.</param>
        /// <param name="importOnlySelected">If selected, then import only selected reefers info.</param>
        /// <param name="currentPort">If not null (by default null), then only current POL reefers will be updated.</param>
        /// <returns></returns>
        private bool ImportReeferManifestInfoOnExecuted(string file, bool importOnlySelected, string currentPort)
        {
            return _workingCargoPlan.ImportReeferManifestInfoFromExcel(file, importOnlySelected, currentPort);
        }

        /// <summary>
        /// Clears existing conflicts, checks the plan and creates conflicts.
        /// When changing property of dg in list.
        /// </summary>
        private void ReCheckDgList()
        {
            ReCheckDgList(WorkingCargoPlan.Model);
        }

        /// <summary>
        /// Clears existing conflicts, checks the given plan and creates conflicts.
        /// When start program, when change dg properties, deleting cargo, open new condition
        /// </summary>
        /// <param name="sourceCargoPlan">Plain cargo plan (model)</param>
        private void ReCheckDgList(CargoPlan sourceCargoPlan)
        {
            //Clear all conflicts
            sourceCargoPlan.ClearConflicts();

            //Check stowage and segregation
            Stowage.CheckStowage(sourceCargoPlan, OwnShip);
            CheckSegregation(sourceCargoPlan, OwnShip);
        }

        /// <summary>
        /// Checks segregation in plain CargoPlan.
        /// </summary>
        /// <param name="cargoPlan">Plain CargoPlan</param>
        /// <param name="ownShip">Current ShipProfile</param>
        private void CheckSegregation(CargoPlan cargoPlan, ShipProfile ownShip)
        {
            Model.Cargo.HandleDgList.CheckSegregation(cargoPlan, ownShip);
            //enterlog(logstreamwriter, "segregation checked");
        }

        /// <summary>
        /// Assigns hold numbers to all units, checks cargo plan, creates wrappers and conflicts.
        /// Used when save ShipProfile. Called by FullReCheck public method.
        /// </summary>
        private void OnShipProfileSavedUpdates()
        {
            AssignHoldNumberToAllInCargoPlan();
            ReCheckDgList(_workingCargoPlan);

            WorkingCargoPlan.CreateCargoPlanWrapper(_workingCargoPlan);

            ////Display info
            Conflicts.CreateConflictList(WorkingCargoPlan.DgList);
            Vents.Check();
        }

        /// <summary>
        /// Assigns Hold number to each unit in entire current CargoPlan.
        /// Called by UpdateCargoPlan private method.
        /// </summary>
        private void AssignHoldNumberToAllInCargoPlan()
        {
            foreach (var unit in _workingCargoPlan.Containers)
            {
                unit.HoldNr = _shipProfileDataService.DefineCargoHoldNumber(unit.Bay);
            }

            foreach (var unit in _workingCargoPlan.DgList)
            {
                unit.HoldNr = _shipProfileDataService.DefineCargoHoldNumber(unit.Bay);
            }

            foreach (var unit in _workingCargoPlan.Reefers)
            {
                unit.HoldNr = _shipProfileDataService.DefineCargoHoldNumber(unit.Bay);
            }
        }

        /// <summary>
        /// Catching exceptions while creating new CargoPlan from file.
        /// </summary>
        /// <param name="file">Readable condition file</param>
        /// <param name="openOption">Option to Open, Update or Import data.</param>
        /// <param name="importOnlySelected">For import: Import only selected for import items.</param>
        /// <param name="currentPort">Port for selecting import.</param>
        /// <returns>True if file read successfully</returns>
        private bool OpenOnExecuted(string file, OpenFile.OpenOption openOption, bool importOnlySelected, string currentPort)
        {
#if !DEBUG
            try
            {
#endif
            bool result = CreateWorkingCargoPlan(file, openOption, importOnlySelected, currentPort);
            ConditionFileName = OpenFile.FileName;
            return result;
#if !DEBUG
        }
            catch
            {
                return false;
            }
#endif
        }

        private void SaveOnExecuted(string fileName)
        {
            ExportCondition.SaveFile(_workingCargoPlan, fileName);
            ConditionFileName = OpenFile.FileName;
        } 

        #endregion


        #region Get data methods

        // ---------------- Get data methods ------------------------------------------------------

        public ConflictsList GetConflictsList()
        {
            return Conflicts;
        }

        public VentilationRequirements GetVentilationRequirements()
        {
            return Vents;
        }

        public CargoPlanWrapper GetCargoPlan()
        {
            return WorkingCargoPlan;
        }

        public ShipProfile GetShipProfile()
        {
            return OwnShip;
        }

        public System.Xml.Linq.XDocument GetDgDataBase()
        {
            return _dgDataBase;
        } 

        #endregion


        #region Constructors and Singleton
        // ---------------- Constructors --------------------------------------------------------

        /// <summary>
        /// Empty constructor
        /// </summary>
        private CurrentProgramData()
        {

        }

        private static CurrentProgramData instance = null;
        public static CurrentProgramData GetCurrentProgramData()
        {
            if (instance == null)
                instance = new CurrentProgramData();
            return instance;
        } 

        #endregion
    }
}
