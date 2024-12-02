using EasyJob_ProDG.UI.Wrapper;

namespace EasyJob_ProDG.UI.Messages
{
    /// <summary>
    /// Messages used to update ConflictListViewModel (conflicts panel)
    /// </summary>
    class DisplayConflictsToBeRefreshedMessage
    {
        internal bool OnlyUnitStowageToBeUpdated = false;
        internal bool FullListToBeUpdated = false;
        internal DgWrapper dgWrapper;

        public DisplayConflictsToBeRefreshedMessage()
        {

        }

        public DisplayConflictsToBeRefreshedMessage(DgWrapper _dgWrapper)
        {
            dgWrapper = _dgWrapper;
            OnlyUnitStowageToBeUpdated = true;
        }

        public DisplayConflictsToBeRefreshedMessage(bool fullListToBeUpdated = false)
        {
            FullListToBeUpdated = fullListToBeUpdated;
        }

    }
}
