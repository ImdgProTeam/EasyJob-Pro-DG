using EasyJob_ProDG.Model.Transport;

namespace EasyJob_ProDG.UI.Wrapper.UserDefinedConflicts
{
    internal static class UserDefinedConditionWrapperExtensions
    {
        internal static UserDefinedConditionWrapper Clone(this UserDefinedConditionWrapper conditionWrapperToClone)
        {
            return new UserDefinedConditionWrapper(new Model.UserDefinedConflicts.UserDefinedCondition())
            {
                ConditionNumberInList = conditionWrapperToClone.ConditionNumberInList,
                ConditionType = conditionWrapperToClone.ConditionType,
                ConditionValue = conditionWrapperToClone.ConditionValue,
                ConditionOption = conditionWrapperToClone.ConditionOption,
                ConditionDescription = conditionWrapperToClone.ConditionDescription,
                ConditionComment = conditionWrapperToClone.ConditionComment,
                ToolTipText = conditionWrapperToClone.ToolTipText,
                CellPosition = conditionWrapperToClone.Model.CellPosition.Clone()
            };
        }
    }
}
