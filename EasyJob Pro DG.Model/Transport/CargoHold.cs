namespace EasyJob_ProDG.Model.Transport
{
    public class CargoHold
    {
        public byte FirstBay { get; set; }
        public byte LastBay { get; set; }
        public byte HoldNumber { get; set; }


        public CargoHold()
        {
            FirstBay = 0;
            LastBay = 0;
        }

        public CargoHold(byte holdNumber)
        {
            HoldNumber = holdNumber;
        }

        public CargoHold(byte holdNumber, byte firstBay, byte lastBay)
        {
            FirstBay = firstBay;
            LastBay = lastBay;
            HoldNumber = holdNumber;
        }
    }
}
