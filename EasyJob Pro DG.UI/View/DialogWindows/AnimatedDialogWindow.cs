using EasyJob_ProDG.UI.View.Animations;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace EasyJob_ProDG.UI.View.DialogWindows
{
    public class AnimatedDialogWindow : Window
    {
        public AnimationTypes DialogWindowLoadAnimation { get; set; } = AnimationTypes.FadeIn;
        public AnimationTypes DialogWindowUnloadAnimation { get; set; } = AnimationTypes.FadeOut;
        public float FadeInSeconds { get; set; } = 0.4f;
        public float FadeOutSeconds { get; set; } = 0.6f;

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



        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await AnimateIn();
        }
        private async void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            await AnimateOut();
            this.Closing -= Window_Closing;
            this.Close();
        }


        public async Task AnimateIn()
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

        public async Task AnimateOut()
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
    }
}
