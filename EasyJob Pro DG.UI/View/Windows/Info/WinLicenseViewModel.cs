using EasyJob_ProDG.Data;
using EasyJob_ProDG.UI.Utility;
using System;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.View.DialogWindows
{
    public class WinLicenseViewModel
    {
        /// <summary>
        /// End date and time of current license.
        /// </summary>
        public static DateTime LicenseValidity => License.EndLicense;

        /// <summary>
        /// Default constructor
        /// </summary>
        public WinLicenseViewModel() 
        {

        }

        /// <summary>
        /// Shows the agreement window
        /// </summary>
        public ICommand ShowAgreement => new DelegateCommand(ShowAgreementOnExecuted);
        private void ShowAgreementOnExecuted(object obj)
        {
            ViewModelLocator.MainWindowViewModel.ShowLicenseAgreement();
        }
    }
}
