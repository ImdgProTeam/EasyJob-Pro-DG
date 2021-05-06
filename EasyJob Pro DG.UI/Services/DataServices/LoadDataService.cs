using EasyJob_ProDG.Model.IO;
using EasyJob_ProDG.UI.Data;
using EasyJob_ProDG.UI.Wrapper;

namespace EasyJob_ProDG.UI.Services.DataServices
{
    class LoadDataService : ILoadDataService
    {
        readonly ICurrentProgramData _currentProgramData = new CurrentProgramData();

        public LoadDataService()
        {
            //currentProgramData = currentProgramData;
        }

        public void LoadData()
        {
            _currentProgramData.LoadData();
        }

        public void ConnectProgramFiles()
        {
            _currentProgramData.ConnectProgramFiles();
        }

        public bool OpenNewFile(string file, OpenFile.OpenOption openOption, bool importOnlySelected = false, string currentPort = null)
        {
            return _currentProgramData.OpenNewFile(file, openOption, importOnlySelected, currentPort);
        }

        public void SaveFile(string fileName)
        {
            _currentProgramData.SaveFile(fileName);
        }

        public void ExportToExcel(CargoPlanWrapper cargo)
        {
            _currentProgramData.ExportDgListToExcel(cargo);
        }
    }
}
