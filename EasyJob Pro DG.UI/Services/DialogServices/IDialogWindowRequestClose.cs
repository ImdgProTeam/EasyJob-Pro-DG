using System;

namespace EasyJob_ProDG.UI.Services.DialogServices
{
    public interface IDialogWindowRequestClose
    {
        event EventHandler<DialogWindowCloseRequestedEventArgs> CloseRequested;
    }
}