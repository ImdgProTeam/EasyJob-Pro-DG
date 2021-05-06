using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EasyJob_Pro_DG
{
    public partial class CellPosition
    {
        // ----------- Public constructors --------------------------------------------------
        /// <summary>
        /// Default constructor defines any position
        /// </summary>
        public CellPosition()
        {
            this.HoldNr = 0;
            this.Bay = 0;
            this.Row = 99;
            this.Tier = 0;
            this.Underdeck = 0;
        }

        /// <summary>
        /// Constructor with specified position given as arguments
        /// </summary>
        /// <param name="_holdNr"></param>
        /// <param name="_bay"></param>
        /// <param name="_row"></param>
        /// <param name="_tier"></param>
        /// <param name="_underdeck"></param>
        public CellPosition(byte _holdNr, byte _bay, byte _row, byte _tier, byte _underdeck)
        {
            this.HoldNr = _holdNr;
            this.Bay = _bay;
            this.Row = _row;
            this.Tier = _tier;
            this.Underdeck = _underdeck;
        }


        //------------- Properties -------------------------
        public byte this[int index]
        {
            get { return Position[index]; }
            set
            {
                switch (index)
                {
                    case 0:
                        HoldNr = value;
                        break;
                    case 1:
                        Bay = value;
                        break;
                    case 2:
                        Row = value;
                        break;
                    case 3:
                        Tier = value;
                        break;
                    case 4:
                        Underdeck = value;
                        break;
                    default: throw new ArgumentOutOfRangeException(nameof(index), "Only indexes from 0 to 4 applicable.");

                }
            }
        }
        public byte Bay { get; set; }
        public byte Row { get; set; }
        public byte Tier { get; set; }
        public byte HoldNr { get; set; }
        public byte Underdeck { get; set; }
        public byte[] Position
        {
            get
            {

                return new byte[5] { HoldNr, Bay, Row, Tier, Underdeck };
            }
            set
            {
                //0 - not specified for all but row and underdeck
                HoldNr = value[0];
                Bay = value[1];
                //99 - not specified
                Row = value[2];
                Tier = value[3];
                //1 - underdeck, 2 - on deck, 0 - not specified
                Underdeck = value[4];
            }
        }

        //------------ Override methods -----------
        public override string ToString()
        {
            string result;
            string bayStr = this.Bay == 0 ? "any" : this.Bay.ToString();
            string rowStr = this.Row == 99 ? "any" : this.Row.ToString();
            string tierStr = this.Tier == 0 ? "any" : this.Tier.ToString();
            string underdeckStr = this.Underdeck == 1 ? "underdeck" :
                this.Underdeck == 2 ? "on deck" : "";
            if (HoldNr == 0 && this.Underdeck == 0)
                result = bayStr + rowStr + tierStr;
            else if (HoldNr == 0)
                result = "bay: " + bayStr + " row: " + rowStr + " tier: " + tierStr + " " + underdeckStr;
            else
                result = "hold: " + HoldNr.ToString() + " bay: " + bayStr + " row: " + rowStr + " tier: " + tierStr + " " +
                         underdeckStr;
            return result;
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() == typeof(Dg)) return Equals((Dg)obj);
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CellPosition)obj);
        }
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Bay.GetHashCode();
                hashCode = (hashCode * 397) ^ Row.GetHashCode();
                hashCode = (hashCode * 397) ^ Tier.GetHashCode();
                hashCode = (hashCode * 397) ^ HoldNr.GetHashCode();
                hashCode = (hashCode * 397) ^ Underdeck.GetHashCode();
                return hashCode;
            }
        }

        public bool Equals(CellPosition obj)
        {
            if (HoldNr == obj.HoldNr)
                if (this.Underdeck == obj.Underdeck)
                    if (this.Bay == obj.Bay || this.Bay == obj.Bay+1 || this.Bay == obj.Bay-1)
                        if (this.Row == obj.Row)
                            if (this.Tier == obj.Tier)
                                return true;
            return false;
        }
        public bool Equals(Dg obj)
        {
            if (HoldNr == obj.holdNr || HoldNr == 0)
                if (this.Underdeck == 1 && obj.underdeck || this.Underdeck==2 && !obj.underdeck || this[4] == 0)
                    if (this.Bay == obj.bay || this.Bay == 0 || this.Bay == obj.bay+1 || this.Bay == obj.bay-1)
                        if (this.Row == obj.row || this.Row == 99)
                            if (this.Tier == obj.tier || this.Tier == 0)
                                return true;
            return false;
        }
        public static bool operator == (CellPosition a, CellPosition b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(CellPosition a, CellPosition b)
        {
            return !(a == b);
        }
        public static bool operator ==(CellPosition a, Dg b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(CellPosition a, Dg b)
        {
            return !(a == b);
        }

        // ------------ Methods to create and work with CellPositions -------

        /// <summary>
        /// Method parses string and creates a CellPosition
        /// </summary>
        /// <param name="onePosition"></param>
        /// <returns></returns>
        public static CellPosition CreateCellPosition(string onePosition)
            {
                if (onePosition == null) return null;
                bool read = true;
                CellPosition tempCell = new CellPosition();
                int commandAddress = -1; //Shows to which position to record the number
                bool complicatedPosition = false;
                foreach (char letter in onePosition)
                    if (char.IsLetter(letter))
                    {
                        complicatedPosition = true;
                        break;
                    }

                if (complicatedPosition)
                {
                    foreach (var line in onePosition.Split(' '))
                    {
                        if (line == "") continue;
                        if (char.IsDigit(line[0]))
                        {
                            if (commandAddress == -1)
                            {
                                Output.DisplayLine("Try again");
                                read = false;
                                break;
                            }

                            byte tempPos;
                            if (byte.TryParse(line, out tempPos))
                            {
                                switch (commandAddress)
                                {
                                    case 0:
                                        tempCell.HoldNr = tempPos;
                                        break;
                                    case 1:
                                        tempCell.Bay = tempPos;
                                        break;
                                    case 2:
                                        tempCell.Row = tempPos;
                                        break;
                                    case 3:
                                        tempCell.Tier = tempPos;
                                        break;
                                    default:
                                        break;
                                }
                                continue;
                            }
                            Output.DisplayLine("Repeat again");
                            read = false;
                            continue;

                        }

                        int digitPos = 0;
                        bool digitPresent = false;
                        foreach (var t in line)
                        {
                            if (char.IsDigit(t))
                            {
                                digitPresent = true;
                                break;
                            }
                            digitPos++;
                        }

                        //if cargo hold
                        if (line.Contains("hold") || line.StartsWith("h"))
                            if (digitPresent)
                                tempCell.HoldNr = byte.Parse(line.Substring(digitPos));
                            else
                                commandAddress = 0;

                        //if underdeck
                        if (line.Contains("underdeck") || line.StartsWith("u"))
                            tempCell.Underdeck = 1;
                        //if on deck
                        else if (line.Contains("deck") || line.StartsWith("on"))
                            tempCell.Underdeck = 2;

                        //if bay
                        if (line.Contains("bay") || line.StartsWith("b"))
                            if (digitPresent)
                                tempCell.Bay = byte.Parse(line.Substring(digitPos));
                            else
                                commandAddress = 1;

                        //if row
                        if (line.Contains("row") || line.StartsWith("r"))
                            if (digitPresent)
                                tempCell.Row = byte.Parse(line.Substring(digitPos));
                            else
                                commandAddress = 2;

                        //if tier
                        if (line.Contains("tier") || line.StartsWith("t"))
                            if (digitPresent)
                                tempCell.Tier = byte.Parse(line.Substring(digitPos));
                            else
                                commandAddress = 3;
                    }
                }
                else
                {
                    string tempLine = onePosition.Replace(" ", "");
                    tempCell.Tier = byte.Parse(tempLine.Remove(0, tempLine.Length - 2));
                    tempCell.Row = byte.Parse((tempLine.Remove(0, tempLine.Length - 4)).Remove(2));
                    tempCell.Bay = byte.Parse(tempLine.Remove(tempLine.Length - 4));
                }

                if (read) return tempCell;
                else return null;
            }

        public static List<CellPosition> CreateTempCellList(string input)
            {
                var list = new List<CellPosition>();
                string[] tempArray = input.ToLower().Split(',');
                foreach (var cell in tempArray)
                {
                    list.Add(CreateCellPosition(cell));
                }
                return list;
            }

    }
}
