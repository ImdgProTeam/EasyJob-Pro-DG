namespace EasyJob_ProDG.Model.Cargo
{
    public static class LocationOnBoardExtensions
    {
        /// <summary>
        /// Allows to modify separately bay, row or tier (or any combination of those) in <see cref="ILocationOnBoard"/> instance
        /// </summary>
        /// <param name="location"><see cref="ILocationOnBoard"/> to be changed</param>
        /// <param name="bay">'0' - will not be changed</param>
        /// <param name="row">null - will not be changed</param>
        /// <param name="tier">null - will not be changed</param>
        /// <returns>Modified <see cref="ILocationOnBoard"/> instance</returns>
        public static ILocationOnBoard SetLocation (this ILocationOnBoard location, byte bay, byte? row = null, byte? tier = null)
        {
            string position = location.Location.Replace(" ","");
            string resultPosition = position;

            if (bay != 0) resultPosition = position.Remove(0, 3).Insert(0, $"{bay:D3}");
            if (row!= null) resultPosition = position.Remove(3,2).Insert(3,$"{row:D2}");
            if (tier != null) resultPosition = position.Remove(5, 2).Insert(5, $"{tier:D2}");
            location.Location = resultPosition;
            return location;
        }

        /// <summary>
        /// Modifies separately bay, row or tier (or any combination of those) in <see cref="ILocationOnBoard"/> instance.
        /// </summary>
        /// <param name="location"><see cref="ILocationOnBoard"/> remains unchanged</param>
        /// <param name="bay">'0' - will not be changed</param>
        /// <param name="row">null - will not be changed</param>
        /// <param name="tier">null - will not be changed</param>
        /// <returns>Modified Location as string.</returns>
        public static string GetModifiedLocation(this ILocationOnBoard location, byte bay, byte? row = null, byte? tier = null)
        {
            string position = location.Location.Replace(" ", "");
            string resultPosition = position;
            if (bay != 0) resultPosition = position.Remove(0, 3).Insert(0, $"{bay:D3}");
            if (row != null) resultPosition = position.Remove(3, 2).Insert(3, $"{row:D2}");
            if (tier != null) resultPosition = position.Remove(5, 2).Insert(5, $"{tier:D2}");
            return resultPosition;
        }

        /// <summary>
        /// Creates new <see cref="ILocationOnBoard"/> from elements
        /// </summary>
        /// <param name="bay"></param>
        /// <param name="row"></param>
        /// <param name="tier"></param>
        /// <returns></returns>
        public static ILocationOnBoard CreateLocation(byte bay, byte row, byte tier)
        {
            string position = $"{bay:000}{row:00}{tier:00}";
            var location = new LocationOnBoard();
            location.Location = position;
            return location;
        }
    }
}
