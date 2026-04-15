using System.Collections.Generic;

namespace EasyJob_ProDG.Model
{

    /// <summary>
    /// Class receives and stores <see cref="InformationMessage"/>s received during various operations in <see cref="Model"/> project for further informing of the user.
    /// </summary>
    public class UserInformator
    {
        #region Private members

        private static UserInformator _instance = new UserInformator();
        private List<InformationMessage> generalMessages;
        private List<InformationMessage> shipProfileMessages;
        private List<InformationMessage> readingConditionMessages;

        #endregion

        #region Public accessors

        public List<InformationMessage> GeneralMessages => generalMessages;
        public List<InformationMessage> ShipProfileMessages => shipProfileMessages;
        public List<InformationMessage> ReadingConditionMessages => readingConditionMessages;

        #endregion

        #region Public methods

        /// <summary>
        /// Add new <see cref="InformationMessage"/>.
        /// If no <see cref="InformationMessageType"/> specified, it will be assigned to <see cref="InformationMessageType.General"/>
        /// </summary>
        /// <param name="message"></param>
        public static void AddMessage(InformationMessage message)
        {
            switch (message.MessageType)
            {
                case InformationMessageType.ShipProfile:
                    _instance.shipProfileMessages.Add(message);
                    break;
                case InformationMessageType.ReadingCondition:
                    _instance.readingConditionMessages.Add(message);
                    break;
                case InformationMessageType.General:
                case InformationMessageType.All:
                default:
                    _instance.generalMessages.Add(message);
                    break;
            }
        }

        /// <summary>
        /// Add a <see cref="InformationMessageType.General"/> message without title.
        /// </summary>
        /// <param name="message">Message text</param>
        public static void AddMessage(string message)
        {
            _instance.generalMessages.Add(new InformationMessage("", message));
        }

        /// <summary>
        /// Receive a list of all stored <see cref="InformationMessage"/>s of specified <see cref="InformationMessageType"/>.
        /// If no parameter specified, all stored <see cref="InformationMessage"/>s will be returned.
        /// </summary>
        /// <param name="messageType"></param>
        /// <returns></returns>
        public static List<InformationMessage> GetMessages(InformationMessageType messageType = InformationMessageType.All)
        {
            switch (messageType)
            {
                case InformationMessageType.General:
                    return _instance.generalMessages;

                case InformationMessageType.ShipProfile:
                    return _instance.shipProfileMessages;

                case InformationMessageType.ReadingCondition:
                    return _instance.readingConditionMessages;

                case InformationMessageType.All:
                default:
                    return GetAllMessages();
            }
        }

        /// <summary>
        /// Receive a list of all stored <see cref="InformationMessage"/>s.
        /// </summary>
        /// <returns></returns>
        public static List<InformationMessage> GetAllMessages()
        {
            List<InformationMessage> result =
            [
                .. _instance?.generalMessages,
                .. _instance?.shipProfileMessages,
                .. _instance?.readingConditionMessages,
            ];
            return result;
        }

        /// <summary>
        /// Clear all stored <see cref="InformationMessage"/>s of specified <see cref="InformationMessageType"/>.
        /// If no parameter specified, all stored <see cref="InformationMessage"/>s will be cleared.
        /// </summary>
        /// <param name="messageType"></param>
        public static void ClearMessages(InformationMessageType messageType = InformationMessageType.All)
        {
            switch (messageType)
            {
                case InformationMessageType.General:
                    _instance.generalMessages.Clear();
                    break;
                case InformationMessageType.ShipProfile:
                    _instance.shipProfileMessages.Clear();
                    break;
                case InformationMessageType.ReadingCondition:
                    _instance.readingConditionMessages.Clear();
                    break;
                case InformationMessageType.All:
                default:
                    _instance.generalMessages.Clear();
                    _instance.shipProfileMessages.Clear();
                    _instance.readingConditionMessages.Clear();
                    break;
            }
        }

        #endregion

        #region Constructor
        private UserInformator()
        {
            generalMessages = new List<InformationMessage>();
            shipProfileMessages = new List<InformationMessage>();
            readingConditionMessages = new List<InformationMessage>();
        }

        #endregion
    }
}
