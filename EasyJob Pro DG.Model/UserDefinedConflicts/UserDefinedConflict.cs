using System.Collections.Generic;

namespace EasyJob_ProDG.Model.UserDefinedConflicts
{
    /// <summary>
    /// Defines properties of <see cref="UserDefinedConflict"/> and Equals methods.
    /// </summary>
    public class UserDefinedConflict
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string ContainerNumber { get; set; }

        public bool IsMultiUnitsConflict { get; set; }

        /// <summary>
        /// Reference to a <see cref="UserDefinedCondition"/> that defined this conflict. Used to link conflict with condition that caused it and to enable remove the condition from conflict.
        /// </summary>
        internal UserDefinedCondition ConditionReference;


        #region Equals override methods

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is UserDefinedConflict)) return false;
            return this.Equals((UserDefinedConflict)obj);
        }

        public bool Equals(UserDefinedConflict conflict)
        {
            return string.Equals(Title, conflict.Title)
                && string.Equals(Description, conflict.Description);
        }

        public override int GetHashCode()
        {
            int hashCode = -711990007;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Title);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Description);
            hashCode = hashCode * -1521134295 + IsMultiUnitsConflict.GetHashCode();
            return hashCode;
        }

        #endregion

        #region Constructors

        public UserDefinedConflict()
        {

        }

        public UserDefinedConflict(string title, string description, UserDefinedCondition conditionReference, bool isMultiUnitsConflict = false)
        {
            Title = title;
            Description = description;
            ConditionReference = conditionReference;
            IsMultiUnitsConflict = isMultiUnitsConflict;
        } 

        #endregion
    }
}
