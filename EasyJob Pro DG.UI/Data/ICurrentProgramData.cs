//***************************************************//
//                                                   //
//   Public interface to CurrentProgramData Class    //
//                                                   //
//                                                   //
//***************************************************//

using EasyJob_ProDG.Model.Transport;
using EasyJob_ProDG.UI.Services.DataServices;
using EasyJob_ProDG.UI.Wrapper;
using System.Xml.Linq;
using EasyJob_ProDG.Model.IO;

namespace EasyJob_ProDG.UI.Data
{
    internal interface ICurrentProgramData
    {
        ShipProfileDataService ShipProfileDataService { get; set; }

        bool ConnectProgramFiles();
        void LoadData();
        void ReCheckDgWrapperStowage(DgWrapper dgWrapper);
        void ReCheckDgWrapperList();
        void FullDataReCheck();
        ConflictsList GetConflictsList();
        VentilationRequirements GetVentilationRequirements();
        CargoPlanWrapper GetCargoPlan();
        ShipProfile GetShipProfile();
        XDocument GetDgDataBase();
        void LoadBlankCargoPlan();
        bool OpenNewFile(string file, OpenFile.OpenOption openOption, bool onlySelected = false, string currentPort=null);
        bool UpdateWithNewFile(string file);
        void SaveFile(string fileName);
        void ExportDgListToExcel(CargoPlanWrapper cargo);
        bool ImportReeferManifestInfo(string file, bool importOnlySelected, string currentPort);

        string ConditionFileName { get; }

    }
}
