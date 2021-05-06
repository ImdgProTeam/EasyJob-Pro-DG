using System.Xml.Linq;
using EasyJob_ProDG.UI.Data;

namespace EasyJob_ProDG.UI.Services.DataServices
{
    class DgDataBaseDataService : IDgDataBaseDataService
    {
        ICurrentProgramData currentProgramData = new CurrentProgramData();

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
