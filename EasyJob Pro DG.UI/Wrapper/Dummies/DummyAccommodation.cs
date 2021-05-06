using EasyJob_ProDG.UI.Utility;

namespace EasyJob_ProDG.UI.Wrapper.Dummies
{
    public class DummyAccommodation : Observable
    {
        private byte bay;
        public string Label { get; set; }
        public byte Bay
        {
            get { return bay; }
            set {
                bay = value;
                OnDummyAccommodationChangedEventHandler.Invoke(this);
            }
        }

        public DummyAccommodation()
        {

        }

        public DummyAccommodation(int number, byte bay)
        {
            Label = "Accommodation " + number;
            Bay = bay;
        }

        public delegate void DummyAccommodationChangedEventHandler(object sender);
        public static event DummyAccommodationChangedEventHandler OnDummyAccommodationChangedEventHandler = null;

    }
}
