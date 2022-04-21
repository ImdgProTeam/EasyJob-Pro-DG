namespace EasyJob_ProDG.UI.Messages
{
    public class CancelChangesMessage
    {
        private bool _cancel;
        public bool Cancel
        {
            get => _cancel;
            set => _cancel = value;
        }

        public CancelChangesMessage()
        {

        }

        public CancelChangesMessage(bool cancel)
        {
            _cancel = cancel;
        }
    }
}
