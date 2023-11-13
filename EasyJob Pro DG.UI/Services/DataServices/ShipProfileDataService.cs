using EasyJob_ProDG.Model;
using EasyJob_ProDG.Model.Transport;
using EasyJob_ProDG.UI.Data;
using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.Wrapper;
using System.Collections.Generic;

namespace EasyJob_ProDG.UI.Services.DataServices
{
    public class ShipProfileDataService : IShipProfileDataService
    {
        // ------------------------- Private fields ---------------------------------

        readonly ICurrentProgramData _currentProgramData = CurrentProgramData.GetCurrentProgramData();
        ShipProfile _ship;
        ShipProfileWrapper _shipWrapper;


        // ------------------------- Public properties ------------------------------

        // ------------------------- Public methods ---------------------------------

        /// <summary>
        /// Creates Wrapper from ShipProfile
        /// </summary>
        /// <returns></returns>
        public ShipProfileWrapper CreateShipProfileWrapper()
        {
            _shipWrapper = new ShipProfileWrapper();
            _ship = GetShipProfile();

            _shipWrapper.ShipName = _ship.ShipName;
            _shipWrapper.CallSign = _ship.CallSign;
            _shipWrapper.NumberOfHolds = _ship.NumberOfHolds;
            _shipWrapper.RfMotor = _ship.RfMotor;
            _shipWrapper.Row00Exists = _ship.Row00Exists;
            _shipWrapper.Passenger = _ship.Passenger;
            _shipWrapper.HoldsObservable = ClassConverters.UpgradeToCollection(_ship.Holds);
            _shipWrapper.NumberOfSuperstructures = _ship.NumberOfSuperstructures;
            _shipWrapper.AccommodationBays = _ship.BaysInFrontOfSuperstructures;
            _shipWrapper.SeaSides = _ship.SeaSides;
            _shipWrapper.LivingQuartersList = _ship.LivingQuarters;
            _shipWrapper.HeatedStructuresList = _ship.HeatedStructures;
            _shipWrapper.LSAList = _ship.LSA;
            _shipWrapper.DocObservable = new DOCWrapper(_ship.Doc);
            _shipWrapper.DocObservable.SetDOCTableFromModel();
            _shipWrapper.ErrorList = _ship.ErrorList;

            //TO BE REPLACED
            _shipWrapper.ProfileName = "To be replaced";

            return _shipWrapper;
        }

        /// <summary>
        /// Getter to current ShipProfile
        /// </summary>
        /// <returns>Current ShipProfile</returns>
        public ShipProfile GetShipProfile()
        {
            return _currentProgramData.GetShipProfile();
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
            EasyJob_ProDG.Data.ProgressBarReporter.ReportPercentage = 20;

            //Write ShipProfile on disk
            ProgramFiles.SaveShipProfile(_ship, _shipWrapper.ProfileName);
            EasyJob_ProDG.Data.ProgressBarReporter.ReportPercentage = 50;

            //Call full data re-check in CargoPlan
            _currentProgramData.FullDataReCheck();
            EasyJob_ProDG.Data.ProgressBarReporter.ReportPercentage = 80;

            //Sending notification message to DataMessenger
            DataMessenger.Default.Send(new ShipProfileWrapperMessage(), "ship profile saved");
        }

        /// <summary>
        /// Defines to what CargoHold belongs the bay
        /// </summary>
        /// <param name="bay">Bay in query</param>
        /// <returns>CargoHold number</returns>
        public byte DefineCargoHoldNumber(byte bay)
        {
            return ShipProfile.DefineCargoHoldNumber(bay);
        }
        

        // ------------------------- Private methods --------------------------------

        /// <summary>
        /// Copies all data from _shipWrapper to _ship
        /// </summary>
        private void UpdateLoadedShipProfile()
        {
            _ship.ShipName = _shipWrapper.ShipName;
            _ship.CallSign = _shipWrapper.CallSign;
            _ship.NumberOfHolds = _shipWrapper.NumberOfHolds;
            _ship.Holds = ClassConverters.DowngradeCollectionToList(_shipWrapper.HoldsObservable);

            _ship.RfMotor = _shipWrapper.RfMotor;
            _ship.Row00Exists = _shipWrapper.Row00Exists;
            _ship.Passenger = _shipWrapper.Passenger;


            _ship.NumberOfSuperstructures = _shipWrapper.NumberOfSuperstructures;
            _ship.BaysInFrontOfSuperstructures?.Clear();
            _ship.BaysSurroundingSuperstructure?.Clear();
            foreach (var dummy in _shipWrapper.AccommodationBaysObservable)
            {
                //TODO: Review for the case of two accommodations
                _ship.SetSuperstructuresBaysProperties(dummy.Bay);
            }

            _ship.SeaSides = new List<OuterRow>();
            foreach (var row in _shipWrapper.SeaSidesObservable)
            {
                _ship.SeaSides.Add(row.ToOuterRow());
            }

            _ship.LivingQuarters = new List<CellPosition>();
            foreach (var cell in _shipWrapper.LivingQuartersObservable)
            {
                if (cell.IsEmpty()) continue;
                _ship.LivingQuarters.Add(cell.ToCellPosition());
            }
            _ship.HeatedStructures = new List<CellPosition>();
            foreach (var cell in _shipWrapper.HeatedStructuresObservable)
            {
                if (cell.IsEmpty()) continue;
                _ship.HeatedStructures.Add(cell.ToCellPosition());
            }
            _ship.LSA = new List<CellPosition>();
            foreach (var cell in _shipWrapper.LSAObservable)
            {
                if (cell.IsEmpty()) continue;
                _ship.LSA.Add(cell.ToCellPosition());
            }
            _ship.UpdatePrivateProperties();

            _ship.Doc = new DOC(_ship.NumberOfHolds);
            for (byte h = 0; h < _ship.Doc.NumberOfRows; h++)
                for (byte c = 0; c < _ship.Doc.NumberOfClasses; c++)
                {
                    _ship.Doc.DOCtable[h, c] = _shipWrapper.DocObservable.DOCTable[c].Row[h].Value;
                }
            _ship.ErrorList = _shipWrapper.ErrorList;

        }
        

        // ------------------------- Constructors -----------------------------------

        /// <summary>
        /// Public constructor
        /// </summary>
        public ShipProfileDataService()
        {
            _currentProgramData.ShipProfileDataService = this;
        }

    }
}
