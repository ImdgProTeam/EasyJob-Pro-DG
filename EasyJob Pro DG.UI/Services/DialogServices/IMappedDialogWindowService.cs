namespace EasyJob_ProDG.UI.Services.DialogServices
{
    /// <summary>
    /// Interface to service to display windows in dialog mode with IDialogWindowRequestClose interface
    /// and with prior mapping to ViewModels.
    /// </summary>
    public interface IMappedDialogWindowService
    {
        void Register<TViewModel, TView>() where TViewModel : IDialogWindowRequestClose
                                           where TView : IDialogWindow;

        bool? ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : IDialogWindowRequestClose;
    }
}
