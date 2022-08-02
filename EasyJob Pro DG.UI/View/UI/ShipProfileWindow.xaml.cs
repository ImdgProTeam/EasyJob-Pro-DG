using EasyJob_ProDG.UI.View.DialogWindows;
using System.Windows;

namespace EasyJob_ProDG.UI.View.UI
{
    /// <summary>
    /// Логика взаимодействия для ShipProfile.xaml
    /// </summary>
    public partial class ShipProfileWindow : AnimatedDialogWindow
    {
        public ShipProfileWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Updates IsEnabled values of the buttons Next and Back
        /// </summary>
        private void ButtonsEnabledCheck()
        {
            if (tabShipProfile.SelectedIndex == 0)
                buttonBack.IsEnabled = false;
            else buttonBack.IsEnabled = true;

            if (tabShipProfile.SelectedIndex == tabShipProfile.Items.Count - 1)
            {
                buttonNext.IsEnabled = false;
            }
            else buttonNext.IsEnabled = true;
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            if (tabShipProfile.SelectedIndex > 0)
                tabShipProfile.SelectedIndex--;
            ButtonsEnabledCheck();
        }

        private void buttonNext_Click(object sender, RoutedEventArgs e)
        {
            if (tabShipProfile.SelectedIndex < tabShipProfile.Items.Count - 1)
                tabShipProfile.SelectedIndex++;
            ButtonsEnabledCheck();
        }

        private void tabShipProfile_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ButtonsEnabledCheck();
        }
    }
}
