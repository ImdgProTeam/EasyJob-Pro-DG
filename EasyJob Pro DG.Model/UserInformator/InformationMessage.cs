using System.Collections.Generic;

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

        #region Equals

        public override bool Equals(object obj)
        {
            if (obj is not InformationMessage message) return false;
            return this.Equals(message);
        }

        public bool Equals(InformationMessage message)
        {
            return this.MessageType == message.MessageType
                && string.Equals(this.Title, message.Title)
                && string.Equals(this.Message, message.Message);
        }

        public override int GetHashCode()
        {
            int hashCode = -169503607;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Title);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Message);
            hashCode = hashCode * -1521134295 + MessageType.GetHashCode();
            return hashCode;
        }

        #endregion 
    }
}
