using System.Security;

namespace EasyJob_ProDG.UI.Security
{
    public interface IHavePassword
    {
        SecureString SecurePassword { get; }
    }
}
