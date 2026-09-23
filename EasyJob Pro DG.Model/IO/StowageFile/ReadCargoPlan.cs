using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.Model.IO.Excel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static EasyJob_ProDG.Model.IO.OpenFile;

namespace EasyJob_ProDG.Model.IO
{
    internal static class ReadCargoPlan
    {

        /// <summary>
        /// Creates CargoPlan from a given file.
        /// </summary>
        /// <param name="fileName">Full path and file name.</param>
        /// <param name="ownShip">Current ShipProfile.</param>
        /// <returns></returns>
        public static CargoPlan ReadCargoPlanFromFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName))
            {
                Data.LogWriter.Write($"File {fileName} cannot be found.");
                return null;
            }
            Data.LogWriter.Write($"Reading {fileName}...");

            var fileType = OpenFile.DefineFileType(fileName);
            CargoPlan cargoPlan = new CargoPlan();
            bool isIftdgn = fileType == FileTypes.IFTDGN;

            switch (fileType)
            {
                //open .edi
                case FileTypes.Other:
                case FileTypes.Edi:
                case FileTypes.IFTDGN:
                    ReadBaplieFile.ReadBaplie(fileName, ref isIftdgn);
                    cargoPlan = ReadBaplieFile.GetCargoPlan();
                    break;

                //open excel
                case FileTypes.Excel:
                    WithXlDg.Import(fileName, out var dgList, out var containers);
                    cargoPlan.DgList = dgList;
                    cargoPlan.Containers = containers.ToList();
                    foreach (var c in cargoPlan.Containers)
                        if (c.IsRf)
                            cargoPlan.Reefers.Add(c);
                    break;

                //open ejc
                case FileTypes.Ejc:
                    cargoPlan = EasyJobCondition.EasyJobCondition.LoadCondition(fileName);
                    break;

                case FileTypes.XML:
                    cargoPlan = ReadXMLStowageFile.ReadFile(fileName);
                    break;

                //default
                default:
                    cargoPlan.DgList = new List<Dg>();
                    cargoPlan.Containers = new List<Container>();
                    break;
            }

            OpenFile.SetFileName(OpenFile.GetFileNameWithExtension(fileName));
            Data.LogWriter.Write($"CargoPlan read from {OpenFile.FileName}");
            return cargoPlan;
        }
    }
}