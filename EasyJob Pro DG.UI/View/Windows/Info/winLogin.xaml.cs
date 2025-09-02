using EasyJob_ProDG.UI.Security;
using EasyJob_ProDG.UI.Services.DialogServices;
using EasyJob_ProDG.UI.View.WindowBase;
using System.Security;

namespace EasyJob_ProDG.UI.View.DialogWindows
{
    /// <summary>
    /// Логика взаимодействия для winLogin.xaml
    /// </summary>
    public partial class winLogin : AnimatedWindow, IDialogWindow, IHavePassword
    {
        public winLogin()
        {
            InitializeComponent();
        }

        public SecureString SecurePassword => txtPassword.SecurePassword;
    }
}
