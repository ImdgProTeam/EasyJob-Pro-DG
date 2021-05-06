using EasyJob_ProDG.Model.Transport;
using EasyJob_ProDG.UI.Wrapper;

namespace EasyJob_ProDG.UI.Services.DataServices
{
    public interface IShipProfileDataService
    {
        ShipProfile GetShipProfile();
        ShipProfileWrapper CreateShipProfileWrapper();
        void SaveShipProfile();
        void OpenShipProfile();
        byte DefineCargoHoldNumber(byte bay);
    }
}
