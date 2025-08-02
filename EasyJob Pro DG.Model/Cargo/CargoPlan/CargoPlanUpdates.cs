using System.Collections.Generic;

namespace EasyJob_ProDG.Model.Cargo
{
    /// <summary>
    /// Class contains data of removed and added containers when Update condition.
    /// </summary>
    public class CargoPlanUpdates
    {
        public List<Container> DischargedContainers { get; set; }
        public List<Container> LoadedContainers { get; set; }

        public bool IsEmpty => DischargedContainers.Count == 0 && LoadedContainers.Count == 0;
        public bool HasPOLChanged { get; set; } = true;


        public CargoPlanUpdates()
        {
            DischargedContainers = new List<Container>();
            LoadedContainers = new List<Container>();
        }
    }
}
