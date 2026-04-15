namespace EasyJob_ProDG.Model
{
    public class InformationMessage
    {
        public string Title { get; private set; }
        public string Message { get; private set; }
        public InformationMessageType MessageType { get; private set; }

        public InformationMessage(string title, string message, InformationMessageType messageType = InformationMessageType.General)
        {
            Title = title;
            Message = message;
            MessageType = messageType;
        }
    }
}
