using EasyJob_ProDG.UI.Wrapper;

namespace EasyJob_ProDG.UI.Messages
{
    class ConflictListToBeUpdatedMessage
    {
        internal bool OnlyUnitStowageToBeUpdated = false;
        internal DgWrapper dgWrapper;

        public ConflictListToBeUpdatedMessage()
        {

        }

        public ConflictListToBeUpdatedMessage(DgWrapper _dgWrapper)
        {
            dgWrapper = _dgWrapper;
            OnlyUnitStowageToBeUpdated = true;
        }
    }
}
