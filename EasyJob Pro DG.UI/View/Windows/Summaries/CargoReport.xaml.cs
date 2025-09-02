using EasyJob_ProDG.UI.IO;
using EasyJob_ProDG.UI.Services.DialogServices;
using EasyJob_ProDG.UI.View.WindowBase;

namespace EasyJob_ProDG.UI.View.DialogWindows
{
    /// <summary>
    /// Логика взаимодействия для CargoReport.xaml
    /// </summary>
    public partial class CargoReport : AnimatedWindow, IDialogWindow
    {
        public CargoReport()
        {
            InitializeComponent();
        }

        private void ExportToExcel(object sender, System.Windows.RoutedEventArgs e)
        {
            ExportDataGridToExcel.ExportToExcel(CargoReportDataGrid, addSummary: true);
        }
    }
}
