using EasyJob_ProDG.Model.Transport;

namespace EasyJob_ProDG.UI.Wrapper
{
    public class CargoHoldWrapper : ModelWrapper<CargoHold>
    {
        public byte FirstBay
        {
            get { return GetValue<byte>(); }
            set { SetValue(value); }
        }

        public byte LastBay
        {
            get { return GetValue<byte>(); }
            set { SetValue(value); }
        }

        public string HoldNumber { get; private set; }

        internal CargoHoldWrapper(CargoHold model) : base(model) 
        {
            HoldNumber = "";
            FirstBay = 0;
            LastBay = 0;
        }
        internal CargoHoldWrapper(CargoHold model, byte holdNumber) : base (model)
        {
            HoldNumber = "Hold " + holdNumber;
            FirstBay = 0;
            LastBay = 0;
        }
        internal CargoHoldWrapper(CargoHold model, byte holdNumber, byte firstBay, byte lastBay) : base(model)
        {
            HoldNumber = "Hold " + holdNumber;
            FirstBay = firstBay;
            LastBay = lastBay;
        }

        public CargoHold ToCargoHold()
        {
            return Model;
        }
    }
}
