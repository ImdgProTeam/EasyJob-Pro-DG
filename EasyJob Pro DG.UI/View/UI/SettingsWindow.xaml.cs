using EasyJob_ProDG.UI.View.DialogWindows;
using System.Windows;

namespace EasyJob_ProDG.UI.View.UI
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : AnimatedDialogWindow
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Updates IsEnabled values of the buttons Next and Back
        /// </summary>
        private void ButtonsEnabledCheck()
        {
            if (tabUserSettings.SelectedIndex == 0)
                buttonBack.IsEnabled = false;
            else buttonBack.IsEnabled = true;

            if (tabUserSettings.SelectedIndex == tabUserSettings.Items.Count - 1)
            {
                buttonNext.IsEnabled = false;
            }
            else buttonNext.IsEnabled = true;
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            if (tabUserSettings.SelectedIndex > 0)
                tabUserSettings.SelectedIndex--;
            ButtonsEnabledCheck();
        }

        private void buttonNext_Click(object sender, RoutedEventArgs e)
        {
            if (tabUserSettings.SelectedIndex < tabUserSettings.Items.Count - 1)
                tabUserSettings.SelectedIndex++;
            ButtonsEnabledCheck();
        }

        private void tabUserSettings_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            ButtonsEnabledCheck();
        }
    }
}
