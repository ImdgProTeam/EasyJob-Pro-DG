using EasyJob_ProDG.Data;
using EasyJob_ProDG.UI.Utility;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.View.DialogWindows
{
    public class WinAboutViewModel
    {
        public static string Description 
            => "Software is designed for automatic checking of stowage and segregation of dangerous goods on container ships in accordance with the requirements of IMDG Code " 
            + ProgramDefaultSettingValues.codeAmmendmentVersion;

        public static string ProgramVersion
            => "EasyJob ProDG Pro version " + ProgramDefaultSettingValues.ReleaseVersion;

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
