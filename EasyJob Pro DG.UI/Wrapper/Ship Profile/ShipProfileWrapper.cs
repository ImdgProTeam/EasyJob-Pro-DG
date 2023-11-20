using EasyJob_ProDG.Model.Transport;
using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EasyJob_ProDG.UI.Wrapper
{
    public class ShipProfileWrapper : ModelWrapper<ShipProfile>
    {
        #region Bindable properties

        public string ProfileName => "Main ship profile";


        // --------------- Wrapping properties ----------------------------
        #region Tab Main
        // ----- Tab Main -----

        /// <summary>
        /// Vessel name
        /// </summary>
        public string ShipName
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        /// <summary>
        /// Vessel call sign
        /// </summary>
        public string CallSign
        {
            get => GetValue<string>();
            set => SetValue(value);
        }

        /// <summary>
        /// Is she a passenger ship
        /// </summary>
        public bool Passenger
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        /// <summary>
        /// Row '00' exists on board
        /// </summary>
        public bool Row00Exists
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        /// <summary>
        /// Reefer motor facing 
        /// (byte number corresponding to <see cref="ShipProfile.MotorFacing"/> enum
        /// </summary>
        public byte RfMotor
        {
            get => GetValue<byte>();
            set => SetValue(value);
        }

        /// <summary>
        /// List of possible options for reefers facing.
        /// List to be used in combobox list items.
        /// </summary>
        public List<string> MotorFacingList => GetValue<List<string>>();


        /// <summary>
        /// Number of superstructures on the vessel
        /// </summary>
        public byte NumberOfSuperstructures
        {
            get { return GetValue<byte>(); }
            set
            {
                SetValue(value);
                UpdateSuperstructuresBays();
            }
        }

        /// <summary>
        /// Number of cargo holds on the vessel
        /// </summary>
        public byte NumberOfHolds
        {
            get { return GetValue<byte>(); }
            set
            {
                SetValue(value);
                UpdateCargoHoldsNumber();
                OnPropertyChanged(nameof(CargoHolds));
            }
        }

        #endregion

        #region Other tabs
        // ----- Other tabs -----

        public ChangeTrackingCollection<OuterRowWrapper> SeaSides { get; private set; }

        public ChangeTrackingCollection<DummySuperstructure> SuperstructuresBays { get; private set; }

        public ChangeTrackingCollection<CargoHoldWrapper> CargoHolds { get; private set; }

        public ChangeTrackingCollection<CellPositionWrapper> LivingQuarters { get; private set; }

        public ChangeTrackingCollection<CellPositionWrapper> HeatedStructures { get; private set; }

        public ChangeTrackingCollection<CellPositionWrapper> LSA { get; private set; }

        public DOCWrapper Doc { get; private set; }
        
        #endregion

        #endregion


        #region Constructor logic

        public ShipProfileWrapper(ShipProfile model) : base(model)
        {
            InitializeComplexProperties();
            InitializeCollectionProperties();

            RegisterInMessenger();
        }

        private void RegisterInMessenger()
        {
            DataMessenger.Default.UnregisterAll(this);
            DataMessenger.Default.Register<RemoveRowMessage>(this, OnRemoveRowMessageReceived);
        }

        private void InitializeCollectionProperties()
        {
            // sea sides
            if (Model.SeaSides == null)
            {
                throw new ArgumentException("SeaSides cannot be null");
            }
            SeaSides = new ChangeTrackingCollection<OuterRowWrapper>(Model.SeaSides.Select(s => new OuterRowWrapper(s)));
            RegisterCollection(SeaSides, Model.SeaSides);

            // superstructure bays
            if (Model.BaysInFrontOfSuperstructures == null)
            {
                throw new ArgumentException("BaysInFrontOfSuperstructures cannot be null");
            }
            SuperstructuresBays = new ChangeTrackingCollection<DummySuperstructure>(Model.BaysInFrontOfSuperstructures.Select(b => new DummySuperstructure(b)));
            RegisterCollection(SuperstructuresBays, Model.BaysSurroundingSuperstructure);

            // cargo holds
            if (Model.CargoHolds == null)
            {
                throw new ArgumentException("CargoHolds cannot be null");
            }
            CargoHolds = new ChangeTrackingCollection<CargoHoldWrapper>(Model.CargoHolds.Select(h => new CargoHoldWrapper(h)));
            RegisterCollection(CargoHolds, Model.CargoHolds);

            // living quarters
            if (Model.LivingQuarters == null)
            {
                throw new ArgumentException("LivingQuarters cannot be null");
            }
            LivingQuarters = new ChangeTrackingCollection<CellPositionWrapper>(Model.LivingQuarters.Select(c => new CellPositionWrapper(c)));
            RegisterCollection(LivingQuarters, Model.LivingQuarters);

            // heated structures
            if (Model.HeatedStructures == null)
            {
                throw new ArgumentException("HeatedStructures cannot be null");
            }
            HeatedStructures = new ChangeTrackingCollection<CellPositionWrapper>(Model.HeatedStructures.Select(c => new CellPositionWrapper(c)));
            RegisterCollection(HeatedStructures, Model.HeatedStructures);

            // life-saving appliances
            if (Model.LSA == null)
            {
                throw new ArgumentException("LSA cannot be null");
            }
            LSA = new ChangeTrackingCollection<CellPositionWrapper>(Model.LSA.Select(c => new CellPositionWrapper(c)));
            RegisterCollection(LSA, Model.LSA);

        }

        private void InitializeComplexProperties()
        {
            if (Model.Doc == null)
            {
                throw new ArgumentException("DOC cannot be null");
            }
            Doc = new DOCWrapper(Model.Doc);
            RegisterComplexProperty(Doc);
        }

        #endregion



        // ----- Add remove CellPosition -----

        public static void OnAddNewCell(ICollection<CellPositionWrapper> collection)
        {
            ReNumberCellPositionWrapperList(collection);
        }
        private void OnRemoveRowMessageReceived(RemoveRowMessage obj)
        {
            switch (obj.Collection)
            {
                case "living quarters":
                    RemoveRowFromLivingQuartersObservable(obj.Row);
                    ReNumberCellPositionWrapperList(LivingQuarters);
                    break;
                case "heated structures":
                    RemoveRowFromHeatedStructuresObservable(obj.Row);
                    ReNumberCellPositionWrapperList(HeatedStructures);
                    break;
                case "LSA":
                    RemoveRowFromLSAObservable(obj.Row);
                    ReNumberCellPositionWrapperList(LSA);
                    break;
                default:
                    break;
            }
        }
        private void RemoveRowFromLivingQuartersObservable(byte row)
        {
            LivingQuarters.RemoveAt(row);
        }
        private void RemoveRowFromHeatedStructuresObservable(byte row)
        {
            HeatedStructures.RemoveAt(row);
        }
        private void RemoveRowFromLSAObservable(byte row)
        {
            LSA.RemoveAt(row);
        }
        private static void ReNumberCellPositionWrapperList(ICollection<CellPositionWrapper> collection)
        {
            byte i = 1;
            foreach (var cell in collection)
            {
                cell.NumberInList = i;
                i++;
            }
        }





        /// <summary>
        /// Updates number of rows in SuperstructureBays on change of SuperstructuresNumber
        /// </summary>
        private void UpdateSuperstructuresBays()
        {
            if (SuperstructuresBays == null || SuperstructuresBays.Count == NumberOfSuperstructures || SuperstructuresBays.Count == 0)
                return;
            if (NumberOfSuperstructures > SuperstructuresBays.Count)
            {
                SuperstructuresBays.Add(new DummySuperstructure(SuperstructuresBays.Count + 1, 0));
            }
            if (NumberOfSuperstructures < SuperstructuresBays.Count)
            {
                SuperstructuresBays.RemoveAt(SuperstructuresBays.Count - 1);
            }
        }


        /// <summary>
        /// Updates number of cargo holda
        /// </summary>
        private void UpdateCargoHoldsNumber()
        {
            //if (CargoHolds == null || CargoHolds.Count == NumberOfHolds || CargoHolds.Count == 0)
            //    return;
            //if (NumberOfHolds > CargoHolds.Count)
            //{
            //    CargoHolds.Add(new CargoHoldWrapper(NumberOfHolds));
            //    Doc.AddNewHold(NumberOfHolds);
            //}
            //if (NumberOfHolds < CargoHolds.Count)
            //{
            //    CargoHolds.RemoveAt(CargoHolds.Count - 1);
            //    //Doc.RemoveLastHold();
            //}
        }






        public string ErrorList { get; set; }




        // --------------- Events ---------------------------------------


    }
}
