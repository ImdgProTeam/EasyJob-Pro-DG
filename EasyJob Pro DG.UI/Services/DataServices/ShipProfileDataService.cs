using EasyJob_ProDG.Model;
using EasyJob_ProDG.Model.Transport;
using EasyJob_ProDG.UI.Data;
using EasyJob_ProDG.UI.Wrapper;

namespace EasyJob_ProDG.UI.Services.DataServices
{
    public class ShipProfileDataService : IShipProfileDataService
    {
        // ------------------------- Private fields ---------------------------------

        readonly ICurrentProgramData _currentProgramData = CurrentProgramData.GetCurrentProgramData();

        ShipProfile _ship => _currentProgramData.GetShipProfile();
        ShipProfileWrapper _shipWrapper;

        public bool IsShipProfileDefault => _ship.IsDefault;


        // ------------------------- Public properties ------------------------------

        // ------------------------- Public methods ---------------------------------

        /// <summary>
        /// Creates Wrapper from ShipProfile
        /// </summary>
        /// <returns></returns>
        public ShipProfileWrapper CreateShipProfileWrapper()
        {
            _shipWrapper = new ShipProfileWrapper(_ship);
            return _shipWrapper;
        }

        /// <summary>
        /// TO BE IMPLEMENTED
        /// Opens a ShipProfile from disk
        /// </summary>
        public void OpenShipProfile()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Saves current ShipProfile from Wrapper and writes on disk
        /// </summary>
        public void SaveShipProfile()
        {
            //Update ship with changed values from wrapper
            UpdateLoadedShipProfile();

            //Write ShipProfile on disk
            ProgramFiles.SaveShipProfile(_ship, _shipWrapper.ProfileName);
        }


        // ------------------------- Private methods --------------------------------

        /// <summary>
        /// Copies all data from _shipWrapper to _ship
        /// </summary>
        private void UpdateLoadedShipProfile()
        {
            _shipWrapper.AcceptChanges();
        }


        // ------------------------- Constructors -----------------------------------

        /// <summary>
        /// Public constructor
        /// </summary>
        public ShipProfileDataService()
        {
        }

    }
}
