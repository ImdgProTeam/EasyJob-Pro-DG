using System.Windows;

namespace EasyJob_ProDG.UI.Services.DialogServices
{
    /// <summary>
    /// Service is to display Windows in dialog mode without any additional functionality.
    /// </summary>
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
