using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.Wrapper.Dummies;
using EasyJob_ProDG.Model.Transport;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EasyJob_ProDG.UI.Wrapper
{
    public class ShipProfileWrapper : ModelWrapper<ShipProfile>
    {
        public ShipProfileWrapper() : base(new ShipProfile())
        {
            AccommodationBaysObservable = new ObservableCollection<DummyAccommodation>();
            HoldsObservable = new ObservableCollection<DummyCargoHold>();

            DataMessenger.Default.Register<RemoveRowMessage>(this, OnRemoveRowMessageReceived);
        }


        public static void OnAddNewCell(ObservableCollection<CellPositionWrapper> collection)
        {
            ReNumberCellPositionWrapperList(collection);
        }

        private void OnRemoveRowMessageReceived(RemoveRowMessage obj)
        {
            switch (obj.Collection)
            {
                case "living quarters":
                    RemoveRowFromLivingQuartersObservable(obj.Row);
                    ReNumberCellPositionWrapperList(livingQuartersObservable);
                    break;
                case "heated structures":
                    RemoveRowFromHeatedStructuresObservable(obj.Row);
                    ReNumberCellPositionWrapperList(heatedStructuresObservable);
                    break;
                case "LSA":
                    RemoveRowFromLSAObservable(obj.Row);
                    ReNumberCellPositionWrapperList(lsaObservable);
                    break;
                default:
                    break;
            }
        }
        private void RemoveRowFromLivingQuartersObservable(byte row)
        {
            livingQuartersObservable.RemoveAt(row);
        }
        private void RemoveRowFromHeatedStructuresObservable(byte row)
        {
            heatedStructuresObservable.RemoveAt(row);
        }
        private void RemoveRowFromLSAObservable(byte row)
        {
            lsaObservable.RemoveAt(row);
        }

        // --------------- Wrapping properties ----------------------------

        public string ShipName { get; set; }
        public string CallSign { get; set; }
        public byte RfMotor { get; set; }
        public bool Row00Exists { get; set; }
        public bool Passenger { get; set; }


        public byte NumberOfAccommodations
        {
            get { return GetValue<byte>(); }
            set
            {
                SetValue(value);
                UpdateAccommodationBays();
            }
        }
        internal List<byte> AccommodationBays { get; set; }
        public ObservableCollection<DummyAccommodation> AccommodationBaysObservable { get; set; }
        private void UpdateAccommodationBays()
        {
            if (AccommodationBaysObservable == null || AccommodationBaysObservable.Count == NumberOfAccommodations || AccommodationBaysObservable.Count == 0)
                return;
            if (NumberOfAccommodations > AccommodationBaysObservable.Count)
            {
                AccommodationBaysObservable.Add(new DummyAccommodation(AccommodationBaysObservable.Count + 1, 0));
            }
            if (NumberOfAccommodations < AccommodationBaysObservable.Count)
            {
                AccommodationBaysObservable.RemoveAt(AccommodationBaysObservable.Count - 1);
            }
        }

        public byte NumberOfHolds
        {
            get { return GetValue<byte>(); }
            set
            {
                SetValue(value);
                UpdateCargoHoldsNumber();
            }
        }
        public ObservableCollection<DummyCargoHold> HoldsObservable { get; set; }

        //
        //ADD LOGIC FOR DOC
        //
        private void UpdateCargoHoldsNumber()
        {
            if (HoldsObservable == null || HoldsObservable.Count == NumberOfHolds || HoldsObservable.Count == 0)
                return;
            if (NumberOfHolds > HoldsObservable.Count)
            {
                HoldsObservable.Add(new DummyCargoHold(NumberOfHolds));
                DocObservable.AddNewHold(NumberOfHolds);
            }
            if (NumberOfHolds < HoldsObservable.Count)
            {
                HoldsObservable.RemoveAt(HoldsObservable.Count - 1);
                //DocObservable.RemoveLastHold();
            }
        }


        internal List<OuterRow> SeaSides { get; set; }
        private ObservableCollection<OuterRowWrapper> seaSidesObservable;
        public ObservableCollection<OuterRowWrapper> SeaSidesObservable
        {
            get
            {
                if (seaSidesObservable == null)
                {
                    seaSidesObservable = new ObservableCollection<OuterRowWrapper>();
                    foreach (var outerRow in SeaSides)
                    {
                        OuterRowWrapper outerRowWrapper = new OuterRowWrapper();
                        outerRowWrapper.ConvertToWrapper(outerRow);
                        seaSidesObservable.Add(outerRowWrapper);
                    }
                }
                return seaSidesObservable;
            }
            set
            {
                seaSidesObservable = value;
            }
        }

        internal List<CellPosition> LivingQuartersList { get; set; }
        private ObservableCollection<CellPositionWrapper> livingQuartersObservable;
        public ObservableCollection<CellPositionWrapper> LivingQuartersObservable
        {
            get
            {
                if (livingQuartersObservable == null)
                {
                    livingQuartersObservable = new ObservableCollection<CellPositionWrapper>();
                    byte i = 1;
                    CellPositionWrapper newCellWrapper;
                    foreach (var cell in LivingQuartersList)
                    {
                        newCellWrapper = new CellPositionWrapper(cell);
                        newCellWrapper.NumberInList = i;
                        livingQuartersObservable.Add(newCellWrapper);
                        i++;
                    }
                }
                return livingQuartersObservable;
            }
            set
            {
                livingQuartersObservable = value;
            }
        }

        internal List<CellPosition> HeatedStructuresList { get; set; }
        private ObservableCollection<CellPositionWrapper> heatedStructuresObservable;
        public ObservableCollection<CellPositionWrapper> HeatedStructuresObservable
        {
            get
            {
                if (heatedStructuresObservable == null)
                {
                    heatedStructuresObservable = new ObservableCollection<CellPositionWrapper>();
                    byte i = 1;
                    CellPositionWrapper newCellWrapper;
                    foreach (var cell in HeatedStructuresList)
                    {
                        newCellWrapper = new CellPositionWrapper(cell);
                        newCellWrapper.NumberInList = i;
                        heatedStructuresObservable.Add(newCellWrapper);
                        i++;
                    }
                }
                return heatedStructuresObservable;
            }
            set
            {
                heatedStructuresObservable = value;
            }
        }

        internal List<CellPosition> LSAList { get; set; }
        private ObservableCollection<CellPositionWrapper> lsaObservable;
        public ObservableCollection<CellPositionWrapper> LSAObservable
        {
            get
            {
                if (lsaObservable == null)
                {
                    lsaObservable = new ObservableCollection<CellPositionWrapper>();
                    byte i = 1;
                    CellPositionWrapper newCellWrapper;
                    foreach (var cell in LSAList)
                    {
                        newCellWrapper = new CellPositionWrapper(cell);
                        newCellWrapper.NumberInList = i;
                        lsaObservable.Add(newCellWrapper);
                        i++;
                    }
                }
                return lsaObservable;
            }
            set
            {
                lsaObservable = value;
            }
        }
        //private void ReNumberLivingQuartersObservable()
        //{
        //    byte i = 1;
        //    foreach (var cell in livingQuartersObservable)
        //    {
        //        cell.NumberInList = i;
        //        i++;
        //    }
        //}
        private static void ReNumberCellPositionWrapperList(ObservableCollection<CellPositionWrapper> collection)
        {
            byte i = 1;
            foreach (var cell in collection)
            {
                cell.NumberInList = i;
                i++;
            }
        }

        public DOCWrapper DocObservable
        {
            get; 
            set;
        }

        public string ErrorList { get; set; }


        public static List<string> MotorFacingList
        {
            get { return ShipProfile.MotorFacingList; }
        }

        public string ProfileName { get; set; }

        public void CancelChanges()
        {

        }

        // --------------- Events ---------------------------------------


    }
}
