using Microsoft.Win32;
using System;
using System.Windows;

namespace EasyJob_ProDG.UI.Data
{
    public static class DialogOpenSettings
    {
        /// <summary>
        /// Opens dialog and returns file name as 'out' parameters.
        /// </summary>
        /// <param name="file">Opened file name</param>
        /// <returns>True if file opened successfully</returns>
        public static bool OpenSettingsFileWithDialog(out string file)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                DefaultExt = ".xml",
                Filter = "Settings file (*.xml)|*.xml;*.xls|All files (*.*)|*.*",
                CheckFileExists = true,
                Multiselect = false
            };

            if ((bool)dlg.ShowDialog())
            {
                try
                {
                    file = dlg.FileName;
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not load file " + ex.Message, "Error", MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
                }
            }

            file = null;
            return false;
        }
    }
}
