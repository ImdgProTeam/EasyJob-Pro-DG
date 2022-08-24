using EasyJob_ProDG.Model.IO;
using EasyJob_ProDG.UI.Data;


namespace EasyJob_ProDG.UI.Services.DataServices
{
    class LoadDataService : ILoadDataService
    {
        readonly ICurrentProgramData _currentProgramData = new CurrentProgramData();

        public bool IsShipProfileNotFound => _currentProgramData.GetShipProfile().IsShipProfileNotFound;
        public bool IsShipProfileDefault => _currentProgramData.GetShipProfile().IsDefault;

        public LoadDataService()
        {

        }

        public void LoadData()
        {
            _currentProgramData.LoadData();
        }
        
        /// <summary>
        /// Connects ShipProfile and Dg DataBase from Program files.
        /// </summary>
        /// <returns>False if Dg DataBase has not been connected.</returns>
        public bool ConnectProgramFiles()
        {
            return _currentProgramData.ConnectProgramFiles();
        }

        public bool OpenNewFile(string file, OpenFile.OpenOption openOption, bool importOnlySelected = false, string currentPort = null)
        {
            return _currentProgramData.OpenNewFile(file, openOption, importOnlySelected, currentPort);
        }

        public void SaveFile(string fileName)
        {
            _currentProgramData.SaveFile(fileName);
        }

        public void ExportToExcel(Wrapper.CargoPlanWrapper cargo)
        {
            _currentProgramData.ExportDgListToExcel(cargo);
        }

        public bool ImportReeferManifestInfo(string file, bool importOnlySelected = false, string currentPort = null)
        {
            return _currentProgramData.ImportReeferManifestInfo(file, importOnlySelected, currentPort);
        }

        public void LoadBlankCargoPlan()
        {
            _currentProgramData.LoadBlankCargoPlan();
        }
    }
}
