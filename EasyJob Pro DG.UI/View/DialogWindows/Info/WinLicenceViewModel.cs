using EasyJob_ProDG.Data;
using EasyJob_ProDG.UI.Utility;
using System;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.View.DialogWindows
{
    public class WinLicenceViewModel
    {
        /// <summary>
        /// End date and time of current licence.
        /// </summary>
        public static DateTime LicenceValidity => Licence.EndLicence;

        /// <summary>
        /// Default constructor
        /// </summary>
        public WinLicenceViewModel() 
        {

        }

        /// <summary>
        /// Shows the agreement window
        /// </summary>
        public ICommand ShowAgreement => new DelegateCommand(ShowAgreementOnExecuted);
        private void ShowAgreementOnExecuted(object obj)
        {
            ViewModelLocator.MainWindowViewModel.ShowLicenceAgreement();
        }
    }
}
