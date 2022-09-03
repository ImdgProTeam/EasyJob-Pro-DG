using EasyJob_ProDG.UI.Services.DialogServices;

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
            throw new System.Exception("Function not implemented for this report.");
        }
    }
}
