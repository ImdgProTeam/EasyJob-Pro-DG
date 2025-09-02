using System;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Input;
using EasyJob_ProDG.UI.Security;
using EasyJob_ProDG.UI.Services.DialogServices;
using EasyJob_ProDG.UI.Utility;

namespace EasyJob_ProDG.UI.View.DialogWindows
{
    public class WinLoginViewModel : IDialogWindowRequestClose
    {
        public string Email { get; set; }
        public SecureString Password { get; set; }



        public WinLoginViewModel()
        {
            LoginCommand = new DelegateCommand(async (parameter) => await Login(parameter));
            CancelCommand = new DelegateCommand(p => CloseRequested?.Invoke(this, new DialogWindowCloseRequestedEventArgs(false)));
        }


        /// <summary>
        /// Attempts to log the user in
        /// </summary>
        /// <param name="parameter">The <see cref="SecureString"/> passed in from the view for the users password.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task Login(object parameter)
        {
            var pass = (parameter as IHavePassword).SecurePassword.Unsecure();
        }








        #region IDialogWindowRequestClose

        public event EventHandler<DialogWindowCloseRequestedEventArgs> CloseRequested;

        public ICommand LoginCommand { get; }
        public ICommand CancelCommand { get; }

        public SecureString SecurePassword => throw new NotImplementedException();

        #endregion

    }
}
