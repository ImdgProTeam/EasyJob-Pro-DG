using EasyJob_ProDG.Model.Transport;

namespace EasyJob_ProDG.Model.Cargo
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Method creates ILocationOnBoard without additional functionality based on string location
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        public static ILocationOnBoard ConvertToLocationOnBoard(this string location)
        {
            if (string.IsNullOrWhiteSpace(location))
                return new LocationOnBoard();
            var locationOnBoard = new LocationOnBoard();
            locationOnBoard.Location = location;
            locationOnBoard.HoldNr = ShipProfile.DefineCargoHoldNumber(locationOnBoard.Bay);
            return locationOnBoard;
        }
    }
}
