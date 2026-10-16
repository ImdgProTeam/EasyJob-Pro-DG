namespace EasyJob_ProDG.Model.Transport
{
    /// <summary>
    /// Extends <see cref="CellPosition"/> to include functionality to change it from string
    /// </summary>
    public static class CellPositionTryChange
    {
        // ------------ Methods to create and work with CellPositions -------

        /// <summary>
        /// Tryes to change <see cref="CellPosition"/> from string.
        /// Remains <see cref="CellPosition"/> empty if not succeeded to change.
        /// </summary>
        /// <param name="cellPosition"></param>
        /// <param name="onePosition"></param>
        /// <returns>True if succeeded to change.</returns>
        public static bool TryChangeCellPosition(this CellPosition cellPosition, string onePosition)
        {
            bool isSuccessful;

            try
            {
                cellPosition.ChangeCellPosition(onePosition);
                isSuccessful = true;
            }
            catch
            {
                isSuccessful = false;
                cellPosition.ChangeCellPosition(new CellPosition());
            }
            return isSuccessful;
        }


        #region Private methods

        /// <summary>
        /// Method parses string and creates a CellPosition. Returns null in case of null or not recognized string.
        /// </summary>
        /// <param name="onePosition"></param>
        /// <returns>New CellPosition or Null in case of null or not recognized string.</returns>
        private static CellPosition CreateCellPosition(string onePosition)
        {
            if (onePosition == null) return null;
            bool read = true;
            CellPosition tempCell = new CellPosition();

            if(string.Equals(onePosition, CellPosition.ANY_POSITION))
                return tempCell;

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
                foreach (var line in onePosition.Split([' ',',']))
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

        private static void ChangeCellPosition(this CellPosition cellPosition, string newPosition)
        {
            CellPosition tempCell = CreateCellPosition(newPosition);
            cellPosition.ChangeCellPosition(tempCell);
        }

        private static void ChangeCellPosition(this CellPosition cellPosition, CellPosition newPosition)
        {
            cellPosition.Bay = newPosition.Bay;
            cellPosition.Row = newPosition.Row;
            cellPosition.Tier = newPosition.Tier;
            cellPosition.HoldNr = newPosition.HoldNr;
            cellPosition.Underdeck = newPosition.Underdeck;
        }

        #endregion

    }
}
