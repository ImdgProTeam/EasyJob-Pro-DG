using Microsoft.Win32;

namespace EasyJob_ProDG.UI.Data
{
    public static class DialogSaveSettings
    {
        /// <summary>
        /// Opens dialog to save a settings file
        /// </summary>
        /// <param name="fileName">Proposed file name</param>
        /// <returns>Returns true if file 'Save' button pressed</returns>
        internal static bool SaveSettingsToFileWithDialog(out string fileName)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "settings file (*.xml)|*.xml";
            dialog.OverwritePrompt = true;
            dialog.AddExtension = true;
            dialog.FileName = "ProDG settings";

            bool result = (bool)dialog.ShowDialog();
            fileName = dialog.FileName;
            return result;
        }
    }
}
