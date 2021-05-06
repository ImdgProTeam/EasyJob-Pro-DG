using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;


namespace EasyJob_Pro_DG
{
    partial class ShipProfile
    {

       
        // ------------------------------ Methods to work with ship profile ----------------------------------------------

        // ---------------------- non-static -----------------------------------------------------------------------------

        /// <summary>
        /// Method checks if any errors contained in loaded ship profile which may lead to fatal error further in program.
        /// Returns true if errors found.
        /// </summary>
        private bool CheckErrorsInShipProfile()
        {
            bool result = true;
            bool interResult = false;

            //accommodation
            if (numberOfAccommodations == 0 || Accommodation==null || Accommodation.Count == 0)
            {
                ErrorList = "accommodation";
                result = false;
            }
            //holds
            if (numberOfHolds != Holds.Length || numberOfHolds == 0)
            {
                ErrorList = "holds";
                result = false;
            }

            foreach (var hold in Holds)
            {
                if (hold == null)
                {
                    ErrorList = "holds";
                    result = false;
                    break;
                }
            }
            //seasides
            if (seaSides.Count == 0 || seaSides == null)
            {
                ErrorList = "seasides";
                result = false;
            }

            foreach (outerRow orow in seaSides)
            {
                if (orow.bay == 0)
                {
                    interResult = true;
                    break;
                }
            }

            if (!interResult)
            {
                ErrorList = "seasides";
                result = false;
            }

            //DOC
            if (doc.DOCtable.GetLength(0) != numberOfHolds + 1 && !errorList.Contains("holds"))
            {
                ErrorList = "doc";
                result = false;
            }

            if (result) errorList.Clear();
            return result;
        }

        
        // ---------------------- static ----------------------------------------------------------------------------------

        public static ShipProfile ReadShipProfile(string shipFile, bool openDefault)
        {
            MessageBoxResult result;
            string fileName = OpenShipProfile(shipFile, openDefault);
            if (!File.Exists(fileName))
            {
                //Case file not found
                result = MessageBox.Show("Ship configuration file not found.\nDo you wish to create it? Y/N?\n(It may take some time and will require from you detailed information regarding your ship)", "Ship profile", MessageBoxButton.YesNo);
                //Creation of a new file and commence working with it
                if (result == MessageBoxResult.Yes)
                    return CreateShipProfile(multiprofile);
                //Loading of default shipprofile
                MessageBox.Show("A default ship profile configuration will be loaded. That will affect the accuracy of stowage and segregation check.");
                return new ShipProfile();
            }
            //Case if file found and read
            ShipProfile ship = new ShipProfile(fileName);
            if (!ship.filecorrupt) return ship;
            //Errors encountered while reading the file
            result = MessageBox.Show("Ship settings file was read with errors. Would you like to change it? Y/N?\n(It may take some time and will require from you detailed information regarding your ship)", "Error message", MessageBoxButton.YesNo);
            //Option chosen to change configuration
            if (result == MessageBoxResult.Yes)
            {
                MessageBox.Show("Not implemented");
                return ship;
            }
            //Loading of default ship profile
            MessageBox.Show("A default ship profile configuration will be loaded. That will affect the accuracy of stowage and segregation check.");
            return new ShipProfile();
        }

