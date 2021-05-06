using System.Windows;


namespace EasyJob_Pro_DG
{
    partial class ShipProfile
    {
        //-------------------------Methods used in creation of Ship Profile---------------------------------------

        // -------------- static -------------------------------------------------

        private static string OpenShipProfile(string shipFile, bool openDefault)
        {
            //return default ship profile if option set
            if (openDefault) return dir + shipFile;
            return null;
        }

        /// <summary>
        /// Method will make requested changes and return default ship profile
        /// </summary>
        /// <returns></returns>
        public static ShipProfile ChangeShipProfile()
        {
            return ChangeShipProfile(dir + defaultShipProfile);
        }

        private static ShipProfile ChangeShipProfile(string fileName)
        {
            string tempFileName = fileName.Substring(fileName.LastIndexOf('\\') + 1).Replace(shipProfileExtension, "");
            ShipProfile tempShip = new ShipProfile(fileName);

            WriteShipProfile(tempFileName, tempShip);
            MessageBox.Show("Ship parameters have been saved.\nPress any key to continue...");
            return tempShip;
        }

        private static ShipProfile CreateShipProfile(bool _multiprofile)
        {
            //TO BE IMPLEMENTED IN WPF
            ShipProfile ship;
            ship = new ShipProfile();
            return ship;
        }

    }
}
