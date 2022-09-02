using EasyJob_ProDG.UI.Services.DialogServices;
using EasyJob_ProDG.UI.Utility;

namespace EasyJob_ProDG.UI.View.DialogWindows
{
    /// <summary>
    /// Логика взаимодействия для PortToPortReport.xaml
    /// </summary>
    public partial class PortToPortReport : AnimatedDialogWindow, IDialogWindow
    {
        public PortToPortReport()
        {
            InitializeComponent();
        }

        private void ExportToExcel(object sender, System.Windows.RoutedEventArgs e)
        {
            ExportDataGridToExcel.ExportToExcel(CargoReportDataGrid, addSummary: false);
        }
    }
}
