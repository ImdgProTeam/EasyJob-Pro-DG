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
            dg.Clear(dg.Unno);
            if (!Validators.UnnoValidator.Validate(dg.Unno)) return;
            
            dg.AssignFromDgList(unitIsNew: true);
            dg.AssignSegregationGroup();
        }

        public static void UpdateOnlyNonchangeableDgInfo(this Dg dg)
        {
            if (!Validators.UnnoValidator.Validate(dg.Unno)) return;
            dg.AssignFromDgListOnlyNonChangeableProperties();
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
        /// Defines compatibility group for segregation of class 1
        /// </summary>
        internal static void DefineCompatibilityGroup(this Dg dg)
        {
            char group = '0';
            foreach (string s in dg.AllDgClasses)
                if (s.StartsWith("1") && group == '0')
                    group = s.Length > 3 ? char.ToUpper(s[3]) : '0';

            dg.CompatibilityGroup = group;
        }

        /// <summary>
        /// For new units being imported - updating missing info from DgList
        /// </summary>
        /// <param name="dgToUpdate"></param>
        internal static void UpdateMissingDgInfo(this Dg dgToUpdate)
        {
            if (dgToUpdate.Unno <= 1) return;

            var dgFromImdgCode = new Dg()
            {
                Unno = dgToUpdate.Unno,
                PackingGroupAsByte = dgToUpdate.PackingGroupAsByte
            };
            dgFromImdgCode.AssignFromDgList();

            if (dgToUpdate.PackingGroupAsByte == 0 && dgFromImdgCode.PackingGroupAsByte != 0)
                dgToUpdate.PackingGroupAsByte = dgFromImdgCode.PackingGroupAsByte;
            if (string.IsNullOrEmpty(dgToUpdate.DgEMS))
                dgToUpdate.DgEMS = dgFromImdgCode.DgEMS;
            if (!dgToUpdate.mpDetermined && !dgToUpdate.IsMp)
                dgToUpdate.IsMp = dgFromImdgCode.IsMp;

            dgToUpdate.SetIMDGCodeValues(dgFromImdgCode);

            if (string.IsNullOrEmpty(dgToUpdate.Name))
                dgToUpdate.Name = dgFromImdgCode.Name;

            if (string.IsNullOrEmpty(dgToUpdate.SegregationGroup))
                dgToUpdate.AssignSegregationGroup();
        }
    }
}
