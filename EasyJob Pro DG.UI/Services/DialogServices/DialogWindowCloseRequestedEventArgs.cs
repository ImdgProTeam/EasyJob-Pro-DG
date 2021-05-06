using System;

namespace EasyJob_ProDG.UI.Services.DialogServices
{
    public class DialogWindowCloseRequestedEventArgs : EventArgs
    {
        public bool? DialogResult { get; }

        public DialogWindowCloseRequestedEventArgs(bool? dialogResult)
        {
            DialogResult = dialogResult;
        }

    }
}
