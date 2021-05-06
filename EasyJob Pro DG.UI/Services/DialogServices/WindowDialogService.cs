using System.Windows;

namespace EasyJob_ProDG.UI.Services.DialogServices
{
    class WindowDialogService
    {
        public void ShowDialog(Window window)
        {
            window.ShowDialog();
        }

        public void CloseDialog(Window window)
        {
            window.Close();
        }
    }
}
