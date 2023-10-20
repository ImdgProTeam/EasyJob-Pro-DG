using EasyJob_ProDG.Data;

namespace EasyJob_ProDG.Model.Cargo
{
    public abstract class ContainerAbstract : LocationOnBoard, IContainer, IO.IUpdatable
    {
        #region IContainer

        public bool IsClosed { get; set; }
        public bool IsRf { get; set; }
        public bool ContainerTypeRecognized { get; set; }
        public string ContainerNumber { get; set; }
        public string DisplayContainerNumber => UserSettings.ContainerNumberToDisplay(ContainerNumber);
        public bool HasNoNumber => string.IsNullOrEmpty(ContainerNumber)
                                || ContainerNumber.StartsWith(Data.ProgramDefaultSettingValues.NoNamePrefix);
        public virtual string ContainerType { get; set; }
        public string POD { get; set; }
        public string POL { get; set; }
        public string FinalDestination { get; set; }
        public string Carrier { get; set; }

        #endregion

        #region IUpdatable

        public bool IsPositionLockedForChange { get; set; }
        public bool IsNotToImport { get; set; }
        public bool IsToImport { get; set; }
        public bool IsToBeKeptInPlan { get; set; }
        public bool IsNewUnitInPlan { get; set; }
        public bool HasLocationChanged { get; set; }
        public bool HasUpdated { get; set; }
        public bool HasPodChanged { get; set; }
        public bool HasContainerTypeChanged { get; set; }
        public string LocationBeforeRestow { get; set; }

        #endregion
    }
}