      /// <summary>
        /// Method creates new profileName.ini file from ShipProfile ship
        /// </summary>
        /// <param name="profileName"></param>
        /// <param name="ship"></param>
        /// <returns></returns>
        private static void WriteShipProfile(string profileName, ShipProfile ship)
        {
            string fname = dir + profileName + shipProfileExtension;

            string addvalue;
            int i;

            using (StreamWriter writer = new StreamWriter(File.Create(fname)))
            {
                writer.WriteLine("***ShipProfile***");
                writer.WriteLine("");
                writer.WriteLine("//Caution! Any change to this file can also be done in text editor. Please do NOT change any value left of '=' sign as well as the order of fields, as it will corrupt the file. The accuracy of the information will affect stowage and segregation checks.\n");
                writer.WriteLine("");
                writer.WriteLine("Profile name = " + profileName);
                writer.WriteLine("Ship name = " + ship.shipName);
                writer.WriteLine("Call sign = " + ship.CallSign);
                writer.WriteLine("//General");
                writer.WriteLine("Passenger = " + ship.passenger);
                writer.WriteLine("Number of accommodations = " + ship.numberOfAccommodations);
                writer.WriteLine("Row '00' exists = " + ship.row00exists.ToString());
                foreach (outerRow orow in ship.seaSides)
                {
                    writer.WriteLine("Seasides = " + orow.bay + ", " + orow.portMost + ", " + orow.starboardMost);
                }
                writer.WriteLine("//0 - forward, 1 - aft, 2 - not defined");
                writer.WriteLine("Reefer motors facing = " + ship.rfMotor);
                writer.WriteLine("//Cargo holds");
                writer.WriteLine("Holds number = " + ship.numberOfHolds);
                for (int z = 0; z < ship.numberOfHolds; z++)
                {
                    CH hold = ship.Holds[z];
                    writer.WriteLine("Hold " + (z + 1) + " = " + hold.firstBay + "," + hold.lastBay);
                }
                writer.WriteLine("//Last bays before the accommodations");
                for (byte n = 1; n <= ship.numberOfAccommodations; n++)
                    writer.WriteLine(string.Concat("Accommodation ", n, " = ", ship.accommodationBays[n - 1]));
                writer.WriteLine("//Living quarters");
                foreach (CellPosition cell in ship.LivingQuartersList)
                    writer.WriteLine("LQ = " + cell[0] + "," + cell[1] + "," + cell[2] + "," + cell[3] + "," + cell[4]);
                writer.WriteLine("//Heated structures");
                foreach (CellPosition cell in ship.HeatedStructuresList)
                    writer.WriteLine("HS = " + cell[0] + "," + cell[1] + "," + cell[2] + "," + cell[3] + "," + cell[4]);
                writer.WriteLine("//LSA");
                foreach (CellPosition cell in ship.LSAList)
                    writer.WriteLine("LSA = " + cell[0] + "," + cell[1] + "," + cell[2] + "," + cell[3] + "," + cell[4]);
                writer.WriteLine("");
                writer.WriteLine("//DOC");
                writer.WriteLine("//0 - not allowed, 1 - packaged goods allowed");
                for (i = 1; i <= ship.numberOfHolds + 1; i++)
                {
                    addvalue = i == (ship.numberOfHolds + 1) ? "Weather deck" : "Hold " + i;
                    string entry = "DOC " + addvalue + " = ";
                    for (int x = 0; x < 23; x++)
                        entry += (x != 0 ? "," : "") + ship.doc.DOCtable[i - 1, x];
                    writer.WriteLine(entry);
                }
                writer.WriteLine("");
                AssemblyName thisAssemName = typeof(ProgramFiles).Assembly.GetName();
                Version ver = thisAssemName.Version;
                writer.WriteLine("Assembly version {0}", ver);
                writer.WriteLine("File version 1.0");
                writer.WriteLine("Created: {0:dd-MM-yyyy hh:mm}", DateTime.Now);
                writer.WriteLine("");
                writer.WriteLine("\n***ShipProfile***");

            }
        }

        /// <summary>
        /// Method used when create ship profile process aborted. It implements changes already made and the remaining fields updates with default values.
        /// </summary>
        /// <returns></returns>
        public static ShipProfile ReturnNull(ShipProfile ship)
        {
            MessageBox.Show("\nShip profile setting process is cancelled. The made changes will be considered, the remaining will be assigned with default values.\n");
            if (ship == null) return new ShipProfile();
            ship.numberOfHolds = ship.numberOfHolds == 0 ? 1 : ship.numberOfHolds;
            ship.Holds = ship.Holds ?? new CH[ship.numberOfHolds];
            int last = 1;
            for (int i = 0; i < ship.numberOfHolds; i++)
            {
                ship.Holds[i] = ship.Holds[i] ?? new CH(last, last + 99);
                last = ship.Holds[i].lastBay;
            }
            ship.numberOfAccommodations = ship.numberOfAccommodations == 0 ? (byte)1 : ship.numberOfAccommodations;
            ship.LivingQuartersList = ship.LivingQuartersList ?? new List<CellPosition> {new CellPosition(99, 199, 199, 199, 0)};
            ship.HeatedStructuresList = ship.HeatedStructuresList ?? new List<CellPosition> {new CellPosition(99, 199, 199, 199, 0)};
            ship.LSAList = ship.LSAList ?? new List<CellPosition> {new CellPosition(99, 199, 199, 199, 0)};
            ship.seaSides = ship.seaSides ?? new List<outerRow>() { new outerRow(0, 99, 99) };
            if (ship.doc == null)
            {
                ship.doc = new DOC(ship.numberOfHolds);
                for (byte i = 1; i <= ship.numberOfHolds + 1; i++)
                    ship.doc.SetDOCtable("1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1", i);
            }
            if (ship.Accommodation == null) ship.SetAccommodation(199);
            return ship;
        }

    }
}
