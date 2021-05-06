using System.Windows;

namespace EasyJob_ProDG.UI.View.UI
{
    /// <summary>
    /// Логика взаимодействия для ShipProfile.xaml
    /// </summary>
    public partial class ShipProfileWindow : Window
    {
        public ShipProfileWindow()
        {
            InitializeComponent();
            buttonBack.IsEnabled = false;

        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            if (tabShipProfile.SelectedIndex > 0)
                tabShipProfile.SelectedIndex--;
            buttonNext.IsEnabled = true;
            if (tabShipProfile.SelectedIndex == 0)
                buttonBack.IsEnabled = false;
        }

        private void buttonNext_Click(object sender, RoutedEventArgs e)
        {
            if (tabShipProfile.SelectedIndex < tabShipProfile.Items.Count - 1)
                tabShipProfile.SelectedIndex++;
            buttonBack.IsEnabled = true;
            if (tabShipProfile.SelectedIndex == tabShipProfile.Items.Count - 1)
            {
                buttonNext.IsEnabled = false;
            }
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {

        }

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ButtonState == System.Windows.Input.MouseButtonState.Pressed)
                this.DragMove();
        }


    }
}
