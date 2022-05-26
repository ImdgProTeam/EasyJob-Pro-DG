using System;
using System.Collections.Generic;
using System.IO;
using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.Model.IO.Excel;
using EasyJob_ProDG.Model.Transport;

namespace EasyJob_ProDG.Model.IO
{
    public static class OpenFile
    {
        public static string FileName { get; private set; }

        public enum FileTypes : byte
        {
            Edi = 0,
            Excel,
            Ejc,
            IFTDGN,
            Other,
            None
        }

        public enum OpenOption : byte
        {
            Open = 0,
            Update,
            Import
        }

        /// <summary>
        /// Defiles file type from extension or file name.
        /// Returns iftdgn type, if the same contained in the file name.
        /// </summary>
        /// <param name="fileExtensionOrName">Extension or file name</param>
        /// <returns></returns>
        public static FileTypes DefineFileType(string fileExtensionOrName)
        {
            FileTypes ftype;

            if (fileExtensionOrName.ToLower().Contains("iftdgn"))
                return FileTypes.IFTDGN;

            var _fileExt = fileExtensionOrName.Contains(".")
                ? GetExtension(fileExtensionOrName)
                : fileExtensionOrName;

            switch (_fileExt)
            {
                case "edi":
                    ftype = FileTypes.Edi;
                    break;
                case "xls":
                    ftype = FileTypes.Excel;
                    break;
                case "xlsx":
                    ftype = FileTypes.Excel;
                    break;
                case "ejc":
                    ftype = FileTypes.Ejc;
                    break;
                default:
                    ftype = FileTypes.Other;
                    break;
            }

            return ftype;
        }

        /// <summary>
        /// Creates CargoPlan from a given file.
        /// </summary>
        /// <param name="fileName">Full path and file name.</param>
        /// <param name="ownShip">Current ShipProfile.</param>
        /// <returns></returns>
        public static CargoPlan ReadCargoPlanFromFile(string fileName, ShipProfile ownShip)
        {
            var fileType = DefineFileType(fileName);
            CargoPlan cargoPlan = new CargoPlan();
            bool isIftdgn = false;

            switch (fileType)
            {
                //open .edi
                case FileTypes.Other:
                case FileTypes.Edi:
                    var edi = new ReadBaplieFile(fileName, ownShip, ref isIftdgn);
                    cargoPlan = edi.GetCargoPlan();
                    break;

                case FileTypes.IFTDGN:
                    isIftdgn = true;
                    cargoPlan = new ReadBaplieFile(fileName, ownShip, ref isIftdgn).GetCargoPlan();
                    break;

                //open excel
                case FileTypes.Excel:
                    cargoPlan.DgList = WithXl.Import(fileName, ownShip, out var containers);
                    cargoPlan.Containers = containers;
                    foreach (var c in cargoPlan.Containers)
                        if (c.IsRf)
                            cargoPlan.Reefers.Add(c);
                    break;

                //open ejc
                case FileTypes.Ejc:
                    CargoPlan cPlan = EasyJobCondition.EasyJobCondition.LoadCondition(fileName, ownShip);
                    cargoPlan.Containers = cPlan.Containers;
                    cargoPlan.DgList = cPlan.DgList;
                    cargoPlan.Reefers = cPlan.Reefers;
                    cargoPlan.VoyageInfo = cPlan.VoyageInfo;
                    break;

                //default
                default:
                    cargoPlan.DgList = new List<Dg>();
                    cargoPlan.Containers = new List<Container>();
                    break;
            }

            SetFileName(GetFileNameWithExtension(fileName));
            return cargoPlan;
        }

        /// <summary>
        /// Gets extension from fileName
        /// </summary>
        /// <param name="fileName">File name, either short or full path</param>
        /// <returns></returns>
        public static string GetExtension(string fileName)
        {
            string fileExt = fileName.Substring(fileName.LastIndexOf(".", StringComparison.Ordinal) + 1).ToLower();
            return fileExt;
        }

        /// <summary>
        /// Returns file name with extension and separated from path.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>File name with extension.</returns>
        public static string GetFileNameWithExtension(string fileName)
        {
            string result = fileName.Substring(fileName.LastIndexOf("\\", StringComparison.Ordinal) + 1);
            return result;
        }

        /// <summary>
        /// Changes FileName property.
        /// </summary>
        /// <param name="newFileName"></param>
        internal static void SetFileName(string newFileName)
        {
            FileName = newFileName;
        }

        /// <summary>
        /// Checks if given text file is a standard IFTDGN .edi file.
        /// </summary>
        /// <param name="fileName">Full path file name.</param>
        /// <returns></returns>
        internal static bool IsIftdgnEdi(string fileName)
        {
            var reader = new StreamReader(fileName);
            string text = reader.ReadToEnd();
            reader.Close();

            if (text.Contains("UNH") && text.Contains("IFTDGN")) return true;
            return false;
        }

    }
}

