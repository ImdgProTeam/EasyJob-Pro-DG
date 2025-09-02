using EasyJob_ProDG.UI.Services.DialogServices;
using System;
using System.Windows.Input;
using EasyJob_ProDG.UI.Utility;

namespace EasyJob_ProDG.UI.View.DialogWindows
{
    public class DialogWindowOptionsViewModel : IDialogWindowRequestClose
    {
        public string Header { get; }
        public string Message { get; }
        public string Button1Capture { get; }
        public string Button2Capture { get; }
        public string Button3Capture { get; }
        public bool IsButton2Visible { get; }
        public bool IsButton3Visible { get; }
        public bool IsHeaderVisible { get; }
        public byte ResultOption { get; private set; }
        public event EventHandler<DialogWindowCloseRequestedEventArgs> CloseRequested;

        public DialogWindowOptionsViewModel(string message, string button1Capture, string button2Capture = null, string button3Capture = null, string header = null)
        {
            Message = message;
            Button1Capture = button1Capture;

            if (header == null) IsHeaderVisible = false;
            else
            {
                Header = header;
                IsHeaderVisible = true;
            }

            if (button3Capture == null)
            {
                IsButton3Visible = false;
            }
            else
            {
                IsButton3Visible = true;
                Button3Capture = button3Capture;
            }
            if (button2Capture == null)
            {
                IsButton2Visible = false;
            }
            else
            {
                IsButton2Visible = true;
                Button2Capture = button2Capture;
            }

            Button1Command = new DelegateCommand(ButtonOnExecuted);
            Button2Command = new DelegateCommand(ButtonOnExecuted);
            Button3Command = new DelegateCommand(ButtonOnExecuted);
        }

        private void ButtonOnExecuted(object obj)
        {
            byte resultOption;
            if (byte.TryParse((string) obj, out resultOption)) ResultOption = resultOption;
            CloseRequested?.Invoke(this,new DialogWindowCloseRequestedEventArgs(true));
        }

        public ICommand Button1Command { get; }
        public ICommand Button2Command { get; }
        public ICommand Button3Command { get; }
}
}
