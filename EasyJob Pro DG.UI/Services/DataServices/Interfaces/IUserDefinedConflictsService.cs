using EasyJob_ProDG.UI.Wrapper.UserDefinedConflicts;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace EasyJob_ProDG.UI.Services.DataServices.Interfaces
{
    internal interface IUserDefinedConflictsService
    {
        void AddConflict(UserDefinedConflictWrapper conflict);
        void RemoveConflict(UserDefinedConflictWrapper conflict);
        void AddCondition(UserDefinedConditionWrapper condition);
        void RemoveCondition(UserDefinedConditionWrapper condition);

        List<UserDefinedConflictWrapper> GetConflicts();
        ObservableCollection<UserDefinedConditionWrapper> Conditions { get; set; }
    }
}
