using System.Windows;
using EasyJob_ProDG.UI.Services.DialogServices;

namespace EasyJob_ProDG.UI.View.DialogWindows
{
    /// <summary>
    /// Логика взаимодействия для dialogOpenFileOptions.xaml
    /// </summary>
    public partial class DialogWindowOptions : Window, IDialogWindow
    {
        public DialogWindowOptions()
        {
            InitializeComponent();
        }

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ButtonState == System.Windows.Input.MouseButtonState.Pressed)
                this.DragMove();
        }
    }
}
