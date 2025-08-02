using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.UI.Wrapper;
using System.Collections.Generic;

namespace EasyJob_ProDG.UI.Services.DataServices
{
    public interface ICargoUpdatesDataService
    {
        List<Container> DischargedContainers { get; }
        bool HasUpdates { get; }
        List<Container> LoadedContainers { get; }
        CargoPlanWrapper WorkingCargoPlan { get; }
    }
}