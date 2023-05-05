using EasyJob_ProDG.UI.Wrapper;

namespace EasyJob_ProDG.UI.Messages
{
    /// <summary>
    /// Messages used to update ConflictListViewModel (conflicts panel)
    /// </summary>
    class ConflictListToBeUpdatedMessage
    {
        internal bool OnlyUnitStowageToBeUpdated = false;
        internal bool FullListToBeUpdated = false;
        internal bool OnlyClearDeletedConflictsList;
        internal DgWrapper dgWrapper;

        public ConflictListToBeUpdatedMessage()
        {

        }

        public ConflictListToBeUpdatedMessage(DgWrapper _dgWrapper)
        {
            dgWrapper = _dgWrapper;
            OnlyUnitStowageToBeUpdated = true;
        }

        public ConflictListToBeUpdatedMessage(bool fullListToBeUpdated = false)
        {
            FullListToBeUpdated = fullListToBeUpdated;
        }

    }
}
