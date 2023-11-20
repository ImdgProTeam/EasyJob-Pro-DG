using EasyJob_ProDG.Model.Transport;

namespace EasyJob_ProDG.UI.Wrapper
{
    public class OuterRowWrapper : ModelWrapper<OuterRow>
    {
        public OuterRowWrapper(OuterRow model) : base(model)
        {

        }

        public OuterRowWrapper(byte bay, byte portM, byte stbdM) : base(new OuterRow())
        {
            Bay = bay;
            PortMost = portM;
            StarboardMost = stbdM;
        }

        public byte StarboardMost
        {
            get
            {
                return GetValue<byte>();
            }
            set
            {
                SetValue(value);
            }
        }
        public byte PortMost
        {
            get
            {
                return GetValue<byte>();
            }
            set
            {
                SetValue(value);
            }
        }
        public byte Bay
        {
            get
            {
                return GetValue<byte>();
            }
            set
            {
                SetValue(value);
            }
        }

        public OuterRow ToOuterRow()
        {
            return Model;
        }

        public bool Equals(OuterRow obj)
        {
            if (Bay == obj.Bay && StarboardMost == obj.StarboardMost && PortMost == obj.PortMost)
                return true;
            return false;
        }
        public static bool operator == (OuterRowWrapper a, OuterRow b)
        {
            return a.Equals(b);
        }
        public static bool operator != (OuterRowWrapper a, OuterRow b)
        {
            return !(a == b);
        }

        // --------------- Events ---------------------------------------

        public delegate void OuterRowChangedEventHandler(object sender);
        public static event OuterRowChangedEventHandler OnOuterRowChangedEventHandler = null;
    }
}
