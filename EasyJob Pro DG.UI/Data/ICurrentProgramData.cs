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
using EasyJob_ProDG.Model.Cargo;

namespace EasyJob_ProDG.UI.Data
{
    internal interface ICurrentProgramData
    {
        string ConditionFileName { get; }
        CargoPlan CargoPlan { get; }

        CargoPlan GetCargoPlan();
        ShipProfile GetShipProfile();


        void SetConditionFileName(string name);
        void SetCargoPlan(CargoPlan cargoPlan);

        //void ReCheckDgWrapperStowage(DgWrapper dgWrapper);
        //void ReCheckDgWrapperList();
        //void FullDataReCheck();
        //ConflictsList GetConflictsList();
        //VentilationRequirements GetVentilationRequirements();
        ////CargoPlanWrapper GetCargoPlan();
        ////XDocument GetDgDataBase();
        //void LoadBlankCargoPlan();
        //bool OpenNewFile(string file, OpenFile.OpenOption openOption, bool onlySelected = false, string currentPort=null);
        //void SaveConditionToFile(string fileName);
        //void ExportDgListToExcel(CargoPlanWrapper cargo);
        //bool ImportReeferManifestInfo(string file, bool importOnlySelected, string currentPort);


    }
}
