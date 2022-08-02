using System.Windows;

namespace EasyJob_ProDG.UI.View.User_Controls
{
    /// <summary>
    /// Логика взаимодействия для MainMenu.xaml
    /// </summary>
    public partial class MainMenu : AnimatedUserControl
    {
        public MainMenu() : base(Animations.AnimationTypes.SlideAndFadeInFromTop)
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
