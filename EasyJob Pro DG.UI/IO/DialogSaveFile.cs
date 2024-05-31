using Microsoft.Win32;

namespace EasyJob_ProDG.UI.IO
{
    public static class DialogSaveFile
    {
        /// <summary>
        /// Opens dialog to save a condition file
        /// </summary>
        /// <param name="fileName">Returns given file name in out parameter</param>
        /// <returns>Returns true if file 'Save' button pressed</returns>
        public static bool SaveFileWithDialog(ref string fileName)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "EasyJob Condition File (*.ejc)|*.ejc";
            dialog.OverwritePrompt = true;
            dialog.AddExtension = true;
            dialog.FileName = fileName;

            bool result = (bool)dialog.ShowDialog();
            fileName = dialog.FileName;

            return result;
        }
    }
}
