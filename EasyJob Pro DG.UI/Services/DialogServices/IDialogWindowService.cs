namespace EasyJob_ProDG.UI.Services.DialogServices
{
    public interface IDialogWindowService
    {
        void Register<TViewModel, TView>() where TViewModel : IDialogWindowRequestClose
                                           where TView : IDialogWindow;

        bool? ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : IDialogWindowRequestClose;
    }
}
