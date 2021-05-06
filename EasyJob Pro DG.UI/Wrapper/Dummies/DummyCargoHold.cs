using EasyJob_ProDG.Model.Transport;

namespace EasyJob_ProDG.UI.Wrapper.Dummies
{
    public class DummyCargoHold
    {
        public byte FirstBay { get; set; }
        public byte LastBay { get; set; }
        public string HoldNumber { get; set; }

        internal DummyCargoHold()
        {
            HoldNumber = "";
            FirstBay = 0;
            LastBay = 0;
        }
        internal DummyCargoHold(byte holdNumber)
        {
            HoldNumber = "Hold " + holdNumber;
            FirstBay = 0;
            LastBay = 0;
        }
        internal DummyCargoHold(byte holdNumber, byte firstBay, byte lastBay)
        {
            HoldNumber = "Hold " + holdNumber;
            FirstBay = firstBay;
            LastBay = lastBay;
        }

        public CargoHold ToCargoHold()
        {
            return new CargoHold(FirstBay, LastBay);
        }
    }
}
