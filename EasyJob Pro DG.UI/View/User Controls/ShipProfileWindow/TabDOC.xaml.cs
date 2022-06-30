using System.Windows.Controls;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.View.User_Controls
{
    /// <summary>
    /// Логика взаимодействия для TabDOC.xaml
    /// </summary>
    public partial class TabDOC : UserControl
    {
        public TabDOC()
        {
            InitializeComponent();
        }

        private void DataGrid_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            //if(e.Key == Key.Up)
            //{
            //    gridDOC.SelectedIndex = gridDOC.SelectedIndex == 0 ? 0 : gridDOC.SelectedIndex - 1;

            //    e.Handled = true;
            //}
        }
    }
}
