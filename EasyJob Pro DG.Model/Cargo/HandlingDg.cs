using EasyJob_ProDG.Model.IO;
using System.Collections.Generic;
using System.Xml.Linq;

namespace EasyJob_ProDG.Model.Cargo
{
    public static class HandlingDg
    {
        //------------------- Supportive methods to work with Dg list --------------------------------------------------------------

        /// <summary>
        /// Method searches for duplicate records and safely removes them. Wrong DG classes will be checked and option will be proposed to ammend it in accordance with IMDG code.
        /// </summary>
        /// <param name="dgList"></param>
        /// <param name="fileType"></param>
        public static void CheckDgList(ICollection<Dg> dgList, byte fileType)
        {
            if (fileType != (byte)OpenFile.FileTypes.Edi) return;
            //WrongDGInfoDisplay(dgList);
            //EnterLog(logStreamWriter, "Dg list checked");
        }

        /// <summary>
        /// Method checks the segregation between all the units in dg list and adds a conflict to the units if any.
        /// </summary>
        /// <param name="cargoPlan"></param>
        /// <param name="ownShip"></param>
        /// <param name="reefers"></param>
        public static void CheckSegregation(CargoPlan cargoPlan, Transport.ShipProfile ownShip)
        {
            Segregation.AssignSegregatorClassesAndGroups(cargoPlan.DgList);
            foreach (Dg dg in cargoPlan.DgList)
            {
                Segregation.Segregate(dg, cargoPlan, ownShip);
            }
            Segregation.PostSegregation(cargoPlan, ownShip);
        }

        /// <summary>
        /// Method applies the information from all available sources to units in dg list
        /// </summary>
        /// <param name="dgList"></param>
        /// <param name="dgDataBase"></param>
        public static void UpdateDgInfo(ICollection<Dg> dgList, XDocument dgDataBase)
        {
            foreach (Dg unit in dgList)
            {
                unit.AssignFromDgList(xmlDoc: dgDataBase);
                unit.AssignRowFromDOC();
            }
            //EnterLog(logStreamWriter, "Dg info updated");
        }

        /// <summary>
        /// Method will reset available data and update information from dataBase
        /// on a selected dg, if Unno is recognized
        /// </summary>
        /// <param name="dg"></param>
        /// <param name="dgDataBase"></param>
        public static void UpdateDgInfo(Dg dg, XDocument dgDataBase)
        {
            dg.Clear(dg.Unno);
            
            if (!Validators.UnnoValidator.Validate(dg.Unno)) return;
            dg.AssignFromDgList(xmlDoc: dgDataBase, unitIsNew: true);
            dg.AssignRowFromDOC();
            dg.AssignSegregationGroup();
        }

    }
}
