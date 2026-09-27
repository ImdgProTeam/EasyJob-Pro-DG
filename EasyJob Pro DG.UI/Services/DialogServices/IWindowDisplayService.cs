using System.Windows;

namespace EasyJob_ProDG.UI.Services.DialogServices
{
    /// <summary>
    /// Interface to service to display Windows in dialog mode with or without binding to ViewModel 
    /// and without any additional functionality.
    /// </summary>
    internal interface IWindowDisplayService
    {
        /// <summary>
        /// Closes window.
        /// </summary>
        /// <param name="window">Window to close.</param>
        void CloseDialog(Window window);

        /// <summary>
        /// Displays dialog window.
        /// </summary>
        /// <param name="window">Window to display.</param>
        void ShowDialog(Window window);

        /// <summary>
        /// Displays dialog window with data bound to viewModel.
        /// </summary>
        /// <typeparam name="TViewModel">Type of view model class.</typeparam>
        /// <param name="window">window to be displayed.</param>
        /// <param name="viewModel">VM the window DataContext to bound to.</param>
        void ShowDialog<TViewModel>(Window window, TViewModel viewModel) where TViewModel : class, new();

        /// <summary>
        /// Displays normal window.
        /// </summary>
        /// <param name="window">Window to display.</param>
        void ShowNormal(Window window);

        /// <summary>
        /// Displays window with data bound to viewModel.
        /// </summary>
        /// <typeparam name="TViewModel">Type of view model class.</typeparam>
        /// <param name="window">window to be displayed.</param>
        /// <param name="viewModel">VM the window DataContext to bound to.</param>
        void ShowNormal<TViewModel>(Window window, TViewModel viewModel) where TViewModel : class, new();
    }
}