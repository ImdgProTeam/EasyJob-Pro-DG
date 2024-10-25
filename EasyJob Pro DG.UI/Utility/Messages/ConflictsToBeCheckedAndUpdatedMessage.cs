using EasyJob_ProDG.UI.Wrapper;

namespace EasyJob_ProDG.UI.Messages
{
    /// <summary>
    /// To be used when conflicts need to be rechecked (then updated conflict list respectively)
    /// </summary>
    class ConflictsToBeCheckedAndUpdatedMessage
    {
        internal DgWrapper dgWrapper;
        internal bool FullListToBeUpdated = false;

        public ConflictsToBeCheckedAndUpdatedMessage()
        {
                
        }

        public ConflictsToBeCheckedAndUpdatedMessage(DgWrapper _dgWrapper)
        {
                dgWrapper = _dgWrapper;
        }

        public ConflictsToBeCheckedAndUpdatedMessage(bool fullListToBeUpdated)
        {
            FullListToBeUpdated = fullListToBeUpdated;       
        }
    }
}
