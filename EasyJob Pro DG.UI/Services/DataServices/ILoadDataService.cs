using EasyJob_ProDG.Model.IO;

namespace EasyJob_ProDG.UI.Services.DataServices
{
    public interface ILoadDataService
    {
        bool IsShipProfileNotFound { get; }

        bool ConnectProgramFiles();

        void LoadCargoData(string openPath);
        void LoadBlankCargoPlan();

        bool OpenCargoPlanFromFile(string file, OpenFile.OpenOption openOption, bool importOnlySelected, string currentPort);

        /// <summary>
        /// Save condition file with given fileName
        /// </summary>
        /// <param name="fileName"></param>
        void SaveConditionToFile(string fileName);
        
        bool ImportReeferManifestInfo(string file, bool importOnlySelected, string currentPort);
        void ExportDgListToExcel();
    }
}
