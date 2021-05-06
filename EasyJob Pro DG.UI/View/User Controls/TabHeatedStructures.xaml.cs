using System.Windows;
using System.Windows.Controls;

namespace EasyJob_ProDG.UI.View.User_Controls
{
    /// <summary>
    /// Логика взаимодействия для TabHeatedStructures.xaml
    /// </summary>
    public partial class TabHeatedStructures : UserControl
    {
        public TabHeatedStructures()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            byte row = (byte)dataGridHeatedStructures.SelectedIndex;
            OnRemoveRecordEventHandler.Invoke(row);
        }

        public delegate void RemoveRecord(byte row);
        public static event RemoveRecord OnRemoveRecordEventHandler = null;
    }
}
