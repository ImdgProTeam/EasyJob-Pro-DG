namespace EasyJob_ProDG.Model.Cargo
{

    public class Container : ContainerAbstract, IReefer
    {
        #region Public properties
        // -------------- public properties -----------------------------------------

        public byte DgCountInContainer { get; set; }
        public bool ContainsDgCargo => DgCountInContainer > 0;
        public string Remarks { get; set; }

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
            DgCountInContainer = container.DgCountInContainer;
            IsRf = container.IsRf;

            ContainerType = container.ContainerType;
            ContainerTypeRecognized = container.ContainerTypeRecognized;
            IsClosed = container.IsClosed;

            Carrier = container.Carrier;
            FinalDestination = container.FinalDestination;
            POD = container.POD;
            POL = container.POL;
        }
        #endregion


        #region Constructor
        // -------------- public constructor ----------------------------------------

        public Container()
        {
            ContainerNumber = null;
            DgCountInContainer = 0;
            IsRf = false;
            IsClosed = true;
            ContainerTypeRecognized = false;
        }

        #endregion
    }
}
