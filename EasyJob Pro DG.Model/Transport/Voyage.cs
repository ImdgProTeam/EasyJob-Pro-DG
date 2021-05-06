using System;

namespace EasyJob_ProDG.Model.Transport
{
    public class Voyage
    {
        public string PortOfDeparture { get; set; }
        public string PortOfDestination { get; set; }
        public string VoyageNumber { get; set; }
        public string CallSign;
        public DateTime DepartureDate;

        public Voyage()
        {
            PortOfDeparture = null;
            PortOfDestination = null;
            VoyageNumber = null;
            CallSign = null;
        }
    }
}
