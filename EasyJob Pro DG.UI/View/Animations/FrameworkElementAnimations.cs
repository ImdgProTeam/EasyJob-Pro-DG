using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace EasyJob_ProDG.UI.View.Animations
{
    /// <summary>
    /// Helper class for <see cref="FrameworkElement"/> animations.
    /// </summary>
    public static class FrameworkElementAnimations
    {
        public static async Task FadeInAsync(this FrameworkElement element, float seconds = 0.3f)
        {
            var sb = new Storyboard();

            element.Visibility = Visibility.Visible;
            sb.AddFadeIn(seconds);

            sb.Begin(element);

            await Task.Delay((int)(seconds * 1000));
        }

        public static async Task FadeOutAsync(this FrameworkElement element, float seconds = 0.3f)
        {
            var sb = new Storyboard();
            sb.AddFadeOut(seconds);
            sb.Begin(element);

            await Task.Delay((int)(seconds * 1000));

            element.Visibility = Visibility.Collapsed;
        }
    }
}
