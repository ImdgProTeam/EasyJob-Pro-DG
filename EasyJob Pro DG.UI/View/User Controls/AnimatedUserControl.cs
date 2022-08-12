using EasyJob_ProDG.UI.View.Animations;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EasyJob_ProDG.UI.View.User_Controls
{
    public class AnimatedUserControl : UserControl
    {
        private bool _isAnimatedEachTime;

        public AnimationTypes UserControlLoadAnimation { get; set; } = AnimationTypes.None;
        public AnimationTypes UserControlUnloadAnimation { get; set; } = AnimationTypes.None;
        public float SlideInSeconds { get; set; }


        public AnimatedUserControl()
        {
            if (UserControlLoadAnimation != AnimationTypes.None)
                Visibility = Visibility.Collapsed;

            this.Loaded += UserControl_Loaded;
        }

        public AnimatedUserControl(AnimationTypes userControlLoadAnimation, float slideInSeconds = 1.5f, 
                                    AnimationTypes userControlUnloadAnimation = AnimationTypes.None, bool isAnimatedEachTime = false)
            : this()
        {
            SlideInSeconds = slideInSeconds;
            UserControlLoadAnimation = userControlLoadAnimation;
            UserControlUnloadAnimation = userControlUnloadAnimation;

            _isAnimatedEachTime = isAnimatedEachTime;
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if(!_isAnimatedEachTime)
                Loaded -= UserControl_Loaded;
            await AnimateIn();
        }

        public async Task AnimateIn()
        {
            if (this.UserControlLoadAnimation == AnimationTypes.None) return;

            switch (this.UserControlLoadAnimation)
            {
                case AnimationTypes.SlideAndFadeInFromRight:
                    await this.SlideAndFadeInFromRight(this.SlideInSeconds);
                    break;
                case AnimationTypes.SlideAndFadeInFromLeft:
                    await this.SlideAndFadeInFromLeft(this.SlideInSeconds);
                    break;
                case AnimationTypes.SlideAndFadeInFromTop:
                    await this.SlideAndFadeInFromTop(this.SlideInSeconds);
                    break;
                case AnimationTypes.SlideAndFadeInFromBottom:
                    await this.SlideAndFadeInFromBottom(this.SlideInSeconds);
                    break;
                default:
                    return;
            }
        }



    }
}
