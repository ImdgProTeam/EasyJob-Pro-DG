using EasyJob_ProDG.Model.Cargo;

namespace EasyJob_ProDG.UI.Messages
{
    /// <summary>
    /// Message class to send information necessary to update same properties of all units in the plan.
    /// </summary>
    public class CargoPlanUnitPropertyChanged
    {
        public object Unit;
        public string PropertyName;
        public string ContainerNumber;
        public object Value;
        public object OldValue;
        public bool IsDgWrapper;

        public CargoPlanUnitPropertyChanged(IContainer unit, object value, string propertyName, bool isDgWrapper = true)
        {
            PropertyName = propertyName;
            ContainerNumber = unit?.ContainerNumber;
            Value = value;
            Unit = unit;
            IsDgWrapper = isDgWrapper;
        }

        public CargoPlanUnitPropertyChanged(IContainer unit, object value, object oldValue, string propertyName, bool isDgWrapper = true)
        {
            PropertyName = propertyName;
            Unit = unit;
            ContainerNumber = unit.ContainerNumber;
            Value = value;
            OldValue = oldValue;
            Unit = unit;
            IsDgWrapper = isDgWrapper;
        }
    }
}
