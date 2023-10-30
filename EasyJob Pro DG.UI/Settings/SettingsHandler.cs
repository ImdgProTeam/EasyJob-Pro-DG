using EasyJob_ProDG.UI.Data;
using System;
using System.Threading.Tasks;

namespace EasyJob_ProDG.UI.Settings
{
    public class SettingsHandler
    {
        internal static void SaveSettings()
        {
            if (DialogSaveSettings.SaveSettingsToFileWithDialog())
            {
                Action d = delegate ()
                {
                    //loadDataService.SaveFile(fileName);
                };
                //Task.Run(() => WrapMethodWithIsLoading(d));
            }
        }

        internal static void RestoreSettings()
        {
            if (!DialogOpenSettings.OpenSettingsFileWithDialog(out var file))
            {
                //StatusBarControl.Cancel();
                return;
            }
            //_ = OpenFileWithOptionsChoiceAsync(file);
        }
    }
}
