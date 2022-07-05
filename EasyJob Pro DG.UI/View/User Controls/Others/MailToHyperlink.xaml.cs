using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace EasyJob_ProDG.UI.View.User_Controls
{
    /// <summary>
    /// Логика взаимодействия для MailToHyperlink.xaml
    /// </summary>
    public partial class MailToHyperlink : UserControl
    {
        public MailToHyperlink()
        {
            InitializeComponent();
        }

        private void OnNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.AbsoluteUri);
            e.Handled = true;
        }
    }
}
