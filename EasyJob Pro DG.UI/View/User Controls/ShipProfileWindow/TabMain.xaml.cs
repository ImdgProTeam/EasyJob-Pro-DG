using System.Windows.Controls;

namespace EasyJob_ProDG.UI.View.User_Controls
{
    /// <summary>
    /// Логика взаимодействия для TabMain.xaml
    /// </summary>
    public partial class TabMain : UserControl
    {
        public TabMain()
        {
            InitializeComponent();
            InvertRadioButtons();
        }

        private void InvertRadioButtons()
        {
            radRow00no.IsChecked = !radRow00yes.IsChecked;
            radNoPassenger.IsChecked = !radPassenger.IsChecked;
        }

        private void txbShipName_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            txbShipName.SelectAll();
        }
    }
}
