using EasyJob_ProDG.Data.Info_data;
using System.Collections.Generic;
using System.Xml.Linq;

namespace EasyJob_ProDG.Model.Cargo
{
    public static class HandleDg
    {

        /// <summary>
        /// Method applies the information from all available sources to units in dg list
        /// </summary>
        /// <param name="dgList"></param>
        /// <param name="dgDataBase"></param>
        internal static void UpdateDgInfo(this ICollection<Dg> dgList, XDocument dgDataBase)
        {
            foreach (Dg unit in dgList)
            {
                unit.AssignFromDgList(xmlDoc: dgDataBase);
                unit.AssignRowFromDOC();
            }
            Data.LogWriter.Write($"Dg info updated.");
        }

        /// <summary>
        /// Method will reset available data and update information from dataBase
        /// on a selected dg, if Unno is recognized
        /// </summary>
        /// <param name="dg"></param>
        /// <param name="dgDataBase"></param>
        internal static void UpdateDgInfo(this Dg dg, XDocument dgDataBase)
        {
            dg.Clear(dg.Unno);

            if (!Validators.UnnoValidator.Validate(dg.Unno)) return;
            dg.AssignFromDgList(xmlDoc: dgDataBase, unitIsNew: true);
            dg.AssignRowFromDOC();
            dg.AssignSegregationGroup();
        }

        internal static void AssignSegregationGroup(this Dg dg)
        {
            var segregationGroupList = IMDGCode.AssignSegregationGroup(dg.Unno);
            if (segregationGroupList.Count > 0)
            {
                foreach (var group in segregationGroupList)
                    dg.SegregationGroupByte = group;
            }
        }
    }
}
