using EasyJob_ProDG.Data.Info_data;

namespace EasyJob_ProDG.Model.Cargo
{
    /// <summary>
    /// Class to assign or update various <see cref="Dg"/> properties
    /// </summary>
    public static class HandleDg
    {
        /// <summary>
        /// Method will reset available data and update information from dataBase
        /// on a selected dg, if Unno is recognized
        /// </summary>
        /// <param name="dg"></param>
        public static void UpdateDgInfo(this Dg dg)
        {
            if (!Validators.UnnoValidator.Validate(dg.Unno)) return;
            
            dg.Clear(dg.Unno);
            dg.AssignFromDgList(unitIsNew: true);
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

        /// <summary>
        /// Will define row number in IMDG Code segregation table and assign it to DgRowInSegregationTable
        /// </summary>
        internal static void AssignSegregationTableRowNumber(this Dg dg)
        {
            dg.DgRowInSegregationTable = IMDGCode.AssignSegregationTableRowNumber(dg.DgClass);
        }

        /// <summary>
        /// Defines compatibility group for segregation of class 1
        /// </summary>
        internal static void DefineCompatibilityGroup(this Dg dg)
        {
            foreach (string s in dg.AllDgClasses)
                if (s.StartsWith("1"))
                    dg.CompatibilityGroup = s.Length > 3 ? char.ToUpper(s[3]) : '0';
        }
    }
}
