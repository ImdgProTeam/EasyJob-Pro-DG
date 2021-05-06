namespace EasyJob_ProDG.Model.IO
{
    public interface IUpdatable
    {
        /// <summary>
        /// Location of unit cannot be changed.
        /// </summary>
        bool IsPositionLockedForChange { get; set; }

        /// <summary>
        /// Unit will be ignored when importing data.
        /// </summary>
        bool IsNotToImport { get; set; }

        /// <summary>
        /// Unit selected to import the data.
        /// </summary>
        bool IsToImport { get; set; }

        /// <summary>
        /// Unit shall be kept in plan after update.
        /// </summary>
        bool IsToBeKeptInPlan { get; set; }

        /// <summary>
        /// Has unit just been added with the last update.
        /// </summary>
        bool IsNewUnitInPlan { get; set; }

        /// <summary>
        /// Has location of the unit changed with the last update.
        /// </summary>
        bool HasLocationChanged { get; set; }

        /// <summary>
        /// Has unit Dg properties been updated with the last import.
        /// </summary>
        bool HasUpdated { get; set; }

        /// <summary>
        /// Has POL of the unit changed with the last update.
        /// </summary>
        bool HasPodChanged { get; set; }

        /// <summary>
        /// Has container type changed with the last update.
        /// </summary>
        bool HasContainerTypeChanged { get; set; }

        /// <summary>
        /// Location which unit had before re-assigned position.
        /// </summary>
        string LocationBeforeRestow { get; set; }

        abstract string ContainerNumber { get; set; }
        abstract string Location { get; set; }

    }
}
