using EasyJob_ProDG.UI.View.Animations;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace EasyJob_ProDG.UI.View.DialogWindows
{
    /// <summary>
    /// Window class with In and Out animation added.
    /// Also defines DragMove (Window_MouseLeftButtonDown) and CloseWindow methods that can be reffered to through xaml.
    /// </summary>
    public class AnimatedDialogWindow : FeaturedDialogWindow
    {
        private AnimationTypes DialogWindowLoadAnimation { get; set; } = AnimationTypes.FadeIn;
        private AnimationTypes DialogWindowUnloadAnimation { get; set; } = AnimationTypes.FadeOut;
        private float FadeInSeconds { get; set; } = 0.4f;
        private float FadeOutSeconds { get; set; } = 0.6f;


        #region Constructors

        public AnimatedDialogWindow()
        {
            if (DialogWindowLoadAnimation != AnimationTypes.None)
                Visibility = Visibility.Collapsed;

            this.Loaded += Window_Loaded;
            this.Closing += Window_Closing;
        }


        public AnimatedDialogWindow(AnimationTypes dialogWindowLoadAnimation, float animationSeconds = 1,
                                    AnimationTypes dialogWindowUnloadAnimation = AnimationTypes.None, float animationOutSeconds = 1)
            : this()
        {
            FadeInSeconds = animationSeconds;
            FadeOutSeconds = animationOutSeconds;
            DialogWindowLoadAnimation = dialogWindowLoadAnimation;
            DialogWindowUnloadAnimation = dialogWindowUnloadAnimation;
        }

        #endregion


        /// <summary>
        /// Switching opening animation.
        /// </summary>
        /// <returns>None.</returns>
        private async Task AnimateIn()
        {
            if (this.DialogWindowLoadAnimation == AnimationTypes.None) return;

            switch (this.DialogWindowLoadAnimation)
            {
                case AnimationTypes.FadeIn:
                    await this.FadeIn(this.FadeInSeconds);
                    break;
                default:
                    return;
            }
        }

        /// <summary>
        /// Switching closing animation.
        /// </summary>
        /// <returns>None.</returns>
        private async Task AnimateOut()
        {
            if (this.DialogWindowUnloadAnimation == AnimationTypes.None) return;

            switch (this.DialogWindowUnloadAnimation)
            {
                case AnimationTypes.FadeOut:
                    await this.FadeOut(this.FadeOutSeconds);
                    break;
                default:
                    return;
            }
        }


        /// <summary>
        /// Defines Window.Loaded logic.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await AnimateIn();
        }

        /// <summary>
        /// Defines Window closing logic.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual async void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            await AnimateOut();
            this.Closing -= Window_Closing;
            this.Close();
        }


    }
}
