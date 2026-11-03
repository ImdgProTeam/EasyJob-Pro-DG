using EasyJob_ProDG.Model.Transport;
using System.Collections.Generic;

namespace EasyJob_ProDG.Model.UserDefinedConflicts
{
    /// <summary>
    /// Describes <see cref="UserDefinedCondition"/> - a set of requirements defined by a user to popup as a conflict if condition is met.
    /// </summary>
    public class UserDefinedCondition
    {
        public UserDefinedConditionType ConditionType { get; set; }
        public string ConditionValue { get; set; }
        public CellPosition CellPosition { get; set; }

        public UserDefinedConditionOption ConditionOption { get; set; }
        public string ConditionComment { get; set;  }

        public override bool Equals(object obj)
        {
            if(obj == null || !(obj is UserDefinedCondition)) return false;
            return this.Equals((UserDefinedCondition)obj);
        }

        public bool Equals(UserDefinedCondition condition)
        {
            return ConditionType == condition.ConditionType
                && string.Equals(ConditionValue, condition.ConditionValue)
                && CellPosition == condition.CellPosition
                && ConditionOption == condition.ConditionOption;
        }

        public override int GetHashCode()
        {
            int hashCode = 129416394;
            hashCode = hashCode * -1521134295 + ConditionType.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(ConditionValue);
            hashCode = hashCode * -1521134295 + EqualityComparer<CellPosition>.Default.GetHashCode(CellPosition);
            hashCode = hashCode * -1521134295 + ConditionOption.GetHashCode();
            return hashCode;
        }

        #region Constructors

        public UserDefinedCondition(UserDefinedConditionType conditionType, string conditionValue, CellPosition cellPosition = null, UserDefinedConditionOption conditionOption = 0)
        {
            ConditionType = conditionType;
            ConditionValue = conditionValue;
            ConditionOption = conditionOption;
            CellPosition = cellPosition ?? new CellPosition();
        }

        public UserDefinedCondition()
        {

        } 

        #endregion
    }
}
