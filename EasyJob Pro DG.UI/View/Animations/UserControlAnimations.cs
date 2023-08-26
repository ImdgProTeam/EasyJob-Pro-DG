using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace EasyJob_ProDG.UI.View.Animations
{
    /// <summary>
    /// Helper class for UserControl Animations
    /// </summary>
    public static class UserControlAnimations
    {
        public static async Task SlideAndFadeInFromRight(this UserControl userControl, float seconds)
        {
            var sb = new Storyboard();

            sb.AddSlideFromRight(seconds, userControl.ActualWidth);
            sb.AddFadeIn(seconds);

            sb.Begin(userControl);
            userControl.Visibility = Visibility.Visible;

            await Task.Delay((int)(seconds * 1000));
        }

        public static async Task SlideAndFadeInFromLeft(this UserControl userControl, float seconds)
        {
            var sb = new Storyboard();

            sb.AddSlideFromLeft(seconds, userControl.ActualWidth);
            sb.AddFadeIn(seconds);

            sb.Begin(userControl);
            userControl.Visibility = Visibility.Visible;

            await Task.Delay((int)(seconds * 1000));
        }

        public static async Task SlideAndFadeInFromTop(this UserControl userControl, float seconds)
        {
            var sb = new Storyboard();

            sb.AddSlideFromTop(seconds, userControl.ActualHeight);
            sb.AddFadeIn(seconds);

            sb.Begin(userControl);
            userControl.Visibility = Visibility.Visible;

            await Task.Delay((int)(seconds * 1000));
        }

        public static async Task SlideAndFadeInFromBottom(this UserControl userControl, float seconds)
        {
            var sb = new Storyboard();

            sb.AddSlideFromBottom(seconds, userControl.ActualHeight);
            sb.AddFadeIn(seconds);

            sb.Begin(userControl);
            userControl.Visibility = Visibility.Visible;

            await Task.Delay((int)(seconds * 1000));
        }
    }
}
