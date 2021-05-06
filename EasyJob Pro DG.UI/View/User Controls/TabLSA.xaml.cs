using System.Windows;
using System.Windows.Controls;

namespace EasyJob_ProDG.UI.View.User_Controls
{
    /// <summary>
    /// Логика взаимодействия для TabLSA.xaml
    /// </summary>
    public partial class TabLSA : UserControl
    {
        public TabLSA()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            byte row = (byte)dataGridLSA.SelectedIndex;
            OnRemoveRecordEventHandler.Invoke(row);
        }

        public delegate void RemoveRecord(byte row);
        public static event RemoveRecord OnRemoveRecordEventHandler = null;
    }
}
