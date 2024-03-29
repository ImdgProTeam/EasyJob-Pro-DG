﻿using EasyJob_ProDG.Data;
using System;
using System.IO;
using System.Reflection;

namespace EasyJob_ProDG.Model.Transport
{
    partial class ShipProfile
    {
        // ------------------------------ Methods to work with ship profile ----------------------------------------------

        // ---------------------- non-static -----------------------------------------------------------------------------

        /// <summary>
        /// Method checks if any errors contained in loaded ship profile which may lead to fatal error further in program.
        /// Returns true if no errors found.
        /// </summary>
        private bool CheckErrorsInShipProfile()
        {
            bool result = true;
            bool interResult = false;

            //accommodation
            if (NumberOfAccommodations == 0 || Accommodation == null || Accommodation.Count == 0)
            {
                ErrorList = "accommodation";
                result = false;
            }
            //holds
            if (NumberOfHolds != Holds.Count || NumberOfHolds == 0)
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
            if (SeaSides.Count == 0 || SeaSides == null)
            {
                ErrorList = "seasides";
                result = false;
            }
            //checking that at least one record covers all bays.
            foreach (OuterRow orow in SeaSides)
            {
                if (orow.Bay == 0)
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
            if (Doc.NumberOfRows != NumberOfHolds + 1 && !_errorList.Contains("holds"))
            {
                ErrorList = "doc";
                result = false;
            }

            if (result) _errorList.Clear();
            return result;
        }


        // ---------------------- static ----------------------------------------------------------------------------------

        public static ShipProfile ReadShipProfile(string shipFile, bool openDefault)
        {
            //MessageBoxResult result;
            string fileName = OpenShipProfile(shipFile, openDefault);
            if (!File.Exists(fileName))
            {
                ////Case file not found
                //result = MessageBox.Show("Ship configuration file not found.\nDo you wish to create it? Y/N?\n(It may take some time and will require from you detailed information regarding your ship)", "Ship profile", MessageBoxButton.YesNo);
                ////Creation of a new file and commence working with it
                //if (result == MessageBoxResult.Yes)
                //    return CreateShipProfile(multiprofile);
                ////Loading of default shipprofile
                //MessageBox.Show("A default ship profile configuration will be loaded. That will affect the accuracy of stowage and segregation check.");
                Output.ThrowMessage("Ship configuration file not found.\nA default ship profile configuration will be loaded. That will affect the accuracy of stowage and segregation check.");
                LogWriter.Write($"File {fileName} does not exist.");
                LogWriter.Write($"Default Ship profile will be loaded");
                return new ShipProfile()
                {
                    isShipProfileNotFound = true,
                    isDefault = true
                };
            }

            //Case if file found and read
            ShipProfile ship = new ShipProfile(fileName);
            if (!ship._filecorrupt)
            {
                LogWriter.Write($"Ship profile is succesfully read from file {fileName}.");
                if (ship.containsErrors)
                {
                    LogWriter.Write("ShipProfile contains errors.");
                }
                return ship;
            }

            LogWriter.Write($"Ship profile read from file {fileName} is corrupt.");
            LogWriter.Write("Default Ship profile will be loaded");
            return new ShipProfile()
            {
                isDefault = true
            };
        }

        /// <summary>
        /// Method creates new profileName.ini file from ShipProfile ship
        /// </summary>
        /// <param name="profileName"></param>
        /// <param name="ship"></param>
        /// <returns></returns>
        internal static void WriteShipProfile(string profileName, ShipProfile ship)
        {
            string fname = ProgramDefaultSettingValues.ProgramDirectory + profileName + ProgramDefaultSettingValues.ShipProfileExtension;

            using (StreamWriter writer = new StreamWriter(File.Create(fname)))
            {
                writer.WriteLine("***ShipProfile***");
                writer.WriteLine("");
                writer.WriteLine("//Caution! Any change to this file can also be done in text editor. Please do NOT change any value left of '=' sign as well as the order of fields, as it will corrupt the file. The accuracy of the information will affect stowage and segregation checks.\n");
                writer.WriteLine("");
                writer.WriteLine("Profile name = " + profileName);
                writer.WriteLine("Ship name = " + ship.ShipName);
                writer.WriteLine("Call sign = " + ship.CallSign);
                writer.WriteLine("//General");
                writer.WriteLine("Passenger = " + ship.Passenger);
                writer.WriteLine("Number of accommodations = " + ship.NumberOfAccommodations);
                writer.WriteLine("Row '00' exists = " + ship.Row00Exists);
                foreach (OuterRow orow in ship.SeaSides)
                {
                    writer.WriteLine("Seasides = " + orow.Bay + ", " + orow.PortMost + ", " + orow.StarboardMost);
                }
                writer.WriteLine("//0 - not defined, 1 - aft, 2 - forward");
                writer.WriteLine("Reefer motors facing = " + ship.RfMotor);
                writer.WriteLine("//Cargo holds");
                writer.WriteLine("Holds number = " + ship.NumberOfHolds);
                for (int z = 0; z < ship.NumberOfHolds; z++)
                {
                    CargoHold hold = ship.Holds[z];
                    writer.WriteLine("Hold " + (z + 1) + " = " + hold.FirstBay + "," + hold.LastBay);
                }
                writer.WriteLine("//Last bays before the accommodations");
                for (byte n = 1; n <= ship.NumberOfAccommodations; n++)
                    writer.WriteLine(string.Concat("Accommodation ", n, " = ", ship.AccommodationBays[n - 1]));
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
                int i;
                for (i = 0; i <= ship.NumberOfHolds; i++)
                {
                    var addvalue = i == 0 ? "Weather deck" : "Hold " + i;
                    string entry = "DOC " + addvalue + " = ";
                    for (int x = 0; x < 23; x++)
                        entry += (x != 0 ? "," : "") + ship.Doc.DOCtable[i, x];
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
            LogWriter.Write($"Ship profile successfully written to {fname}");
        }

    }
}
