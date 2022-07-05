using System.Windows;

namespace EasyJob_ProDG.UI.View.UI
{
    /// <summary>
    /// Логика взаимодействия для winAbout.xaml
    /// </summary>
    public partial class winAbout : Window
    {
        public winAbout()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ButtonState == System.Windows.Input.MouseButtonState.Pressed)
                this.DragMove();

        }

    }
}
