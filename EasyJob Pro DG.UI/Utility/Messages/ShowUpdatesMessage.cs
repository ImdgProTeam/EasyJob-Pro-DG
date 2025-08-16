using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.UI.View;
using System.Collections.Generic;

namespace EasyJob_ProDG.UI.Utility.Messages
{
    /// <summary>
    /// Message contains list of containers and Units type to show in Updates DataGrid.
    /// </summary>
    internal class ShowUpdatesMessage
    {
        public Units Units;
        public List<Container> ContainersToShow;

        public ShowUpdatesMessage(List<Container> containers, Units units)
        {
            ContainersToShow = containers;
            Units = units;
        }
    }
}
