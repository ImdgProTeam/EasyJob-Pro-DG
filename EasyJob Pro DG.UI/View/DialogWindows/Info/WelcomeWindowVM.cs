using EasyJob_ProDG.UI.Services.DialogServices;
using EasyJob_ProDG.UI.Utility;
using System;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.View.DialogWindows
{
    public class WelcomeWindowVM : IDialogWindowRequestClose
    {
        public string Message1 { get; }
            = "Welcome to EasyJob ProDG Pro!";
        public string Message2 { get; }
            = "     Thank you for using this software!";
        public string Message3 { get; }
            = "     It will assist you in checking of stowage and segregation of dangerous cargo, managing reefers," +
            " as well as other cargo planning tasks.";
        public string Message4 { get; }
            = "     We hope you will find it helpful and will enjoy using it.";
        public string Message5 { get; }
            = "     ProDG Pro is not a complete product and our team is still working to make it better and more user-frienly.";
        public string Message6 { get; }
            = "     Therefore we value all your comments and suggestions, and ask you to inform us if you have any or if you experience troubles while using ProDG Pro.";
        public string Signature { get; } = "imdg.pro team";

        public string Suggestion { get; } = "We suggest to start your work with setting up your Ship profile.";
        public string Contacts1 { get; } = "Please visit our web site ";
        public string Contacts2 { get; } = "for the latest information of our services and software.";

        public string HyperlinkText { get; } = "imdg.pro";
        public string HyperlinkUri => $"http://imdg.pro";
        public string Disclamer { get; } = "Remember: Final decision is always behind the user!";


        public WelcomeWindowVM()
        {
            OpenShipProfileSettingsCommand = new DelegateCommand(OpenShipProfileSettingsCommandOnExecuted);
        }



        private void OpenShipProfileSettingsCommandOnExecuted(object obj)
        {
            CloseRequested?.Invoke(this, new DialogWindowCloseRequestedEventArgs(true));
        }

        public ICommand OpenShipProfileSettingsCommand { get; }
        public event EventHandler<DialogWindowCloseRequestedEventArgs> CloseRequested;
    }
}
