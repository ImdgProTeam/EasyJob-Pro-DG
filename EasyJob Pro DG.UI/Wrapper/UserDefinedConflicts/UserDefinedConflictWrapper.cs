using EasyJob_ProDG.Model.UserDefinedConflicts;

namespace EasyJob_ProDG.UI.Wrapper.UserDefinedConflicts
{
    internal class UserDefinedConflictWrapper : ModelWrapper<UserDefinedConflict>
    {
        public string Title
        {
            get { return GetValue<string>(); }
            set
            {
                SetValue(value);
            }
        }

        public string Description
        {
            get { return GetValue<string>(); }
            set
            {
                SetValue(value);
            }
        }

        public UserDefinedConflictWrapper(UserDefinedConflict model) : base(model)
        {
        }
    }
}
