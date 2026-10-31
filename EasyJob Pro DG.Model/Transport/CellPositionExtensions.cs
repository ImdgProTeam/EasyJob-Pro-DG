namespace EasyJob_ProDG.Model.Transport
{
    public static class CellPositionExtensions
    {
        public static CellPosition Clone(this CellPosition cellPosition)
        {
            return new CellPosition()
            {
                Position = cellPosition.Position
            };
        }

        public static string DisplayText(this CellPosition cellPosition)
        {
            if (cellPosition.IsEmpty) return CellPosition.ANY_POSITION;

            string result;
            string bayStr = (cellPosition.Bay == 0 ? "" : ((cellPosition.Bay > 9 ? "" : "0") + cellPosition.Bay.ToString()));
            string rowStr = (cellPosition.Row == 99 ? "" : ((cellPosition.Row > 9 ? "" : "0") + cellPosition.Row.ToString()));
            string tierStr = (cellPosition.Tier == 0 ? "" : ((cellPosition.Tier > 9 ? "" : "0") + cellPosition.Tier.ToString()));
            string underdeckStr = cellPosition.Underdeck == 1 ? "underdeck" :
                cellPosition.Underdeck == 2 ? "on deck" : "";
            if (cellPosition.HoldNr == 0 && cellPosition.Underdeck == 0 && cellPosition.Bay > 0 && cellPosition.Tier > 0 && cellPosition.Row != 99)
                result = bayStr + rowStr + tierStr;
            else
                result = 
                    (cellPosition.HoldNr == 0 ? "" : $"hold: {cellPosition.HoldNr.ToString()} ")
                    + (cellPosition.Bay == 0 ? "" : $"bay: {bayStr} ") 
                    + (cellPosition.Row == 99 ? "" : $"row: {rowStr} ") 
                    + (cellPosition.Tier == 0 ? "" : $"tier: {tierStr} ")
                    + (cellPosition.Underdeck == 0 ? "" : $"{underdeckStr}");
            return result;
        }
    }
}
