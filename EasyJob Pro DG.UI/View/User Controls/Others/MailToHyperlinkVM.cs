using EasyJob_ProDG.Data;

namespace EasyJob_ProDG.UI.View.User_Controls
{
    internal class MailToHyperlinkVM
    {
        public static string HyperlinkUri =>
            $"mailto:feedback@imdg.pro?subject=ProDG Pro feedback (Version {ProgramDefaultSettingValues.ReleaseVersion})" +
            $"&body=/Please attach your .edi and ShipProfile.ini files in case you experience problems or found an error in cargo data/";
    }
}
