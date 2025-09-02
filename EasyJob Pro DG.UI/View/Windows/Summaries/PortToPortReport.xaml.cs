using EasyJob_ProDG.UI.Services.DialogServices;
using EasyJob_ProDG.UI.View.WindowBase;
using System.Windows;

namespace EasyJob_ProDG.UI.View.DialogWindows
{
    /// <summary>
    /// Логика взаимодействия для PortToPortReport.xaml
    /// </summary>
    public partial class PortToPortReport : AnimatedWindow, IDialogWindow
    {
        public PortToPortReport()
        {
            InitializeComponent();
        }

        private void ExportToExcel(object sender, System.Windows.RoutedEventArgs e)
        {
            MessageBox.Show("Not implemented for this report.", "Error", button: MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
    }
}
