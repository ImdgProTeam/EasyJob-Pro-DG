using System.Collections.Generic;
using System.Linq;
using System.Windows.Markup;

namespace EasyJob_ProDG.Model.UserDefinedConflicts
{
    /// <summary>
    /// Defines properties of <see cref="UserDefinedConflict"/> and Equals methods.
    /// </summary>
    public class UserDefinedConflict
    {
        /// <summary>
        /// ContainerNumber of a conflict unit in non-multi-unit conflict.
        /// </summary>
        public string ContainerNumber { get; set; }

        /// <summary>
        /// List of conflicted ContainerNumbers in multi-unit conflict.
        /// </summary>
        public List<string> ContainerNumbers { get; set; }

        /// <summary>
        /// List of conflicted DgIDs in multi-unit conflict affecting DGs.
        /// </summary>
        public List<string> DgIDs { get; set; }

        /// <summary>
        /// Defines if conflict can affect multiple units or only one in case of false.
        /// </summary>
        public bool IsMultiUnitConflict { get; private set; }

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
            return Reference.ReferenceEquals(this.ConditionReference, conflict.ConditionReference)
                && (IsMultiUnitConflict
                        ? MultiUnitConflictsHaveSameValues(conflict)
                        : string.Equals(this.ContainerNumber, conflict.ContainerNumber));
        }

        /// <summary>
        /// Compares <see cref="ContainerNumbers"/> and <see cref="DgIDs"/> of two conflicts.
        /// </summary>
        /// <param name="conflict"></param>
        /// <returns>True if the lists contain all the same values.</returns>
        private bool MultiUnitConflictsHaveSameValues(UserDefinedConflict conflict)
        {

            bool hasDifferentContainerNumbers =
              ContainerNumbers.Except(conflict.ContainerNumbers).Any()
              || conflict.ContainerNumbers.Except(this.ContainerNumbers).Any();
            bool hasDifferentDgIDs =
                DgIDs.Except(conflict.DgIDs).Any() || conflict.DgIDs.Except(this.DgIDs).Any();
            return !hasDifferentContainerNumbers && !hasDifferentDgIDs;
        }

        #endregion

        #region Constructors

        public UserDefinedConflict()
        {

        }

        public UserDefinedConflict(bool isMultiUnit = false)
        {
            if (isMultiUnit)
            {
                ContainerNumbers = new List<string>();
                DgIDs = new List<string>();
            }
        }

        public UserDefinedConflict(string containerNumber, UserDefinedCondition conditionReference)
        {
            ContainerNumber = containerNumber;
            ConditionReference = conditionReference;
            IsMultiUnitConflict = false;
        }

        #endregion
    }
}
