using EasyJob_ProDG.UI.View.Animations;
using System.Windows;

namespace EasyJob_ProDG.UI.View.AttachedProperties
{
    /// <summary>
    /// Attached properties for FrameworkElement animations
    /// </summary>
    public class FrameworkElementsAnimationAttachedProperties
    {
        #region Animate Fade in and out attached property

        public static bool GetAnimateFadeInAndOut(DependencyObject obj)
        {
            return (bool)obj.GetValue(AnimateFadeInAndOutProperty);
        }

        public static void SetAnimateFadeInAndOut(DependencyObject obj, bool value)
        {
            obj.SetValue(AnimateFadeInAndOutProperty, value);
        }

        /// <summary>
        /// On True fades in, on false fades out
        /// </summary>
        public static readonly DependencyProperty AnimateFadeInAndOutProperty =
            DependencyProperty.RegisterAttached("AnimateFadeInAndOut", typeof(bool), typeof(FrameworkElementsAnimationAttachedProperties),
                new UIPropertyMetadata(false, OnAnimateFadeInAndOutPropertyChanged));

        private static async void OnAnimateFadeInAndOutPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement element = (FrameworkElement)d;
            if (element is null) return;

            if ((bool)e.NewValue)
            {
                await element.FadeInAsync(0.15f);
            }
            else
            {
                await element.FadeOutAsync(0.1f);
            }
        } 

        #endregion
    }
}
