using EasyJob_ProDG.Data;

namespace EasyJob_ProDG.Model.Transport
{
    partial class ShipProfile
    {
        //-------------------------Methods used in creation of Ship Profile---------------------------------------

        // -------------- static -------------------------------------------------

        private static string OpenShipProfile(string shipFile, bool openDefault)
        {
            //return default ship profile if option set
            if (openDefault) return ProgramDefaultSettingValues.ProgramDirectory + shipFile;
            return null;
        }

        ///// <summary>
        ///// Method will make requested changes and return default ship profile
        ///// </summary>
        ///// <returns></returns>
        //public static ShipProfile ChangeShipProfile()
        //{
        //    return ChangeShipProfile(ProgramDefaultSettingValues.ProgramDirectory + ProgramDefaultSettingValues.DefaultShipProfile);
        //}

        //private static ShipProfile ChangeShipProfile(string fileName)
        //{
        //    string tempFileName = fileName.Substring(fileName.LastIndexOf('\\') + 1).Replace(ProgramDefaultSettingValues.ShipProfileExtension, "");
        //    ShipProfile tempShip = new ShipProfile(fileName);

        //    WriteShipProfile(tempFileName, tempShip);
        //    Output.ThrowMessage("Ship parameters have been saved.\nPress any key to continue...");
        //    return tempShip;
        //}

        //private static ShipProfile CreateShipProfile(bool multiprofile)
        //{
        //    //TO BE IMPLEMENTED IN WPF
        //    var ship = new ShipProfile();
        //    return ship;
        //}

    }
}
