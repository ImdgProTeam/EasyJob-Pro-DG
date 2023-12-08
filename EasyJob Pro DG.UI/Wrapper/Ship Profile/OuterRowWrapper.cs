using EasyJob_ProDG.Model.Transport;

namespace EasyJob_ProDG.UI.Wrapper
{
    public class OuterRowWrapper : ModelWrapper<OuterRow>
    {
        #region Public properties

        /// <summary>
        /// States true if the <see cref="OuterRowWrapper"/> has no meaningful values.
        /// </summary>
        public bool IsEmpty => Bay == 0 && PortMost == 0 && StarboardMost == 0;

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
        #endregion

        #region Equals methods

        public bool Equals(OuterRow obj)
        {
            if (Bay == obj.Bay && StarboardMost == obj.StarboardMost && PortMost == obj.PortMost)
                return true;
            return false;
        }

        public static bool operator ==(OuterRowWrapper a, OuterRow b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(OuterRowWrapper a, OuterRow b)
        {
            return !(a == b);
        }
        #endregion

        #region Constructors

        public OuterRowWrapper(OuterRow model) : base(model)
        {

        }

        public OuterRowWrapper(byte bay, byte portM, byte stbdM) : base(new OuterRow(bay, portM, stbdM))
        {
            Bay = bay;
            PortMost = portM;
            StarboardMost = stbdM;
        } 

        #endregion
    }
}
