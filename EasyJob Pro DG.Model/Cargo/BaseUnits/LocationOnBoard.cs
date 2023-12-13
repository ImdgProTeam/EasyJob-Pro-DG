using EasyJob_ProDG.Data;

namespace EasyJob_ProDG.Model.Cargo
{
    public class LocationOnBoard : ILocationOnBoard
    {

        // ---------- private fields -----------------------
        private string _cntrLocation; //LOC+147+BBBRRTT::5'

        // ---------- public properties -----------------------
        public byte Size { get; private set; }
        public byte Bay { get; private set; }
        public byte Row { get; private set; }
        public byte Tier { get; private set; }
        public bool IsUnderdeck { get; private set; } = false;
        public byte HoldNr { get; set; }

        /// <summary>
        /// Set: will record location and update fields row, bay, underdeck etc.
        /// </summary>
        public string Location
        {
            get => $"{Bay:D3} {Row:D2} {Tier:D2}";
            set
            {
                _cntrLocation = value.Replace(" ","");
                DefineContainerLocation();
                AssignStack();
            }
        }

        /// <summary>
        /// Represents one vertical stack of both 20" and 40" units.
        /// Includes the bay and adjacent bays.
        /// </summary>
        public byte[] Stack { get; private set; }

        // ---------- public methods -----------------------

        /// <summary>
        /// Parses string location into bay, row and tier, defines isUnderDeck and size
        /// </summary>
        private void DefineContainerLocation()
        {
            Bay = byte.Parse(_cntrLocation.Remove(_cntrLocation.Length - 4));
            Row = byte.Parse(_cntrLocation.Remove(0, _cntrLocation.Length - 4).Remove(2));
            Tier = byte.Parse(_cntrLocation.Remove(0, _cntrLocation.Length - 2));
            IsUnderdeck = Tier < ProgramDefaultSettingValues.lowestTierOnDeck;
            Size = (byte)(Bay % 2 == 0 ? 40 : 20);
        }

        /// <summary>
        /// Method assigns a stack to the unit, taking into account 20 and 40'
        /// </summary>
        private void AssignStack()
        {
            Stack = new byte[3];
            Stack[0] = (byte)(Bay - 1);
            Stack[1] = Bay;
            Stack[2] = (byte)(Bay + 1);
        }
    }
}
