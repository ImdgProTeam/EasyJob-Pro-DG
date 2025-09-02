using EasyJob_ProDG.UI.Services.DialogServices;
using System.ComponentModel;

namespace EasyJob_ProDG.UI.View.WindowBase
{
    public class AnimatedDialogResultWindow : AnimatedWindow, IDialogWindow
    {
        protected override void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = false;
        }
    }
}
