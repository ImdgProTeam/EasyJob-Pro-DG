using System.Windows;

namespace EasyJob_ProDG.UI.View.DialogWindows
{
    /// <summary>
    /// Логика взаимодействия для winLicence.xaml
    /// </summary>
    public partial class winLicence : AnimatedDialogWindow
    {
        public winLicence()
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
