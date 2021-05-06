using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using EasyJob_ProDG.Model;
using EasyJob_ProDG.Model.Cargo;
using Dg = EasyJob_ProDG.Model.Cargo.Dg;

namespace EasyJob_ProDG.UI.Classes
{
    static class Style1
    {

        //--------------------------- Messages style --------------------------------------------------------

        /// <summary>
        /// Method prints on display all found conflicts
        /// </summary>
        /// <param name="dgList"></param>
        /// <param name="textToExport"></param>
        //public static void PrintConflictMessages(List<Dg> dgList, StringBuilder textToExport)
        //{
        //    StringBuilder text = new StringBuilder();
        //    string value = $"{DateTime.Now:dd-MMM-yyyy hh:mm}";
        //    string temp;
        //    textToExport.AppendLine("\n\nPro DG completed its job at " + value);
        //    textToExport.AppendLine("\nThe list of stowage and segregation conflicts found:");

        //    foreach (Dg dg in dgList)
        //        if (dg.IsConflicted && dg.Conflict.FailedStowage)
        //        {
        //            text.AppendLine("");
        //            text.AppendFormat("\n{0} (unno {3}) in {1} - {2}", dg.Number, 
        //                dg.Location, dg.Conflict.ShowStowageConflicts(), dg.Unno);
        //            if (dg.Surrounded == null || !dg.Conflict.ShowStowageConflicts().Contains("SW1")) continue;
        //            temp = " (Unit protected only from: " + dg.Surrounded + ")";
        //            text.Append(temp);
        //        }
            
        //    if (Stowage.SWgroups.CountSW19 > 0)
        //    {
        //        temp = Stowage.SWgroups.ListSW19;
        //        text.AppendLine("").AppendLine("");

        //        //add to final message
        //        text.AppendLine("SW19 For batteries transported in accordance with special provisions 376 or 377, category C, unless transported on a short international voyage.").Append("Please check cargo documents of the following units: ");
        //        text.Append(temp);
        //    }
        //    if (Stowage.SWgroups.CountSW22 > 0)
        //    {
        //        temp = Stowage.SWgroups.ListSW22;
        //        text.AppendLine("").AppendLine("");

        //        //add to final message
        //        text.AppendLine("SW22 For WASTE AEROSOLS: category C, clear of living quarters.").Append("Please check cargo documents of the following units: ");
        //        text.Append(temp);
        //    }
            

        //    //Segregation
        //    text.AppendLine("");


        //    foreach (Dg dg in dgList)
        //        if (dg.IsConflicted && dg.Conflict.FailedSegregation)
        //        {
        //            text.AppendLine("");
        //            temp = dg.Conflict.ShowSegregationConflicts();

        //            //display in a final message
        //            text.AppendFormat("\n{0} in {1} (class {2} unno {3}) is in conflict with \n", dg.Number, 
        //                dg.Location, dg.AllDgClasses, dg.Unno);
        //            foreach(string one in temp.Split('\n'))
        //                text.AppendLine(one);
        //        }

        //    //Mechanical ventilation
        //    if (Stowage.SWgroups.CountVentHolds > 0)
        //    {
        //        temp = Stowage.SWgroups.VentHold;
        //        text.AppendLine("").AppendLine("");
        //        //add to final message
        //        text.Append("Mechanical ventilation should be started in ");
        //        text.Append(temp);
        //    }

        //    //Temperature control
        //    int count = 0;
        //    foreach (Dg dg in dgList)
        //        if (dg.IsRf)
        //        {
        //            if (count == 0)
        //            {
        //                text.AppendLine("").AppendLine("").AppendLine("The following units are transported under the temperature control (refer to IMDG Code 7.1.4.6):");
        //            }
        //            count++;
        //            text.AppendFormat("{0} (class {1}) in {2}", dg.Number, dg.AllDgClasses, dg.Location).AppendLine("");
        //        }
            


        //    text.AppendLine().AppendLine("Please, send your comments, findings and suggestions to feedback@imdg.pro");
        //    textToExport.Append(text);

