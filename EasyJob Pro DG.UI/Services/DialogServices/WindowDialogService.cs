using System.Windows;

namespace EasyJob_ProDG.UI.Services.DialogServices
{
    /// <summary>
    /// Service is to display Windows in dialog mode without any additional functionality.
    /// </summary>
    class WindowDialogService
    {
        /// <summary>
        /// Displays window.
        /// </summary>
        /// <param name="window"></param>
        public void ShowDialog(Window window)
        {
            window.ShowDialog();
        }

        /// <summary>
        /// Displays window with data bound to viewModel.
        /// </summary>
        /// <typeparam name="TViewModel">Type of view model class.</typeparam>
        /// <param name="window">window to be displayed.</param>
        /// <param name="viewModel">VM the window DataContext to bound to.</param>
        public void ShowDialog<TViewModel>(Window window, TViewModel viewModel)
            where TViewModel : class, new()
        {
            window.DataContext = viewModel;
            window.ShowDialog();
        }

        /// <summary>
        /// Closes window.
        /// </summary>
        /// <param name="window"></param>
        public void CloseDialog(Window window)
        {
            window.Close();
        }
    }
}
