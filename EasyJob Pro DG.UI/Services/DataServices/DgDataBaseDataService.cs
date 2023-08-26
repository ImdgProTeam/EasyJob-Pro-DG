using System.Xml.Linq;
using EasyJob_ProDG.UI.Data;

namespace EasyJob_ProDG.UI.Services.DataServices
{
    class DgDataBaseDataService : IDgDataBaseDataService
    {
        ICurrentProgramData currentProgramData => CurrentProgramData.GetCurrentProgramData();

        public DgDataBaseDataService()
        {
            //this.currentProgramData = currentProgramData;
        }

        public XDocument GetDgDataBase()
        {
            return currentProgramData.GetDgDataBase();
        }
    }
}
