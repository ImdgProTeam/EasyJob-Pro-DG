using EasyJob_ProDG.Data;

namespace EasyJob_ProDG.UI.Services
{
    internal class FileNameService : IFileNameService
    {
        ServicesHandler Services => ServicesHandler.GetServicesAccess();

        /// <summary>
        /// Suggests the fileName when saving condition based on voyage info.
        /// </summary>
        /// <returns></returns>
        public string GetSuggestedFileName()
        {
            string suggestion = string.Empty;
            var conditionName = Services.CargoDataServiceAccess.ConditionFileName;
            if (conditionName.EndsWith(ProgramDefaultSettingValues.ConditionFileExtension) && !string.Equals(conditionName, Properties.Settings.Default.WorkingCargoPlanFile))
                return conditionName;

            conditionName = conditionName.ToUpper().Replace(" ", "").Replace("-", "").Replace("_", "");
            if (!string.IsNullOrEmpty(conditionName))
            {
                if (conditionName.Contains("PREFINAL"))
                {
                    suggestion = "Pre-Final";
                }
                else if (conditionName.Contains("FINAL"))
                {
                    suggestion = "Final";
                }
                else if (conditionName.Contains("PRESTOW"))
                {
                    suggestion = "Prestow";
                }
                else if (conditionName.Contains("STOWAGE"))
                {
                    suggestion = "Stowage";
                }
                else if (conditionName.Contains("LOAD"))
                {
                    suggestion = "Load";
                }
                else if (conditionName.Contains("PRE"))
                {
                    suggestion = "Prestow";
                }
                if (conditionName.Contains("UPDATED"))
                {
                    suggestion += "Updated";
                }
                else if (conditionName.Contains("UPDATE"))
                {
                    suggestion += "Update";
                }
                else if (conditionName.Contains("CORRECTED"))
                {
                    suggestion += "Corrected";
                }
                else if (conditionName.Contains("CORRECT"))
                {
                    suggestion += "Correct";
                }
            }

            return Services.CargoDataServiceAccess.WorkingCargoPlan.VoyageInfo.VoyageNumber + " "
            + Services.CargoDataServiceAccess.WorkingCargoPlan.VoyageInfo.PortOfDeparture + " "
            + suggestion;
        }

    }
}
