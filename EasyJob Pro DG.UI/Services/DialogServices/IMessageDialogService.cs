namespace EasyJob_ProDG.UI.Services.DialogServices
{
    public interface IMessageDialogService
    {
        void ShowOkDialog(string text, string title);
        MessageDialogResult ShowOkCancelDialog(string text, string title);
        MessageDialogResult ShowYesNoDialog(string text, string title);
        MessageDialogResult ShowYesNoCancelDialog(string text, string title);
    }
}