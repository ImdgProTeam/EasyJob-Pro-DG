using EasyJob_ProDG.Data.Info_data;


namespace EasyJob_ProDG.Model.Cargo
{

    public class Container : LocationOnBoard, IContainer, IReefer, IO.IUpdatable
    {
        #region Public properties
        // -------------- public properties -----------------------------------------

        public bool IsClosed { get; set; }
        public bool IsRf { get; set; }
        public bool ContainerTypeRecognized { get; set; }
        public byte DgCountInContainer { get; set; }

        public string Carrier { get; set; }
        public string FinalDestination { get; set; }

        public string ContainerNumber { get; set; }
        public string POD { get; set; }
        public string POL { get; set; }

        private string _type;
        public string ContainerType
        {
            get => _type;
            set
            {
                _type = value;
                UpdateContainerType();
            }
        }

        public string Remarks { get; set; }
        #endregion


        #region IUpdatable
        // -------------- IUpdatable ------------------------------------------------

        public bool IsToBeKeptInPlan { get; set; }
        public bool IsPositionLockedForChange { get; set; }
        public bool IsToImport { get; set; }
        public bool IsNotToImport { get; set; }
        public bool IsNewUnitInPlan { get; set; }
        public bool HasLocationChanged { get; set; }
        public bool HasUpdated { get; set; }
        public bool HasPodChanged { get; set; }
        public bool HasContainerTypeChanged { get; set; }

        public string LocationBeforeRestow { get; set; }
        #endregion


        #region IReefer
        // -------------- IReefer ---------------------------------------------------

        public double SetTemperature { get; set; }
        public double LoadTemperature { get; set; }
        public string Commodity { get; set; }
        public string VentSetting { get; set; }
        public string ReeferSpecial { get; set; }
        public string ReeferRemark { get; set; }

        /// <summary>
        /// Removes all reefer-related property values.
        /// </summary>
        public void ResetReefer()
        {
            SetTemperature = 0;
            Commodity = string.Empty;
            VentSetting = string.Empty;
            LoadTemperature = 0;
            ReeferSpecial = string.Empty;
            ReeferSpecial = string.Empty;
        }
        #endregion


        #region Constructor
        // -------------- public constructor ----------------------------------------

        public Container()
        {
            ContainerNumber = null;
            DgCountInContainer = 0;
            IsRf = false;
            IsUnderdeck = false;
            IsClosed = true;
            ContainerTypeRecognized = false;
        }
        #endregion


        #region Public methods
        // -------------- public methods --------------------------------------------

        /// <summary>
        /// Method converting Container into a blank Dg
        /// </summary>
        /// <returns></returns>
        public Dg ConvertToDg()
        {
            Dg dg = new Dg();
            dg.CopyContainerInfo(this);
            if (IsRf) dg.DgClass = "Reefer";
            return dg;
        }

        /// <summary>
        /// Explicit operator converting Container into a blank Dg
        /// </summary>
        /// <param name="container"></param>
        public static explicit operator Dg(Container container)
        {
            return container.ConvertToDg();
        }

        /// <summary>
        /// Updates information derived from container types dictionary
        /// </summary>
        public void UpdateContainerType()
        {
            UpdateContainerType(this);
        }

        /// <summary>
        /// Updates information derived from container types dictionary on a IContainer item
        /// </summary>
        /// <param name="a"></param>
        public static void UpdateContainerType(IContainer a)
        {
            //reset to default
            //by default IsClosed = true, TypeRecognized = false;
            a.ContainerTypeRecognized = false;
            a.IsClosed = true;

            foreach (CodesDictionary.ContainerType t in CodesDictionary.ContainerTypes)
            {
                if (t.Code != a.ContainerType) continue;
                a.IsClosed = t.Closed;
                a.ContainerTypeRecognized = true;
                break;
            }
        }

        /// <summary>
        /// Decrements dg cargo counter in container
        /// </summary>
        public void RemoveDgFromContainer()
        {
            if (DgCountInContainer > 0) DgCountInContainer--;
        }

        /// <summary>
        /// Copies basic IContainer and LocationOnBoard properties from another container
        /// </summary>
        /// <param name="container"></param>
        public void CopyContainerInfo(Container container)
        {
            Location = container.Location;
            IsUnderdeck = container.IsUnderdeck;
            DgCountInContainer = container.DgCountInContainer;
            IsRf = container.IsRf;

            _type = container._type;
            ContainerTypeRecognized = container.ContainerTypeRecognized;
            IsClosed = container.IsClosed;

            Carrier = container.Carrier;
            FinalDestination = container.FinalDestination;
            POD = container.POD;
            POL = container.POL;
        }
        #endregion
    }
}
