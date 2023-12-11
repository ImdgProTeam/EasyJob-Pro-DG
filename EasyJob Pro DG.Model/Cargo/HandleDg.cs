using EasyJob_ProDG.Data.Info_data;

namespace EasyJob_ProDG.Model.Cargo
{
    public static class HandleDg
    {
        /// <summary>
        /// Method will reset available data and update information from dataBase
        /// on a selected dg, if Unno is recognized
        /// </summary>
        /// <param name="dg"></param>
        public static void UpdateDgInfo(this Dg dg)
        {
            dg.Clear(dg.Unno);

            if (!Validators.UnnoValidator.Validate(dg.Unno)) return;
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
    }
}
