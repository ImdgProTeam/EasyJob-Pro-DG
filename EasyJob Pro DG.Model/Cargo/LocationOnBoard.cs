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
            }
        }

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
    }
}
