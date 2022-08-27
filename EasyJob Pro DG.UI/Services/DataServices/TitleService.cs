using EasyJob_ProDG.Data;
using EasyJob_ProDG.UI.Data;
using System;
using System.Text;

namespace EasyJob_ProDG.UI.Services.DataServices
{
    internal class TitleService : ITitleService
    {
        private ICurrentProgramData _currentProgramData = CurrentProgramData.GetCurrentProgramData();

        public string GetTitle()
        {
            StringBuilder header = new StringBuilder();
            string version = ProgramDefaultSettingValues.ReleaseVersion;
            string programName = ProgramDefaultSettingValues.ProgramTitle;
            string shipName = _currentProgramData.GetShipProfile().ShipName;
            string conditionFile = _currentProgramData.ConditionFileName;

            header.Append($"{programName} v {version}");
            header.Append($"     {shipName}");
            header.Append($"     {conditionFile}");

            return header.ToString();
        }

        public void UpdateTitle()
        {
            throw new NotImplementedException();
        }
    }
}
