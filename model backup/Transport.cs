using System;


namespace EasyJob_Pro_DG
{
    public class Transport
    {
        public string portOfDeparture;
        public string portOfDestination;
        public string voyageNr;
        public string callSign;
        public DateTime departureDate;
        public bool passenger;          //Implement procedure to change on demand

        public Transport()
        {
            portOfDeparture = null;
            portOfDestination = null;
            voyageNr = null;
            callSign = null;
            passenger = false;
        }

    }
}
