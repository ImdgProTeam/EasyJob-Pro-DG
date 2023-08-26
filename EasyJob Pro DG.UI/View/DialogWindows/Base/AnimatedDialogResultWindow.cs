using EasyJob_ProDG.UI.Services.DialogServices;
using System.ComponentModel;

namespace EasyJob_ProDG.UI.View.DialogWindows
{
    public class AnimatedDialogResultWindow : AnimatedDialogWindow, IDialogWindow
    {
        protected override void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = false;
        }
    }
}
