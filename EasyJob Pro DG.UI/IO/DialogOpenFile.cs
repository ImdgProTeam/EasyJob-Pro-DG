using EasyJob_ProDG.Model.IO;
using Microsoft.Win32;
using System;
using System.Windows;
using EasyJob_ProDG.UI.Services.DialogServices;

namespace EasyJob_ProDG.UI.IO
{
    public static class DialogOpenFile
    {
        private static IMessageDialogService _messageDialogService => MessageDialogService.Connect();

        /// <summary>
        /// Opens dialog and returns file name and type as 'out' parameters.
        /// </summary>
        /// <param name="parameter">Owner window</param>
        /// <param name="file">Opened file name</param>
        /// <returns>True if file opened successfully</returns>
        public static bool OpenFileWithDialog(object parameter, out string file)
        {
            var s = parameter as Window;

            OpenFileDialog dlg = new OpenFileDialog
            {
                DefaultExt = ".edi",
                Filter = "Program file (*.edi, *.ejc, *.xls)|*.edi;*.xlsx;*.xls;*.ejc|All files (*.*)|*.*",
                CheckFileExists = true,
                Multiselect = false
            };

            if ((bool)dlg.ShowDialog(s))
            {
                try
                {
                    //Open file and check
                    file = dlg.FileName;
                    if (ConfirmFileType(file)) return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not load file " + ex.Message, s?.Title, MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
                }
            }

            file = null;
            return false;
        }

        /// <summary>
        /// Opens dialog and returns excel file name and type as 'out' parameters.
        /// </summary>
        /// <param name="parameter">Owner window</param>
        /// <param name="file">Opened excel file name</param>
        /// <returns>True if file opened successfully</returns>
        public static bool OpenExcelFileWithDialog(object parameter, out string file)
        {
            var s = parameter as Window;

            OpenFileDialog dlg = new OpenFileDialog
            {
                DefaultExt = ".xlsx",
                Filter = "Excel file (*.xls, *.xlsx)|*.xlsx;*.xls|All files (*.*)|*.*",
                CheckFileExists = true,
                Multiselect = false
            };

            if ((bool)dlg.ShowDialog(s))
            {
                try
                {
                    //Open file and check
                    file = dlg.FileName;
                    if (ConfirmFileType(file, OpenFile.FileTypes.Excel)) return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Could not load file " + ex.Message, s?.Title, MessageBoxButton.OK,
                        MessageBoxImage.Exclamation);
                }
            }

            file = null;
            return false;
        }

        /// <summary>
        /// Checks file type and confirms with user to open other type of file.
        /// </summary>
        /// <param name="file">Any file name with extension</param>
        /// <param name="fileType">If specific file type required to be confirmed. If not mentioned - all readable files will be checked.</param>
        /// <returns>True if file is confirmed to be opened</returns>
        public static bool ConfirmFileType(string file, OpenFile.FileTypes fileType = OpenFile.FileTypes.None)
        {
            string fileExt = OpenFile.GetExtension(file);
            var ftype = OpenFile.DefineFileType(fileExt);

            if (fileType == ftype)
            {
                return true;
            }
            else if (ftype != OpenFile.FileTypes.Other || OtherTypeDialog())
            {
                return true;
            }

            return false;
        }



        /// <summary>
        /// Shows dialog to choose whether to open unknown type of file
        /// </summary>
        /// <returns>True if pressed 'Yes'</returns>
        private static bool OtherTypeDialog()
        {
            return _messageDialogService.ShowYesNoDialog(
                "The file you are trying to open is not a recognized file type. Do you still want to open it?",
                "Attention") == MessageDialogResult.Yes;
        }
    }
}
