using System;
using System.Collections.Generic;
using EasyJob_ProDG.Model.Cargo;

namespace EasyJob_ProDG.Model.Transport
{
    public class CellPosition
    {
        // !!!Check for null reference to be implemented for override operators section!!!


        // ----------- Public constructors --------------------------------------------------
        /// <summary>
        /// Default constructor defines any position
        /// </summary>
        public CellPosition()
        {
            HoldNr = 0;
            Bay = 0;
            Row = 99;
            Tier = 0;
            Underdeck = 0;
        }

        /// <summary>
        /// Constructor with specified position given as arguments
        /// </summary>
        /// <param name="holdNr"></param>
        /// <param name="bay"></param>
        /// <param name="row"></param>
        /// <param name="tier"></param>
        /// <param name="underdeck"></param>
        public CellPosition(byte holdNr, byte bay, byte row, byte tier, byte underdeck)
        {
            HoldNr = holdNr;
            Bay = bay;
            Row = row;
            Tier = tier;
            Underdeck = underdeck;
        }


        //------------- Properties -------------------------
        public byte this[byte index]
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

                return new[] { HoldNr, Bay, Row, Tier, Underdeck };
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

        /// <summary>
        /// True if no any value assigned to the CellPosition.
        /// If any value is set (different from default), it is false.
        /// </summary>
        public bool IsEmpty => this[0] == 0 && this[1] == 0 && this[2] == 99 && this[3] == 0 && this[4] == 0;


        // ------------ Methods to create and work with CellPositions -------

        public bool TryChangeCellPosition(string onePosition)
        {
            bool isSuccessful;
            
            try
            {
                ChangeCellPosition(onePosition);
                isSuccessful = true;
            }
            catch 
            { 
                isSuccessful = false;
                ChangeCellPosition(new CellPosition());
            }
            return isSuccessful;
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


        #region Private methods

        /// <summary>
        /// Method parses string and creates a CellPosition. Returns null in case of null or not recognized string.
        /// </summary>
        /// <param name="onePosition"></param>
        /// <returns></returns>
        private static CellPosition CreateCellPosition(string onePosition)
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

        private void ChangeCellPosition(string newPosition)
        {
            CellPosition tempCell = CreateCellPosition(newPosition);
            ChangeCellPosition(tempCell);
        }

        private void ChangeCellPosition(CellPosition newPosition)
        {
            Bay = newPosition.Bay;
            Row = newPosition.Row;
            Tier = newPosition.Tier;
            HoldNr = newPosition.HoldNr;
            Underdeck = newPosition.Underdeck;
        } 
        #endregion

        #region System override methods
        //------------ System override methods -----------
        public override string ToString()
        {
            string result;
            string bayStr = (Bay == 0 ? "any" : ((Bay > 9 ? "" : "0") + Bay.ToString()));
            string rowStr = (Row == 99 ? "any" : ((Row > 9 ? "" : "0") + Row.ToString()));
            string tierStr = (Tier == 0 ? "any" : ((Tier > 9 ? "" : "0") + Tier.ToString()));
            string underdeckStr = this.Underdeck == 1 ? "underdeck" :
                this.Underdeck == 2 ? "on deck" : "";
            if (HoldNr == 0 && Underdeck == 0 && Bay > 0 && Tier > 0 && Row != 99)
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
            if (obj.GetType() != GetType()) return false;
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
                if (Underdeck == obj.Underdeck)
                    if (Bay == obj.Bay || Bay == obj.Bay + 1 || Bay == obj.Bay - 1)
                        if (Row == obj.Row)
                            if (Tier == obj.Tier)
                                return true;
            return false;
        }
        public bool Equals(ILocationOnBoard obj)
        {
            if (HoldNr == obj.HoldNr || HoldNr == 0)
                if (Underdeck == 1 && obj.IsUnderdeck || Underdeck == 2 && !obj.IsUnderdeck || this[4] == 0)
                    if (Bay == obj.Bay || Bay == 0 || Bay == obj.Bay + 1 || Bay == obj.Bay - 1)
                        if (Row == obj.Row || Row == 99)
                            if (Tier == obj.Tier || Tier == 0)
                                return true;
            return false;
        }
        public static bool operator ==(CellPosition a, CellPosition b)
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
        #endregion

    }
}
