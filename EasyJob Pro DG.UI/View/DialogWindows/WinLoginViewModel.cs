using System;
using System.Windows.Input;
using EasyJob_ProDG.UI.Services.DialogServices;
using EasyJob_ProDG.UI.Utility;

namespace EasyJob_ProDG.UI.View.DialogWindows
{
    public class WinLoginViewModel : IDialogWindowRequestClose
    {
        public WinLoginViewModel()
        {
            LoginCommand = new DelegateCommand(p => CloseRequested?.Invoke(this, new DialogWindowCloseRequestedEventArgs(true)));
            CancelCommand = new DelegateCommand(p => CloseRequested?.Invoke(this, new DialogWindowCloseRequestedEventArgs(false)));
        }

        public event EventHandler<DialogWindowCloseRequestedEventArgs> CloseRequested;

        public ICommand LoginCommand { get; }
        public ICommand CancelCommand { get; }
    }
}
