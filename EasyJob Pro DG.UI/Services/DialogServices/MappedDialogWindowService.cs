using System;
using System.Collections.Generic;
using System.Windows;

namespace EasyJob_ProDG.UI.Services.DialogServices
{
    /// <summary>
    /// Service to display windows in dialog mode with IDialogWindowRequestClose interface
    /// and with prior mapping to ViewModels.
    /// Mappings done in ServiceHandler.cs
    /// </summary>
    public class MappedDialogWindowService : IMappedDialogWindowService
    {
        private readonly Window owner;

        public IDictionary<Type, Type> Mappings;

        /// <summary>
        /// Service constructor.
        /// </summary>
        /// <param name="owner">Window which will be set as an owner to all further created DialogWindows.</param>
        public MappedDialogWindowService(Window owner)
        {
            this.owner = owner;
            Mappings = new Dictionary<Type, Type>();
        }

        /// <summary>
        /// Registers mapping of View (windows) with ViewModels.
        /// </summary>
        /// <typeparam name="TViewModel">Should implement <see cref="IDialogWindowRequestClose"/> interface.</typeparam>
        /// <typeparam name="TView">Should implement <see cref="IDialogWindow"/> interface.</typeparam>
        /// <exception cref="ArgumentException"></exception>
        public void Register<TViewModel, TView>() where TViewModel : IDialogWindowRequestClose where TView : IDialogWindow
        {
            if (Mappings.ContainsKey(typeof(TViewModel)))
            {
                throw new ArgumentException($"Type {typeof(TViewModel)} is already mapped to type {typeof(TView)}");
            }

            Mappings.Add(typeof(TViewModel), typeof(TView));
        }

        /// <summary>
        /// Displays window in dialog mode.
        /// </summary>
        /// <typeparam name="TViewModel">Should implement <see cref="IDialogWindowRequestClose"/> interface.</typeparam>
        /// <param name="viewModel">ViewModel which has prior mapping to view via <see cref="Register{TViewModel, TView}"/> method.</param>
        /// <returns></returns>
        public bool? ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : IDialogWindowRequestClose
        {
            Type viewType = Mappings[typeof(TViewModel)];
            IDialogWindow dialog = (IDialogWindow)Activator.CreateInstance(viewType);
            EventHandler<DialogWindowCloseRequestedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                viewModel.CloseRequested -= handler;
                dialog.DialogResult = e.DialogResult;
            };
            viewModel.CloseRequested += handler;

            dialog.DataContext = viewModel;
            dialog.Owner = owner;

            return dialog.ShowDialog();
        }
    }
}
