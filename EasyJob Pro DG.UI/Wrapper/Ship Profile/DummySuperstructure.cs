namespace EasyJob_ProDG.UI.Wrapper
{
    public class DummySuperstructure : ModelWrapper<byte>
    {
        private byte bay;

        public string Label { get; private set; }
        public byte Bay
        {
            get { return bay; }
            set
            {
                bay = value;
            }
        }

        public DummySuperstructure(byte bay) : base(bay)
        {

        }

        public DummySuperstructure(int number, byte bay) : base(bay)
        {
            Label = "Superstructure " + number;
            Bay = bay;
        }

        public delegate void DummySuperstructureChangedEventHandler(object sender);
        public static event DummySuperstructureChangedEventHandler OnDummySuperstructureChangedEventHandler = null;

    }
}
