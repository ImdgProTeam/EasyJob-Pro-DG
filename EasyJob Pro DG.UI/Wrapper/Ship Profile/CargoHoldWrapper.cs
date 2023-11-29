using EasyJob_ProDG.Model.Transport;

namespace EasyJob_ProDG.UI.Wrapper
{
    public class CargoHoldWrapper : ModelWrapper<CargoHold>
    {
        #region Public (inherited) properties

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

        public string HoldNumber
        {
            get { return "Hold " + GetValue<byte>(); }
            set
            {
                byte result = byte.Parse(value);
                SetValue(result);
            }
        } 

        #endregion

        #region Constructors

        internal CargoHoldWrapper(CargoHold model) : base(model)
        {
        }

        internal CargoHoldWrapper(CargoHold model, byte holdNumber) : base(model)
        {
            HoldNumber = holdNumber.ToString();
            FirstBay = 0;
            LastBay = 0;
        }

        internal CargoHoldWrapper(CargoHold model, byte holdNumber, byte firstBay, byte lastBay) : base(model)
        {
            HoldNumber = holdNumber.ToString();
            FirstBay = firstBay;
            LastBay = lastBay;
        } 

        #endregion
    }
}
