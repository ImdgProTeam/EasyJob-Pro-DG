using EasyJob_ProDG.UI.Services.DialogServices;
using System.Windows;

namespace EasyJob_ProDG.UI.View.DialogWindows
{
    /// <summary>
    /// Логика взаимодействия для DgSummaryReport.xaml
    /// </summary>
    public partial class DgSummaryReport : AnimatedDialogWindow, IDialogWindow
    {
        public DgSummaryReport()
        {
            InitializeComponent();
        }

        private void ExportToExcel(object sender, System.Windows.RoutedEventArgs e)
        {
            MessageBox.Show("Not implemented for this report.", "Error", button:MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
    }
}
