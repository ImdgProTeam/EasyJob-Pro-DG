using System.Windows.Controls;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.View.User_Controls
{
    /// <summary>
    /// Interaction logic for SearchBox.xaml
    /// </summary>
    public partial class SearchBoxUserControl : UserControl
    {
        public SearchBoxUserControl()
        {
            InitializeComponent();
        }

        private void UserControl_GotFocus(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!SearchBox.IsKeyboardFocusWithin)
            {
                SearchBox.Focus();
                Keyboard.Focus(SearchBox);
            }
        }
    }
}
