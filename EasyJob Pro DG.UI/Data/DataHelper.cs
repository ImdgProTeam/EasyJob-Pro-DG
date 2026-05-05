using EasyJob_ProDG.Model.Validators;
using EasyJob_ProDG.UI.Services.DialogServices;
using System.Linq;

namespace EasyJob_ProDG.UI.Data
{
    internal static class DataHelper
    {
        private static IMessageDialogService _messageDialogService => MessageDialogService.Connect();
        private static ICurrentProgramData _currentProgramData => CurrentProgramData.GetCurrentProgramData();

        /// <summary>
        /// Checks if UN no exists in database and prompts user to confirm weather to continue if UN does not exist.
        /// </summary>
        /// <param name="unno">UN no being checked</param>
        /// <returns>If UN no is valid or user acknowledge</returns>
        internal static bool CheckForExistingUnno(ushort unno)
        {
            if (UnnoValidator.Validate(unno)) return true;
            if (_messageDialogService.ShowYesNoDialog(
                    $"UN no {unno:0000} does not exist in the DataBase. \nDo you wish to proceed?", "Attention!") ==
                MessageDialogResult.Yes) return true;
            return false;
        }

        /// <summary>
        /// Defines cargo hold number for given bay
        /// </summary>
        /// <param name="bay"></param>
        /// <returns></returns>
        internal static byte DefineCargoHoldNumber(byte bay)
        {
            return _currentProgramData.GetShipProfile().DefineCargoHoldNumberOfBay(bay);
        }

        /// <summary>
        /// Checks if container number in query already exists in the <see cref="CargoPlan"/> and advises user in the positive case.
        /// </summary>
        /// <param name="containerNumber">Container number in query</param>
        /// <returns>True if no such container number found, false if already exists.</returns>
        internal static bool HandleContainerNumberAlreadyExists(string containerNumber)
        {
            if (_currentProgramData.CargoPlan.Containers.Any(c => string.Equals(c.ContainerNumber, containerNumber)))
            {
                _messageDialogService.ShowOkDialog($"Container number {containerNumber} already exists in the Cargo plan and cannot be set.", null);
                return false;
            }
            return true;
        }
    }
}
