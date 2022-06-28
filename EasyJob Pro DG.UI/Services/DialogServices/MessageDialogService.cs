using System.Windows;

namespace EasyJob_ProDG.UI.Services.DialogServices
{
    public class MessageDialogService : IMessageDialogService
    {
        private static MessageDialogService _messageDialogService = new MessageDialogService();

        public MessageDialogResult ShowOkCancelDialog(string text, string title)
        {
            var result = MessageBox.Show(text, title, MessageBoxButton.OKCancel);
            return result == MessageBoxResult.OK
                ? MessageDialogResult.OK
                : MessageDialogResult.Cancel;
        }

        public MessageDialogResult ShowYesNoDialog(string text, string title)
        {
            var result = MessageBox.Show(text, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return result == MessageBoxResult.Yes
                ? MessageDialogResult.Yes
                : MessageDialogResult.No;
        }

        public MessageDialogResult ShowYesNoCancelDialog(string text, string title)
        {
            var result = MessageBox.Show(text, title, MessageBoxButton.YesNoCancel);
            return result == MessageBoxResult.Yes
                ? MessageDialogResult.Yes
                : result == MessageBoxResult.No
                    ? MessageDialogResult.No
                    : MessageDialogResult.Cancel;
        }

        public void ShowOkDialog(string text, string title)
        {
            MessageBox.Show(text, title);
        }


        public static MessageDialogService Connect() => _messageDialogService;

        private MessageDialogService()
        {
            //empty
        }

    }

    public enum MessageDialogResult
    {
        OK,
        Cancel,
        Yes,
        No
    }
}