        //    ProgramFiles.EnterLog(ProgramFiles.LogStreamWriter, "Conflict messages displayed");

        //}

        /// <summary>
        /// Methods displays the final information message before the application is closed.
        /// </summary>
        public  static void ShowFinalMessage()
        {
            
        }

        //------------------------ Various format methods -----------------------------------------------------

        /// <summary>
        /// Method determines the initial view of Console application
        /// </summary>
        public static void InitialFormatConsole()
        {
#if !DEBUG
            //Console.ForegroundColor = ConsoleColor.White;
            //Console.WriteLine("*** Welcome to 'Pro DG'! ***\nThis is a beta testing version of the furture great product.\nWe hope you will find it very useful in your professional life.");
            //Style.QuestionStyle("\n\nWe, the team of 'IMDG.Pro', creator of 'Pro DG', continuously put efforts to make your hard job easier.");
            //Style.AnswerStyle("\n\nYour comments, findings and suggesions are most welcome on feedback@imdg.pro");
            //Console.ForegroundColor = ConsoleColor.Yellow;
            //Style.GreenStyle("\nIMDG Code version: 2016 (Ammendments 38-16) with corrections of Dec 2017 incorporated.");
            //Style.QuestionStyle("\n\n\nPress any key to start...");
            //Console.ReadKey();
            //Console.Clear();
            //Program.EnterLog(Program.logStreamWriter, "Initial message displayed");
#endif
        }


        // ----------------- Methods to display and export result -----------------------------------------------------------------

        // ReSharper disable once UnusedMember.Local
        //private static void DisplayVoyageInfo(ReadFile edi)
        //{
        //    StringBuilder text = new StringBuilder();
        //    text.AppendLine("");
        //    text.Append("\n\nVoyage number: ").AppendLine(edi.Voyage.VoyageNumber);
        //    text.AppendFormat("Port of departure: {0}", edi.Voyage.PortOfDeparture).AppendLine("");
        //    text.Append("Port of destination: ").AppendLine(edi.Voyage.PortOfDestination);
        //    text.Append("Date: ").AppendLine(edi.Voyage.DepartureDate.ToString(CultureInfo.InvariantCulture)).AppendLine();
        //    text.AppendFormat("Total {0} containers", edi.ContainerCount + 1).AppendLine("");
        //    text.Append("RF: ").AppendLine(edi.RfContainerCount.ToString());
        //    text.AppendFormat("DG: {0}", edi.DgContainerCount).AppendLine("").AppendLine("");

        //    text.AppendFormat("Total containers loaded: {0}", edi.ContainersLoaded).AppendLine("");
        //    text.AppendFormat("RF: {0}", edi.RfContainersLoaded).AppendLine("");
        //    text.AppendFormat("DG: {0}", edi.DgContainersLoaded).AppendLine("").AppendLine("");

        //    EasyJob_ProDG.Model.Output.ThrowMessage(text.ToString());

        //    ProgramFiles.TextToExport.AppendLine(text.ToString());

        //}

        //public static void WrongDgInfoDisplay(List<Dg> dgList)
        //{
        //    foreach (var dg in dgList)
        //        if (dg.differentClass)
        //        {
        //            string[] param = {dg.Number, dg.DgClass, dg.dgClassFromList};
        //            string msg =
        //                $"\nCaution! Unit {param[0]} assigned DG class {param[1]} - different from IMDG code DG List: {param[2]}. Assign as per IMDG code?";
        //            MessageBoxResult result = MessageBox.Show(msg, "Wrong dg class!", MessageBoxButton.YesNo);

        //            if (result == MessageBoxResult.No) continue;
        //            if(!dg.DgSubclass.Contains(dg.DgClass)) dg.AllDgClassesList.Remove(dg.DgClass);
        //            dg.DgClass = dg.dgClassFromList;
        //            dg.DefineCompatibilityGroup();
        //        }

        //}

    }
}
