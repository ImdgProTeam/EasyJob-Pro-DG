using System.Windows;

namespace EasyJob_ProDG.UI.View.WindowBase
{
    public class AnimatedToolWindow : AnimatedWindow
    {
        protected void OnLostFocusHandler(object sender, System.EventArgs e)
        {
            Window window = sender as Window;
            window.Opacity = 0.6;
        }

        protected void OnGotFocusHandler(object sender, System.EventArgs e)
        {
            Window window = sender as Window;
            window.Opacity = 1.0;
        }

        protected void OnClosed(object sender, System.EventArgs e)
        {
        }
    }
}
