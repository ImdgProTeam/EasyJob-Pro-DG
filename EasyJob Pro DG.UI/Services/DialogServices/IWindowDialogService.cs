using System.Windows;

namespace EasyJob_ProDG.UI.Services.DialogServices
{
    /// <summary>
    /// Interface to service to display Windows in dialog mode with or without binding to ViewModel 
    /// and without any additional functionality.
    /// </summary>
    internal interface IWindowDialogService
    {
        void CloseDialog(Window window);
        void ShowDialog(Window window);
        void ShowDialog<TViewModel>(Window window, TViewModel viewModel) where TViewModel : class, new();
    }
}