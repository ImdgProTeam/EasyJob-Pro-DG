using EasyJob_ProDG.Model.Cargo;

namespace EasyJob_ProDG.Model.Transport
{
    /// <summary>
    /// Class describes the most port and the most starboard side row (seaside) for each bay individually if value 'bay' is set. If 'bay' is set as '0' or not defined then the seaside rows will apply to all bays which are not separately specified.
    /// </summary>
    public class OuterRow
    {
        public byte StarboardMost { get; set; }
        public byte PortMost { get; set; }
        public byte Bay { get; set; }

        public OuterRow() { }

        public int this[byte index]
        {
            get
            {
                switch (index)
                {
                    case 0:
                        return Bay;
                    case 1:
                        return PortMost;
                    case 2:
                        return StarboardMost;
                }
                return 0;
            }
            set
            {
                switch (index)
                {
                    case 0:
                        Bay = (byte)value;
                        break;
                    case 1:
                        PortMost = (byte)value;
                        break;
                    case 2:
                        StarboardMost = (byte)value;
                        break;
                }
            }
        }

        public OuterRow(byte bay, byte portM, byte stbdM)
        {
            Bay = bay;
            PortMost = portM;
            StarboardMost = stbdM;
        }

        public OuterRow(byte portM, byte stbdM)
        {
            Bay = 0;
            PortMost = portM;
            StarboardMost = stbdM;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() == typeof(Dg)|| obj.GetType() == typeof(Container) || obj.GetType() == typeof(ILocationOnBoard)) 
                return Equals((ILocationOnBoard)obj);
            if (obj.GetType() != GetType()) return false;
            return Equals((OuterRow)obj);
        }

        public bool Equals(OuterRow obj)
        {
            if (Bay == obj.Bay || Bay == 0 || obj.Bay == 0)
                if (StarboardMost == obj.StarboardMost && PortMost == obj.PortMost)
                    return true;
            return false;
        }

        public bool Equals(ILocationOnBoard obj)
        {
            if (Bay == obj.Bay || Bay == obj.Bay + 1 || Bay == obj.Bay - 1 || Bay == 0)
                if (PortMost == obj.Row || StarboardMost == obj.Row)
                    return true;
            return false;
        }

        public static bool operator ==(OuterRow a, OuterRow b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(OuterRow a, OuterRow b)
        {
            return !(a == b);
        }
        public static bool operator ==(OuterRow a, ILocationOnBoard b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(OuterRow a, ILocationOnBoard b)
        {
            return !(a == b);
        }
    }
}
