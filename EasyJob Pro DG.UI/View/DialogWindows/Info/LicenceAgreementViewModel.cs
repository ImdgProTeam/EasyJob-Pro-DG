using EasyJob_ProDG.Data;
using EasyJob_ProDG.UI.Utility;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.View.DialogWindows
{
    public class LicenceAgreementViewModel
    {
        //Public properties

        public string LicenceText => Licence.LicenceText;

        /// <summary>
        /// States if the Agreement is shown in the first start
        /// </summary>
        public bool IsFirstStart { get; set; }

        /// <summary>
        /// <see langword="true"/> if Accepted, <see langword="false"/> if Declined.
        /// </summary>
        internal bool? Result { get; private set; }

        //Constructors

        /// <summary>
        /// Default constructor with <see cref="IsFirstStart"/> set to 'true'
        /// </summary>
        public LicenceAgreementViewModel()
        {
            IsFirstStart = true;
        }

        /// <summary>
        /// Default constructor with explicit setter of <see cref="IsFirstStart"/> property.
        /// </summary>
        /// <param name="isFirstStart"><see cref="IsFirstStart"/></param>
        public LicenceAgreementViewModel(bool isFirstStart)
        {
            IsFirstStart = isFirstStart;
        }


        //Command methods

        private void AcceptCommandOnExecuted(object obj)
        {
            Result = true;
        }

        private void DeclineCommandOnExecuted(object obj)
        {
            Result = false;
        }


        //Commands
        public ICommand DeclineCommand => new DelegateCommand(DeclineCommandOnExecuted);
        public ICommand AcceptCommand => new DelegateCommand(AcceptCommandOnExecuted);

    }
}
