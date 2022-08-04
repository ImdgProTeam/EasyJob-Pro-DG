using System;
using System.Collections.Generic;
using System.Windows;

namespace EasyJob_ProDG.UI.Services.DialogServices
{
    public class DialogWindowService : IDialogWindowService
    {
        private readonly Window owner;

        public IDictionary<Type, Type> Mappings;

        public DialogWindowService(Window owner)
        {
            this.owner = owner;
            Mappings = new Dictionary<Type, Type>();
        }
        public void Register<TViewModel, TView>() where TViewModel : IDialogWindowRequestClose where TView : IDialogWindow
        {
            if (Mappings.ContainsKey(typeof(TViewModel)))
            {
                throw new ArgumentException($"Type {typeof(TViewModel)} is already mapped to type {typeof(TView)}");
            }

            Mappings.Add(typeof(TViewModel), typeof(TView));
        }

        public bool? ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : IDialogWindowRequestClose
        {
            Type viewType = Mappings[typeof(TViewModel)];
            IDialogWindow dialog = (IDialogWindow)Activator.CreateInstance(viewType);
            EventHandler<DialogWindowCloseRequestedEventArgs> handler = null;
            handler = (sender, e) =>
            {
                viewModel.CloseRequested -= handler;

                dialog.DialogResult = e.DialogResult;
                //dialog.Close();

            };
            viewModel.CloseRequested += handler;

            dialog.DataContext = viewModel;
            dialog.Owner = owner;

            return dialog.ShowDialog();
        }
    }
}
