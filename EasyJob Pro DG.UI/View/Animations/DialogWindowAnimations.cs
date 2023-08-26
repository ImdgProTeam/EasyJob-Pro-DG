using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace EasyJob_ProDG.UI.View.Animations
{
    /// <summary>
    /// Helper class for UserControl Animations
    /// </summary>
    public static class DialogWindowAnimations
    {
        public static async Task FadeIn(this Window window, float seconds = 0.3f)
        {
            var sb = new Storyboard();

            window.Visibility = Visibility.Visible;
            sb.AddFadeIn(seconds);

            sb.Begin(window);

            await Task.Delay((int)(seconds * 1000));
        }

        public static async Task FadeOut(this Window window, float seconds = 0.05f)
        {
            var sb = new Storyboard();
            sb.AddFadeOut(seconds);
            sb.Begin(window);

            await Task.Delay((int)(seconds * 1000));
        }
    }
}
