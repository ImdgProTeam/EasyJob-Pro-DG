using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;
using Microsoft.Win32;

namespace EasyJob_Pro_DG
{
    public class OpenFile
    {

 

        public static byte fileType;

        public enum FileTypes : byte
        {
            EDI = 0,
            EXCEL,
            XML
        }

        /// <summary>
        /// Method determines if any file was dragged & dropped then checks its extension.
        /// Otherwise it will give the full list of acceptable files in program directory and open the chosen file.
        /// Method returns file name and location. It also records File Type to fileType variable.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string Open(string[] args)
        {
            string fileName = null;

            ///Dropped file
            if (args.Length != 0)
            {
                if (args[0].ToLower().EndsWith(".edi"))
                {
                    fileName = args[0];
                    fileType = (byte) FileTypes.EDI;
                }
                else if (args[0].ToLower().EndsWith(".xls") || args[0].ToLower().EndsWith(".xlsx"))
                {
                    fileName = args[0];
                    fileType = (byte) FileTypes.EXCEL;
                }
                else
                {
                    string msg =
                        $"File {args[0]} is of not recognized extension. \nPlease ensure you are trying to open .edi file...";
                    MessageBox.Show(msg);
                }
            }

            ///Open file
            if (fileName != null)
            {
                //appending text for final export file
                ProgramFiles.textToExport.Append("\n\nWorking with ")
                    .AppendLine(fileName.Substring(fileName.LastIndexOf('\\') + 1));
                ProgramFiles.textToExport.AppendFormat("File type: {0}", (FileTypes) fileType).AppendLine();
               
            }

            return fileName;
        }
    }
}

