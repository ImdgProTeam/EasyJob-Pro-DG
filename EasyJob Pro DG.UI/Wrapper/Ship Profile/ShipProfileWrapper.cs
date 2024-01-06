using EasyJob_ProDG.Model.Transport;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace EasyJob_ProDG.UI.Wrapper
{
    public class ShipProfileWrapper : ModelChangeTrackingWrapper<ShipProfile>
    {
        #region Private fields

        private byte _originalSuperstructure2Bay;

        #endregion


        #region Bindable properties

        public string ProfileName => "Main ship profile";

        /// <summary>
        /// Indicates if the profile has been fully loaded. 
        /// It is used as a flag to avoid unnecessary updates of window properties.
        /// </summary>
        public bool IsLoaded = false;

        internal bool IsSpecialPropertiesChanged =>
               (SuperstructuresBays != null && SuperstructuresBays.Any(b => b.IsChanged))
            || (Doc != null && Doc.IsChanged);


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
            set
            {
                if (!IsLoaded) return;
                SetValue(value);
            }
        }

        /// <summary>
        /// Row '00' exists on board
        /// </summary>
        public bool Row00Exists
        {
            get => GetValue<bool>();
            set
            {
                if (!IsLoaded) return;
                SetValue(value);
            }
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

        public ChangeTrackingCollection<CargoHoldWrapper> CargoHolds { get; private set; }

        public ChangeTrackingCollection<CellPositionWrapper> LivingQuarters { get; private set; }

        public ChangeTrackingCollection<CellPositionWrapper> HeatedStructures { get; private set; }

        public ChangeTrackingCollection<CellPositionWrapper> LSA { get; private set; }
        public ObservableCollection<DummySuperstructure> SuperstructuresBays { get; private set; }

        public DOCWrapper Doc { get; private set; }

        #endregion

        #endregion


        #region Constructor logic

        public ShipProfileWrapper(ShipProfile model) : base(model)
        {
            InitializeComplexProperties();
            InitializeCollectionProperties();
            InitializeSpecialProperties();
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
            AssignEventHandlerForSeaSides();

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

            AssignRemoveEventsAndNumbersToCellPositionCollections();
        }

        private void InitializeComplexProperties()
        {
        }

        /// <summary>
        /// Initializes properties not created directly by <see cref="ModelChangeTrackingWrapper{T}"/>
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        private void InitializeSpecialProperties()
        {
            // superstructure bays
            if (Model.BaysInFrontOfSuperstructures == null)
            {
                throw new ArgumentException("BaysInFrontOfSuperstructures cannot be null");
            }

            SuperstructuresBays = new ObservableCollection<DummySuperstructure>();
            for (byte i = 0; i < Model.BaysInFrontOfSuperstructures.Count; i++)
            {
                var dummy = new DummySuperstructure(Model.BaysInFrontOfSuperstructures[i], (byte)(i + 1));
                SuperstructuresBays.Add(dummy);
                if (i == 1)
                    _originalSuperstructure2Bay = dummy.Bay;
            }

            // DOC
            if (Model.Doc == null)
            {
                throw new ArgumentException("DOC cannot be null");
            }
            Doc = new DOCWrapper(Model.Doc);
        }

        #endregion


        #region SeaSides

        /// <summary>
        /// Will remove and empty <see cref="OuterRowWrapper"/> from <see cref="SeaSides"/> after property change.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        internal void OnSeaSidePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OuterRowWrapper seaSide = sender as OuterRowWrapper;

            //remove a row if seaSide is empty except the last one
            if (seaSide.IsEmpty && SeaSides.Count > 1)
            {
                seaSide.PropertyChanged -= OnSeaSidePropertyChanged;
                SeaSides.Remove(seaSide);

                // change first value in list to all bays
                if (SeaSides[0].Bay != 0)
                    SeaSides[0].Bay = 0;
            }
        }
        private void AssignEventHandlerForSeaSides()
        {
            foreach (var seaSide in SeaSides)
            {
                seaSide.PropertyChanged -= OnSeaSidePropertyChanged;
                seaSide.PropertyChanged += OnSeaSidePropertyChanged;
            }
        }

        #endregion

        #region Add / remove cell position

        // ----- Add remove CellPosition -----

        internal void RemoveRowFromLivingQuarters(byte row)
        {
            LivingQuarters[row - 1].RemoveCellRequested -= RemoveRowFromLivingQuarters;
            LivingQuarters.RemoveAt(row - 1);
            ReNumberCellPositionWrapperList(LivingQuarters);
        }

        internal void RemoveRowFromHeatedStructures(byte row)
        {
            HeatedStructures[row - 1].RemoveCellRequested -= RemoveRowFromHeatedStructures;
            HeatedStructures.RemoveAt(row - 1);
            ReNumberCellPositionWrapperList(HeatedStructures);
        }

        internal void RemoveRowFromLSA(byte row)
        {
            LSA[row - 1].RemoveCellRequested -= RemoveRowFromLSA;
            LSA.RemoveAt(row - 1);
            ReNumberCellPositionWrapperList(LSA);
        }

        /// <summary>
        /// Sets consequtive order in provided collection of <see cref="CellPositionWrapper"/>s
        /// </summary>
        /// <param name="collection"></param>
        internal static void ReNumberCellPositionWrapperList(ICollection<CellPositionWrapper> collection)
        {
            byte i = 1;
            foreach (var cell in collection)
            {
                cell.NumberInList = i;
                i++;
            }
        }

        private void AssignRemoveEventsAndNumbersToCellPositionCollections()
        {
            byte i = 1;
            foreach (var cellPosition in LivingQuarters)
            {
                cellPosition.NumberInList = i;
                cellPosition.RemoveCellRequested -= RemoveRowFromLivingQuarters;
                cellPosition.RemoveCellRequested += RemoveRowFromLivingQuarters;
                i++;
            }
            i = 1;
            foreach (var cellPosition in HeatedStructures)
            {
                cellPosition.NumberInList = i;
                cellPosition.RemoveCellRequested -= RemoveRowFromHeatedStructures;
                cellPosition.RemoveCellRequested += RemoveRowFromHeatedStructures;
                i++;
            }
            i = 1;
            foreach (var cellPosition in LSA)
            {
                cellPosition.NumberInList = i;
                cellPosition.RemoveCellRequested -= RemoveRowFromLSA;
                cellPosition.RemoveCellRequested += RemoveRowFromLSA;
                i++;
            }
        }

        #endregion

        #region Private update methods

        /// <summary>
        /// Updates number of rows in SuperstructureBays on change of SuperstructuresNumber
        /// </summary>
        private void UpdateSuperstructuresBays()
        {
            if (SuperstructuresBays == null || SuperstructuresBays.Count == NumberOfSuperstructures || SuperstructuresBays.Count == 0)
                return;
            if (NumberOfSuperstructures > SuperstructuresBays.Count)
            {
                SuperstructuresBays.Add(new DummySuperstructure(_originalSuperstructure2Bay, (byte)(SuperstructuresBays.Count + 1)));
            }
            if (NumberOfSuperstructures < SuperstructuresBays.Count)
            {
                var index = SuperstructuresBays.Count - 1;
                SuperstructuresBays.RemoveAt(index);
            }
        }


        /// <summary>
        /// Updates number of cargo holda
        /// </summary>
        private void UpdateCargoHoldsNumber()
        {
            if (CargoHolds == null || CargoHolds.Count == NumberOfHolds || CargoHolds.Count == 0)
                return;
            if (NumberOfHolds > CargoHolds.Count)
            {
                CargoHolds.Add(new CargoHoldWrapper(new CargoHold(), NumberOfHolds));
                Doc.AddNewHold(NumberOfHolds);
            }
            if (NumberOfHolds < CargoHolds.Count)
            {
                CargoHolds.RemoveAt(CargoHolds.Count - 1);
                Doc.RemoveLastHold();
            }
        }

        #endregion

        #region Accept / reject changes

        /// <summary>
        /// Accepts changes of special properties
        /// </summary>
        internal void AcceptSpecialPropertiesChanges()
        {
            // Arrange sea-sides order
            Model.SeaSides = Model.SeaSides.OrderBy(s => s.Bay).ToList();

            // SuperstructuresBays
            foreach (var dummy in SuperstructuresBays)
            {
                dummy.AcceptChanges();
            }
            byte bay2 = SuperstructuresBays.Count > 1 ? SuperstructuresBays[1].Value : (byte)0;
            Model.SetSuperstructuresBaysProperties(SuperstructuresBays[0].Value, bay2);
            Model.UpdatePrivateProperties();

            //DOC
            Doc.AcceptChanges();

            OnPropertyChanged(nameof(IsSpecialPropertiesChanged));
        }

        /// <summary>
        /// Rejects changes of special properties
        /// </summary>
        internal void RejectSpecialPropertiesChanges()
        {
            // Restore sea-sides order
            Model.SeaSides = Model.SeaSides.OrderBy(s => s.Bay).ToList();

            // SuperstructuresBays
            foreach (var dummy in SuperstructuresBays)
                dummy.RejectChanges();
            UpdateSuperstructuresBays();

            //DOC
            Doc.RejectChanges();

            OnPropertyChanged(nameof(SuperstructuresBays));
            OnPropertyChanged(nameof(Doc));
            OnPropertyChanged(nameof(IsSpecialPropertiesChanged));
        }

        #endregion


    }
}
