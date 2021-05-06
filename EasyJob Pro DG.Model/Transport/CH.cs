namespace EasyJob_ProDG.Model.Transport
{
    public class CargoHold
    {
        public byte FirstBay { get; set; }
        public byte LastBay { get; set; }

        public CargoHold()
        {
            FirstBay = 0;
            LastBay = 0;
        }
        public CargoHold(byte fbay, byte lbay)
        {
            FirstBay = fbay;
            LastBay = lbay;
        }
    }
}
