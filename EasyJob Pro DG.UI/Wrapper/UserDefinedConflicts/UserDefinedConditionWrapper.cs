using EasyJob_ProDG.Model.Transport;
using EasyJob_ProDG.Model.UserDefinedConflicts;

namespace EasyJob_ProDG.UI.Wrapper
{
    public class UserDefinedConditionWrapper : ModelWrapper<UserDefinedCondition>
    {
        // ----- DataGrid display text and commands -----
        public uint ConditionNumberInList { get; set; }
        public string ConditionDescription { get; set; }
        public string ToolTipText { get; set; }


        // ----- Set conditions -----
        public UserDefinedConditionType ConditionType
        {
            get => GetValue<UserDefinedConditionType>();
            set
            {
                SetValue(value);
            }
        }
        public string ConditionValue
        {
            get => GetValue<string>();
            set
            {
                SetValue(value);
            }
        }

        public CellPosition CellPosition
        {
            get => GetValue<CellPosition>();
            set
            {
                SetValue(value);
            }
        }

        public UserDefinedConditionOption ConditionOption
        {
            get => GetValue<UserDefinedConditionOption>();
            set
            {
                SetValue(value);
            }
        }

        public string ConditionComment
        {
            get => GetValue<string>();
            set
            {
                SetValue(value);
            }
        }

        // ----- Methods -----

        /// <summary>
        /// Calls OnPropertyChanged method to all display properties
        /// </summary>
        internal void RefreshView()
        {
            OnPropertyChanged(null);
        }


        // ----- Constructor -----

        public UserDefinedConditionWrapper(UserDefinedCondition model) : base(model)
        {

        }
    }
}
