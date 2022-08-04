using System.ComponentModel;

namespace EasyJob_ProDG.UI.View.DialogWindows
{
    public class AnimatedDialogResultWindow : AnimatedDialogWindow
    {
        protected override void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = false;
        }
    }
}
