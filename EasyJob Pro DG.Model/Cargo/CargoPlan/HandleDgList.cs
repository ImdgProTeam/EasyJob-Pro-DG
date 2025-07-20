using EasyJob_ProDG.Model.IO;
using System.Collections.Generic;

namespace EasyJob_ProDG.Model.Cargo
{
    public static class HandleDgList
    {
        //------------------- Supportive methods to work with Dg list --------------------------------------------------------------

        /// <summary>
        /// Method applies the information from all available sources to units in dg list.
        /// For .ejc condition file will change only fixed (unchangeable) information from IMDG Code.
        /// </summary>
        /// <param name="dgList"></param>
        internal static void UpdateDgInfo(this ICollection<Dg> dgList, OpenFile.FileTypes fileType)
        {
            foreach (Dg unit in dgList)
            {
                if (fileType == OpenFile.FileTypes.Ejc)
                    unit.UpdateOnlyNonchangeableDgInfo();
                else
                    unit.UpdateDgInfo();
            }
            Data.LogWriter.Write($"Dg info updated.");
        }

        /// <summary>
        /// Method searches for duplicate records and safely removes them. Wrong DG classes will be checked and option will be proposed to ammend it in accordance with IMDG code.
        /// </summary>
        /// <param name="dgList"></param>
        /// <param name="fileType"></param>
        public static void CheckDgList(this ICollection<Dg> dgList, byte fileType)
        {
            //TODO: To implement checking of wrong Dg classes

            if (fileType != (byte)OpenFile.FileTypes.Edi) return;
            //WrongDGInfoDisplay(dgList);
            Data.LogWriter.Write($"Dg list checked.");
        }

        /// <summary>
        /// Method checks the segregation between all the units in dg list and adds a conflict to the units if any.
        /// </summary>
        /// <param name="cargoPlan"></param>
        /// <param name="ownShip"></param>
        /// <param name="reefers"></param>
        public static void CheckSegregation(this CargoPlan cargoPlan)
        {
            Segregation.AssignSegregatorClassesAndGroups(cargoPlan.DgList);
            foreach (Dg dg in cargoPlan.DgList)
            {
                Segregation.Segregate(dg, cargoPlan);
            }
            Segregation.PostSegregation(cargoPlan);
            Data.LogWriter.Write($"Segregation checked.");
        }
    }
}