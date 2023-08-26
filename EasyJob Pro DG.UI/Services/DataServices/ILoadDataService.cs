namespace EasyJob_ProDG.UI.Services.DataServices
{
    public interface ILoadDataService
    {
        bool IsShipProfileNotFound { get; }
        bool IsShipProfileDefault { get; }

        void LoadData();
        bool ConnectProgramFiles();
        void LoadBlankCargoPlan();
    }
}
