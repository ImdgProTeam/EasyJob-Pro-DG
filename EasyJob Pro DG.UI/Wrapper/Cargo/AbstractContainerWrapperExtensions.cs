using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyJob_ProDG.UI.Wrapper.Cargo
{
    internal static class AbstractContainerWrapperExtensions
    {
        internal static void SetPOD(this AbstractContainerWrapper<Container> container, string newValue)
        {
            container.Model.POD = newValue;
            NotifyOfChangedContainerProperty(container, newValue, "POD");
        }

        internal static void SetPOL(this AbstractContainerWrapper<Container> container, string newValue)
        {
            container.Model.POL = newValue;
            NotifyOfChangedContainerProperty(container, newValue, "POL");
        }

        internal static void SetFinalDestination(this AbstractContainerWrapper<Container> container, string newValue)
        {
            container.Model.FinalDestination = newValue;
            NotifyOfChangedContainerProperty(container, newValue, "FinalDestination");
        }

        private static void NotifyOfChangedContainerProperty(AbstractContainerWrapper<Container> container, string newValue, string propertyName)
        {
            DataMessenger.Default.Send(new CargoPlanUnitPropertyChanged(container, newValue, null, propertyName, false));
        }
    }
}
