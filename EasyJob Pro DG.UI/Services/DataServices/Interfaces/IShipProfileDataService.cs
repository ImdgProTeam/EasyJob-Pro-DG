using EasyJob_ProDG.UI.Wrapper;

namespace EasyJob_ProDG.UI.Services.DataServices
{
    public interface IShipProfileDataService
    {
        ShipProfileWrapper CreateShipProfileWrapper();
        void SaveShipProfile();
        void OpenShipProfile();
    }
}
