using System.Windows;
using System.Windows.Controls;

namespace EasyJob_ProDG.UI.View.User_Controls
{
    /// <summary>
    /// Логика взаимодействия для TabLivingQuarters.xaml
    /// </summary>
    public partial class TabLivingQuarters : UserControl
    {
        public TabLivingQuarters()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            byte row = (byte)dataGridLivingQuarters.SelectedIndex;
            OnRemoveRecordEventHandler.Invoke(row);
        }

        public delegate void RemoveRecord(byte row);
        public static event RemoveRecord OnRemoveRecordEventHandler = null;
    }
}
