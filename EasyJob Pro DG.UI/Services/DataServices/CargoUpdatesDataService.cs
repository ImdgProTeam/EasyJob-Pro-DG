using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.UI.Wrapper;
using System.Collections.Generic;
using System.Linq;

namespace EasyJob_ProDG.UI.Services.DataServices
{
    public class CargoUpdatesDataService : ICargoUpdatesDataService
    {
        ICargoDataService _cargoDataService => CargoDataService.GetCargoDataService();

        public CargoPlanWrapper WorkingCargoPlan => _cargoDataService.WorkingCargoPlan;

        public List<Container> LoadedContainers => WorkingCargoPlan.Model.Updates.LoadedContainers;
        public List<Container> DischargedContainers => WorkingCargoPlan.Model.Updates.DischargedContainers;
        public bool HasUpdates => 
            !WorkingCargoPlan.Model.Updates.IsEmpty 
            || WorkingCargoPlan.Containers.Any(c => c.HasPodChanged)
            || WorkingCargoPlan.Containers.Any(c => c.HasLocationChanged);
    }
}
