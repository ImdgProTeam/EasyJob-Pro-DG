using EasyJob_ProDG.Model;
using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.Model.IO;
using EasyJob_ProDG.Model.IO.EasyJobCondition;
using EasyJob_ProDG.UI.Data;

namespace EasyJob_ProDG.UI.Services.DataServices
{
    /// <summary>
    /// Responsible for the interaction of UI project with Model project.
    /// Obtains program data and handles open/close/save manipulations with files.
    /// </summary>
    class LoadDataService : ILoadDataService
    {
        readonly ICurrentProgramData _currentProgramData = CurrentProgramData.GetCurrentProgramData();
        private CargoPlan _cargoPlan => _currentProgramData.CargoPlan;

        public bool IsShipProfileNotFound => _currentProgramData.GetShipProfile().IsShipProfileNotFound;



        /// <summary>
        /// Connects DgDataBase, ShipProfile. Initiates EventSupervisor.
        /// </summary>
        /// <returns>True if dg database has been successfully connected.</returns>
        public bool ConnectProgramFiles()
        {
            ////Connect program files
            return ProgramFiles.Connect();
        }

        /// <summary>
        /// Loads default cargo plan.
        /// Called on program start up.
        /// </summary>
        /// <param name="openPath">Path of WorkingCargoPlan received from MainWindow.</param>
        public void LoadCargoData(string openPath)
        {
            if (!OpenCargoPlanFromFile(openPath ?? Properties.Settings.Default.WorkingCargoPlanFile))
            {
                EasyJob_ProDG.Data.LogWriter.Write("Blank condition will be created.");
                CreateBlankCargoPlan();
            }
        }

        /// <summary>
        /// Sets up new blank <see cref="CargoPlan"/> as the working cargo plan.
        /// </summary>
        public void LoadBlankCargoPlan()
        {
            CreateBlankCargoPlan();
        }

        /// <summary>
        /// Creates working cargo plan and checks its stowage and segregation.
        /// Catching exceptions while creating new WorkingCargoPlan from filePath.
        /// </summary>
        /// <param name="file">Readable condition filePath</param>
        /// <param name="openOption">Option to Open, Update or Import data.</param>
        /// <param name="importOnlySelected">For import: Import only selected for import items.</param>
        /// <param name="currentPort">Port for selecting import.</param>
        /// <returns>True if filePath read successfully</returns>
        public bool OpenCargoPlanFromFile(string file, OpenFile.OpenOption openOption = OpenFile.OpenOption.Open, bool importOnlySelected = false, string currentPort = null)
        {
#if !DEBUG
            try
            {
#endif
            bool result = CreateCargoPlanFromFile(file, openOption, importOnlySelected, currentPort);

            if (openOption == OpenFile.OpenOption.Import)
                _currentProgramData.ApendConditionFileNameWithImported();
            else
                _currentProgramData.SetConditionFileName(OpenFile.FileName);

            return result;
#if !DEBUG
        }
            catch
            {
                return false;
            }
#endif
        }

        /// <summary>
        /// Saves condition to the filePath on disk.
        /// </summary>
        /// <param name="filePath">Saving filePath full path</param>
        public void SaveConditionToFile(string filePath)
        {
            ExportCondition.SaveFile(_cargoPlan, filePath);
            _currentProgramData.SetConditionFileName(OpenFile.FileName);
        }

        /// <summary>
        /// Exports DgList to Excel
        /// </summary>
        /// <param name="cargoPlan">WorkingCargoPlan</param>
        public void ExportDgListToExcel()
        {
            Model.IO.Excel.WithXlDg.Export(_cargoPlan.DgList);
        }

        /// <summary>
        /// Imports manifest info from excel filePath and updates Reefers with its data..
        /// </summary>
        /// <param name="filePath">Excel filePath path containing manifest info.</param>
        /// <param name="importOnlySelected">If selected, then import only selected reefers info.</param>
        /// <param name="currentPort">If not null (by default null), then only current POL reefers will be updated.</param>
        /// <returns></returns>
        public bool ImportReeferManifestInfo(string filePath, bool importOnlySelected = false, string currentPort = null)
        {
            return _cargoPlan.ImportReeferManifestInfoFromExcel(filePath, importOnlySelected, currentPort);
        }


        // ----- Private methods -----

        /// <summary>
        /// Creates new WorkingCargoPlan with no cargo in it.
        /// </summary>
        private void CreateBlankCargoPlan()
        {
            _currentProgramData.SetCargoPlan(new CargoPlan());
            _currentProgramData.SetConditionFileName("Blank");
            EasyJob_ProDG.Data.LogWriter.Write("Blank condition has been created.");
        }

        /// <summary>
        /// Reads a fileNam, creates cargo plan from it and sets it as the working cargo plan in <see cref="CurrentProgramData"/>.
        /// </summary>
        /// <param name="fileName">Full path with fileName to open</param>
        /// <param name="openOption">Select from enumeration weather to Open, Update or Import data</param>
        /// <param name="importOnlySelected">For import: Import only selected items.</param>
        /// <param name="currentPort">Port for selecting import.</param>
        /// <returns>True if WorkingCargoPlan created successfully</returns>
        private bool CreateCargoPlanFromFile(string fileName, OpenFile.OpenOption openOption = OpenFile.OpenOption.Open, bool importOnlySelected = false, string currentPort = null)
        {
            var tempCargoPlan = HandleCargoPlan.CreateCargoPlan(fileName, openOption, _cargoPlan, importOnlySelected, currentPort);
            if (tempCargoPlan == null || tempCargoPlan.IsEmpty) return false;

            _currentProgramData.SetCargoPlan(tempCargoPlan);

            return true;
        }


        #region Constructor

        public LoadDataService()
        {

        }

        #endregion
    }
}
