using static EasyJob_ProDG.UI.Settings.UserUISettings;

namespace EasyJob_ProDG.UI.Settings
{
    public interface IUserUISettings
    {
        UserUISettings GetSettings();
        void LoadSettings();
        DgSortOrderPattern DgSortPattern { get; set; }
    }
}
