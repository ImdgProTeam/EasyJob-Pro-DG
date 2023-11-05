using EasyJob_ProDG.Model.Cargo;
using System.Collections.Generic;
using System.Linq;

namespace EasyJob_ProDG.Model.IO
{
    public static class ImportUpdateExtensions
    {
        /// <summary>
        /// Updates List of Reefers with reefer info from manifestInfoReefers
        /// </summary>
        /// <param name="reefers">Reefers to be updated</param>
        /// <param name="manifestInfoReefers">Reefers to update info from</param>
        internal static void UpdateReeferManifestInfo(this IEnumerable<Container> reefers, IEnumerable<Container> manifestInfoReefers, bool importOnlySelected = false, string currentPort = null)
        {
            Container reefer;

            foreach (var unit in manifestInfoReefers)
            {
                reefer = reefers.FirstOrDefault(x => x.ContainerNumber == unit.ContainerNumber);

                if (reefer == null) continue;
                if (reefer.IsNotToImport) continue;
                if (importOnlySelected && !reefer.IsToImport) continue;
                if (currentPort != null && reefer.POL != currentPort) continue;

                reefer.Commodity = unit.Commodity;
                if (unit.SetTemperature != 0) reefer.SetTemperature = unit.SetTemperature;
                reefer.VentSetting = unit.VentSetting;
                reefer.ReeferSpecial = unit.ReeferSpecial;
                reefer.ReeferRemark = unit.ReeferRemark;

                reefer.HasUpdated = true;
            }

            Data.LogWriter.Write($"Reefer manifest info updated.");
        }
    }
}
