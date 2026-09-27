using System.Windows;

namespace EasyJob_ProDG.UI.Services.DialogServices
{
    /// <summary>
    /// Service is to display Windows in dialog mode with or without binding to ViewModel 
    /// and without any additional functionality.
    /// </summary>
    class WindowDisplayService : IWindowDisplayService
    {
        public void ShowNormal(Window window)
        {
            window.Show();
        }

        public void ShowNormal<TViewModel>(Window window, TViewModel viewModel)
            where TViewModel : class, new()
        {
            window.DataContext = viewModel;
            window.Show();
        }

        public void ShowDialog(Window window)
        {
            window.ShowDialog();
        }

        public void ShowDialog<TViewModel>(Window window, TViewModel viewModel)
            where TViewModel : class, new()
        {
            window.DataContext = viewModel;
            window.ShowDialog();
        }

        public void CloseDialog(Window window)
        {
            window.Close();
        }
    }
}
