using EasyJob_ProDG.Data;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace EasyJob_ProDG.Model.Transport
{
    public static class ShipProfileHandler
    {
        private static bool _filecorrupt;


        /// <summary>
        /// Creates a <see cref="ShipProfile"/> and sets it to be the one used in all operations.
        /// </summary>
        /// <param name="shipProfile"></param>
        /// <param name="shipProfileFileLines"></param>
        /// <returns></returns>
        public static ShipProfile SetCreateShipProfile(List<string> shipProfileFileLines)
        {
            ShipProfile shipProfile = CreateShipProfileToUse(shipProfileFileLines);

            if (shipProfile == null)
            {
                shipProfile = ShipProfile.GetDefaultShipProfile();
            }
            else
            {
                shipProfile.SetIsNotDefault();
            }

            shipProfile.SetThisShipProfileToShip();
            return shipProfile;
        }


        /// <summary>
        /// Updates <see cref="ShipProfile"/> with values as received from the file.
        /// </summary>
        /// <param name="shipProfile">ShipProfile will be updated.</param>
        /// <param name="shipProfileFileLines">String lines as read from the ShipProfile.ini.</param>
        /// <returns>ShipProfile complete or incomplete in case of errors / file corrupt.</returns>
        internal static ShipProfile CreateShipProfileToUse(List<string> shipProfileFileLines)
        {
            ShipProfile shipProfile = ReadShipProfileFromFile(shipProfileFileLines);

            //checking profile for errors
            bool containsErrors = shipProfile.CheckContainsErrorsInShipProfile();

            if (_filecorrupt)
                LogWriter.Write($"ShipProfile file is corrupt.");
            if (containsErrors)
                LogWriter.Write("ShipProfile contains errors.");

            LogWriter.Write($"ShipProfile created.");
            return shipProfile;
        }

        /// <summary>
        /// Method creates new ShipProfile.ini file text from <see cref="ShipProfile"/> ship
        /// </summary>
        /// <param name="profileName">Name to be used in ship profile file.</param>
        /// <param name="ship"><see cref="ShipProfile"/> to be saved.</param>
        /// <returns>ShipProfile text to be saved further in a file.</returns>
        internal static string CreateShipProfileFileText(string profileName, ShipProfile ship)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("***ShipProfile***");
            sb.AppendLine("");
            sb.AppendLine("//Caution! Any change to this file can also be done in text editor. Please do NOT change any value left of '=' sign as well as the order of fields, as it will corrupt the file. The accuracy of the information will affect stowage and segregation checks.\n");
            sb.AppendLine("");
            sb.AppendLine("Profile name = " + profileName);
            sb.AppendLine("Ship name = " + ship.ShipName);
            sb.AppendLine("Call sign = " + ship.CallSign);
            sb.AppendLine("//General");
            sb.AppendLine("Passenger = " + ship.Passenger);
            sb.AppendLine("Number of accommodations = " + ship.NumberOfSuperstructures);
            sb.AppendLine("Row '00' exists = " + ship.Row00Exists);
            foreach (OuterRow orow in ship.SeaSides)
            {
                sb.AppendLine("Seasides = " + orow.Bay + ", " + orow.PortMost + ", " + orow.StarboardMost);
            }
            sb.AppendLine("//0 - not defined, 1 - aft, 2 - forward");
            sb.AppendLine("Reefer motors facing = " + ship.RfMotor);
            sb.AppendLine("//Cargo holds");
            sb.AppendLine("Holds number = " + ship.NumberOfHolds);
            for (int z = 0; z < ship.NumberOfHolds; z++)
            {
                CargoHold hold = ship.Holds[z];
                sb.AppendLine("Hold " + (z + 1) + " = " + hold.FirstBay + "," + hold.LastBay);
            }
            sb.AppendLine("//Last bays before the accommodations");
            for (byte n = 1; n <= ship.NumberOfSuperstructures; n++)
                sb.AppendLine(string.Concat("Accommodation ", n, " = ", ship.BaysInFrontOfSuperstructures[n - 1]));
            sb.AppendLine("//Living quarters");
            foreach (CellPosition cell in ship.LivingQuarters)
                sb.AppendLine("LQ = " + cell[0] + "," + cell[1] + "," + cell[2] + "," + cell[3] + "," + cell[4]);
            sb.AppendLine("//Heated structures");
            foreach (CellPosition cell in ship.HeatedStructures)
                sb.AppendLine("HS = " + cell[0] + "," + cell[1] + "," + cell[2] + "," + cell[3] + "," + cell[4]);
            sb.AppendLine("//LSA");
            foreach (CellPosition cell in ship.LSA)
                sb.AppendLine("LSA = " + cell[0] + "," + cell[1] + "," + cell[2] + "," + cell[3] + "," + cell[4]);
            sb.AppendLine("");
            sb.AppendLine("//DOC");
            sb.AppendLine("//0 - not allowed, 1 - packaged goods allowed");
            int i;
            for (i = 0; i <= ship.NumberOfHolds; i++)
            {
                var addvalue = i == 0 ? "Weather deck" : "Hold " + i;
                string entry = "DOC " + addvalue + " = ";
                for (int x = 0; x < 23; x++)
                    entry += (x != 0 ? "," : "") + ship.Doc.DOCtable[i, x];
                sb.AppendLine(entry);
            }
            sb.AppendLine("");
            AssemblyName thisAssemName = typeof(ProgramFiles).Assembly.GetName();
            Version ver = thisAssemName.Version;
            sb.AppendLine($"Assembly version {ver}");
            sb.AppendLine("File version 1.0");
            sb.AppendLine($"Created: {DateTime.Now:dd-MM-yyyy hh:mm}");
            sb.AppendLine("");
            sb.AppendLine("\n***ShipProfile***");

            return sb.ToString();
        }


        /// <summary>
        /// Reads ShipProfile from file lines.
        /// </summary>
        /// <param name="fileLines"><see cref="List{string}"/> containing the lines as read from ShipProfile.ini</param>
        /// <returns><see cref="ShipProfile"/> updated from the lines or an uncomplete one in case of errors.</returns>
        private static ShipProfile ReadShipProfileFromFile(List<string> fileLines)
        {
            ShipProfile shipProfile = ShipProfile.GetDefaultShipProfile();
            Exception ex = new ArgumentException();
            _filecorrupt = false;

            ClearDefaultValues(shipProfile);

            var linecount = 0;
            byte bayOfAccommodation1 = 0;

            foreach (string line in fileLines)
            {
                try
                {
                    if (linecount == 0 && line != "***ShipProfile***")
                    {
                        LogWriter.Write("Ship profile file is wrong or modified.");
                        _filecorrupt = true;
                        return null;
                    }
                    if (line != null && !line.StartsWith("//") && line.Length != 0 && line.Contains("="))
                    {
                        var lineNormalized = line.Replace("=  ", "=").Replace("= ", "=");
                        string lineValue = lineNormalized.Substring(line.IndexOf('=') + 1);
                        string lineDescription = lineNormalized.Substring(0, line.IndexOf('=')).Replace(" ", "")
                            .Replace("\'", "").ToLower();
                        switch (lineDescription)
                        {
                            case "profilename": break;
                            case "shipname":
                                shipProfile.ShipName = lineValue;
                                break;
                            case "callsign":
                                shipProfile.CallSign = lineValue;
                                break;
                            case "numberofaccommodations":
                                ex.Source = "accommodation";
                                shipProfile.NumberOfSuperstructures = byte.Parse(lineValue);
                                break;
                            case "row00exists":
                                ex.Source = "row00exists";
                                shipProfile.Row00Exists = bool.Parse(lineValue);
                                break;
                            case "passenger":
                                ex.Source = "passenger";
                                shipProfile.Passenger = bool.Parse(lineValue);
                                break;
                            case "seasides":
                                ex.Source = "seasides";
                                OuterRow instance = new OuterRow();
                                byte i = 0;
                                foreach (string figure in lineValue.Split(','))
                                {
                                    instance[i] = int.Parse(figure);
                                    i++;
                                }

                                if (instance.Bay == 0)
                                    shipProfile.SeaSides.RemoveAt(0);
                                shipProfile.SeaSides.Add(instance);
                                break;
                            case "reefermotorsfacing":
                                ex.Source = "reefermotor";
                                shipProfile.RfMotor = byte.Parse(lineValue);
                                break;
                            case "accommodation":
                                ex.Source = "accommodation";
                                shipProfile.SetSuperstructuresBaysProperties(byte.Parse(lineValue));
                                break;
                            case "accommodation1":
                                ex.Source = "accommodation";
                                bayOfAccommodation1 = byte.Parse(lineValue);
                                if (shipProfile.NumberOfSuperstructures == 1)
                                    shipProfile.SetSuperstructuresBaysProperties(bayOfAccommodation1);
                                break;
                            case "accommodation2":
                                ex.Source = "accommodation";
                                if (shipProfile.NumberOfSuperstructures == 2)
                                    shipProfile.SetSuperstructuresBaysProperties(bayOfAccommodation1, byte.Parse(lineValue));
                                break;
                            case "lq":
                                ex.Source = "lq";
                                string[] lq = lineValue.Split(',');
                                shipProfile.LivingQuarters.Add(new CellPosition(byte.Parse(lq[0]), byte.Parse(lq[1]),
                                    byte.Parse(lq[2]), byte.Parse(lq[3]), byte.Parse(lq[4])));
                                break;
                            case "hs":
                                ex.Source = "hs";
                                string[] hs = lineValue.Split(',');
                                shipProfile.HeatedStructures.Add(new CellPosition(byte.Parse(hs[0]), byte.Parse(hs[1]),
                                    byte.Parse(hs[2]), byte.Parse(hs[3]), byte.Parse(hs[4])));
                                break;
                            case "lsa":
                                ex.Source = "lsa";
                                string[] lsa = lineValue.Split(',');
                                shipProfile.LSA.Add(new CellPosition(byte.Parse(lsa[0]), byte.Parse(lsa[1]),
                                    byte.Parse(lsa[2]), byte.Parse(lsa[3]), byte.Parse(lsa[4])));
                                break;
                        }

                        if (lineDescription.StartsWith("hold"))
                        {
                            ex.Source = "holds";
                            if (lineDescription == "holdsnumber")
                            {
                                shipProfile.NumberOfHolds = byte.Parse(lineValue);
                                shipProfile.Holds = new List<CargoHold>();
                                shipProfile.Doc = new DOC(shipProfile.NumberOfHolds);
                            }
                            else
                            {
                                var FandLbays = lineValue.Split(',');
                                shipProfile.Holds.Add(new CargoHold(byte.Parse(FandLbays[0]), byte.Parse(FandLbays[1])));
                            }
                        }

                        if (lineDescription.StartsWith("doc"))
                        {
                            ex.Source = "doc";
                            if (lineDescription.Substring(3).StartsWith("weather"))
                                shipProfile.Doc.SetDOCTableRow(lineValue, 0);
                            else
                                shipProfile.Doc.SetDOCTableRow(lineValue, byte.Parse(lineDescription.Substring(7)));
                        }
                    }
                }
                catch
                {
                    shipProfile.ErrorList = ex.Source;
                    LogWriter.Write($"An error occurred while opening the ship profile:\n\tsource:{ex.Source}\n\terror: {ex.Message}");
                    _filecorrupt = true;
                }

                linecount++;
            }

            shipProfile.SetIsNotDefault();
            return shipProfile;
        }

        /// <summary>
        /// Clears default values to avoid copying unnecessary default values on each time ShipProfile creation.
        /// </summary>
        /// <param name="shipProfile"></param>
        private static void ClearDefaultValues(ShipProfile shipProfile)
        {
            shipProfile.HeatedStructures.Clear();
            shipProfile.LivingQuarters.Clear();
            shipProfile.LSA.Clear();
        }

        /// <summary>
        /// Method checks if any errors contained in loaded ship profile which may lead to fatal error further in program.
        /// </summary>
        /// <param name="shipProfile"></param>
        /// <returns>True if errors found</returns>
        private static bool CheckContainsErrorsInShipProfile(this ShipProfile shipProfile)
        {
            bool result = false;
            bool interResult = false;

            //accommodation
            if (shipProfile.NumberOfSuperstructures == 0 || shipProfile.BaysSurroundingSuperstructure == null || shipProfile.BaysSurroundingSuperstructure.Count == 0)
            {
                shipProfile.ErrorList = "accommodation";
                result = true;
            }
            //holds
            if (shipProfile.NumberOfHolds != shipProfile.Holds.Count || shipProfile.NumberOfHolds == 0)
            {
                shipProfile.ErrorList = "holds";
                result = true;
            }
            foreach (var hold in shipProfile.Holds)
            {
                if (hold == null)
                {
                    shipProfile.ErrorList = "holds";
                    result = true;
                    break;
                }
            }
            //seasides
            if (shipProfile.SeaSides.Count == 0 || shipProfile.SeaSides == null)
            {
                shipProfile.ErrorList = "seasides";
                result = true;
            }
            //checking that at least one record covers all bays.
            foreach (OuterRow orow in shipProfile.SeaSides)
            {
                if (orow.Bay == 0)
                {
                    interResult = true;
                    break;
                }
            }
            if (!interResult)
            {
                shipProfile.ErrorList = "seasides";
                result = true;
            }
            //DOC
            if (shipProfile.Doc.NumberOfRows != shipProfile.NumberOfHolds + 1 && !shipProfile.ErrorList.Contains("holds"))
            {
                shipProfile.ErrorList = "doc";
                result = true;
            }

            return result;
        }
    }

}

