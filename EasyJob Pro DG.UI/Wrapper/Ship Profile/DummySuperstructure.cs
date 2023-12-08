namespace EasyJob_ProDG.UI.Wrapper
{
    public class DummySuperstructure : ModelByteDummy
    {
        public byte Number => Index1;
        public string Label { get; private set; }
        public byte Bay
        {
            get { return Value; }
            set
            {
                Value = value;
            }
        }

        public DummySuperstructure(byte bay, byte number) : base(bay, number)
        {
            Label = "Superstructure " + number;
        }
    }
}
