using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Xml.Linq;
using System.Xml;
using System.Text;
using System.IO;
using System.Windows;



namespace EasyJob_Pro_DG
{
    public class ProgramFiles
    {
        public static StreamWriter logStreamWriter;
        public static StringBuilder textToExport;


        /// <summary>
        /// The program will open and read required .edi file and check 
        /// stowage and segregation of all DG containers in file according
        /// to IMDG code 38-16 ammendments.
        /// </summary>
        /// <param name="args"></param>
//        public static void Main(string[] args)
//        {
//            textToExport = new StringBuilder();
//            using (logStreamWriter = new StreamWriter(File.Create(ShipProfile.dir + "log.txt")))
//            {

//                try
//                {
//                    #region Program run
//                    //creating log
//                    CreateLog(logStreamWriter, args.Length>0?args[0]:null);
//                    EnterLog(logStreamWriter, "Commenced execution");

//                    List<Dg> dgList;
//                    List<Container> containers;
//                    List<Container> Reefers;
//                    ShipProfile ownship = null;
//                    XDocument dgDataBase = null;
//                    EnterLog(logStreamWriter, "Fields initialized");


//                    ///Program run:

//                    Output.ProgramInitiatedView();
//#if !DEBUG
//                    Licence.LicenceCheck();
//#endif

//                    ///Connect program files
//                    ConnectProgramFiles(ref ownship, ref dgDataBase);

//                    //Open .edi file
//                    string workingFile = OpenFile.Open(args);
//                    EnterLog(logStreamWriter, "Working file " + workingFile);

//                    Style.GreenStyle();
//                    dgList = CreateDGList(workingFile, OpenFile.fileType, ownship, out containers, out Reefers);
//                    UpdateDGInfo(dgList, dgDataBase);
//                    CheckDGList(dgList, OpenFile.fileType);

//                    Stowage.CheckStowage(dgList, ownship, containers);
//                    CheckSegregation(dgList, ownship, Reefers);

//                    Style.PrintConflictMessages(dgList, textToExport);

//                    ExportDGListToXL(dgList);

//                    Style.ShowFinalMessage();
//                    EnterLog(logStreamWriter, "Job completed");
//#endregion
//            }
//                catch (Exception ex)
//            {
//                LogErrorMesage(ex);
//            }
//        } //end of stream
 


        // ----------------------- STATIC METHODS ---------------------------------------------------------------------------------

        // ------------------ Methods supporting the run of the program -----------------------------------------------------------

        public static void Connect(ref ShipProfile ownship, ref XDocument dgDataBase)
        {
            ownship = ShipProfile.ReadShipProfile(ShipProfile.defaultShipProfile, ShipProfile.alwaysOpenDefaultProfile);
            EnterLog(logStreamWriter, "Ship profile loaded");

            dgDataBase = GetXmlDoc("dglist.xml");
            EnterLog(logStreamWriter, "Database connected");
        }

        /// <summary>
        /// Method loads the given .xml file located in application folder directory.
        /// </summary>
        /// <param name="docName"></param>
        /// <returns></returns>
        public static XDocument GetXmlDoc(string docName)
        {
            try
            {
                XDocument xmlDoc = XDocument.Load(Path.Combine(ShipProfile.dir.ToString()) + docName);
                return xmlDoc;
            }
            catch (FileNotFoundException ex)
            {
                Output.Display(ex.Message);
                return null;
            }
        }

        public static void ExitApp()
        {
            Style.ShowFinalMessage();
            EnterLog(logStreamWriter, "Interrupted by the user");
            logStreamWriter.Close();
            Environment.Exit(0);
        }

        //------------------- Supportive methods to work with Dg list --------------------------------------------------------------

        /// <summary>
        /// Method searches for duplicate records and safely removes them. Wrong DG classes will be checked and option will be proposed to ammend it in accordance with IMDG code.
        /// </summary>
        /// <param name="dgList"></param>
        /// <param name="filetype"></param>
        public static void CheckDGList(List<Dg> dgList, byte filetype)
        {
            if (filetype != (byte) OpenFile.FileTypes.EDI) return;
            RemoveDuplicateRecords(dgList);
            //WrongDGInfoDisplay(dgList);
            EnterLog(logStreamWriter, "Dg list checked");
        }

        /// <summary>
        /// Method checks the segregation between all the units in dg list and adds a conflict to the units if any.
        /// </summary>
        /// <param name="dgList"></param>
        /// <param name="ownship"></param>
        /// <param name="reefers"></param>
        public static void CheckSegregation(DgList dgList, ShipProfile ownship, ObservableCollection<Container> reefers)
        {
            //Segregation segr = new Segregation(ownship);
            foreach (Dg dg in dgList)
            {
              Segregation.Segregate(dg, dgList, reefers, ownship);  
            }
            Segregation.postSegregation(dgList, ownship, reefers);
            EnterLog(logStreamWriter, "Segregation checked");
        }

        /// <summary>
        /// Method applies the information from all available sources to units in dg list
        /// </summary>
        /// <param name="dgList"></param>
        /// <param name="dgDataBase"></param>
        public static void UpdateDGInfo(List<Dg> dgList, XDocument dgDataBase)
        {
            foreach (Dg unit in dgList)
            {
                unit.AssignFromDGList(xmlDoc: dgDataBase);
                unit.AssignRowFromDOC();
            }
            EnterLog(logStreamWriter, "Dg info updated");
        }

        public static void UpdateDGInfo(Dg dg, XDocument dgDataBase, bool unitIsNew = false)
        {
            if(unitIsNew) dg.Clear(dg.unno);
            dg.AssignFromDGList(xmlDoc: dgDataBase, unitIsNew: true);
            dg.AssignRowFromDOC();
            if (unitIsNew)
            {
                dg.AssignSegregationGroup();
            }
        }


        /// <summary>
        /// Removes duplicate records from dg list (based on their class in comparison 
        /// to the class given to respective unno in IMDG code).
        /// </summary>
        /// <param name="dgList"></param>
        public static void RemoveDuplicateRecords(List<Dg> dgList)
        {
            List<Dg> duplicates = new List<Dg>();
            foreach (Dg a in dgList)
            {
                foreach (Dg b in dgList)
                {
                    if (a.cntrNr == b.cntrNr && a.unno == b.unno && a.dgclass!=b.dgclass && a.dgpg==b.dgpg)
                    {
                        if (a.dgclass == a.dgClassFromList) duplicates.Add(b);
                        else if (b.dgclass == b.dgClassFromList) duplicates.Add(a);
                    }
                }
            }

            foreach (var unit in duplicates)
            {
                dgList.Remove(unit);
            }

        }


        // ----------------- Methods to display and export result -----------------------------------------------------------------

        private static void DisplayVoyageInfo(ReadFile edi)
        {
            StringBuilder text = new StringBuilder();
            text.AppendLine("");
            text.Append("\n\nVoyage number: ").AppendLine(edi.vessel.voyageNr);
            text.AppendFormat("Port of departure: {0}", edi.vessel.portOfDeparture).AppendLine("");
            text.Append("Port of destination: ").AppendLine(edi.vessel.portOfDestination);
            text.Append("Date: ").AppendLine(edi.vessel.departureDate.ToString(CultureInfo.InvariantCulture)).AppendLine();
            text.AppendFormat("Total {0} containers", edi.containerCount + 1).AppendLine("");
            text.Append("RF: ").AppendLine(edi.rfContainerCount.ToString());
            text.AppendFormat("DG: {0}", edi.dgContainerCount).AppendLine("").AppendLine("");

            text.AppendFormat("Total containers loaded: {0}", edi.containersLoaded).AppendLine("");
            text.AppendFormat("RF: {0}", edi.rfContainersLoaded).AppendLine("");
            text.AppendFormat("DG: {0}", edi.dgContainersLoaded).AppendLine("").AppendLine("");

            Output.Display(text.ToString());

            textToExport.AppendLine(text.ToString());

        }

        private static void ExportDGListToXL(List<Dg> dgList)
        {
            //Export dg list to excel
            WithXl.Export(dglist: dgList);
            EnterLog(logStreamWriter, "Excel export successful");
        }

        public static void WrongDGInfoDisplay(List<Dg> dgList)
        {
            foreach (var dg in dgList)
            if (dg.differentClass)
            {
                string[] param = {dg.cntrNr, dg.dgclass, dg.dgClassFromList};
                string msg =
                    $"\nCaution! Unit {param[0]} assigned DG class {param[1]} - different from IMDG code DG List: {param[2]}. Assign as per IMDG code?";
                MessageBoxResult result = MessageBox.Show(msg, "Wrong dg class!", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.No) continue;
                if(!dg.dgsubclass.Contains(dg.dgclass)) dg.allDgClasses.Remove(dg.dgclass);
                dg.Dgclass = dg.dgClassFromList;
                dg.DefineCompatibilityGroup();
            }

        }


        // ---------------- Static methods for creation and maintaining of the error log -------------------------------------------

        public static void EnterLog(StreamWriter stream, string message)
        {
            stream?.WriteLine("{0:O}\t{1}", DateTime.Now, message);
        }

        public static void CreateLog(StreamWriter stream, string arg=null)
        {
            stream?.WriteLine("Program activated @ {0:O}", DateTime.Now);
            stream?.WriteLine("activation method: {0}", arg ?? "executable");
            stream?.WriteLine("Licence expire on: {0:R}", Licence.EndLicence);
            stream?.WriteLine();
        }

        private static void LogErrorMesage(Exception ex)
        {
            EnterLog(logStreamWriter, "Exception caught");
            logStreamWriter.WriteLine("exception data {0}", ex);
            logStreamWriter.WriteLine("exception message {0}", ex.Message);
            logStreamWriter.WriteLine("exception data {0}", ex.Data);
            logStreamWriter.WriteLine("exception source {0}", ex.Source);
            logStreamWriter.WriteLine("exception target {0}", ex.TargetSite);
            logStreamWriter.WriteLine("exception inner {0}", ex.InnerException);
            string msg = "An error occurred when running Pro DG!\nPlease contact feedback@imdg.pro for further assistance\nExtremely sorry for inconvenience...";
            MessageBox.Show(msg);
        }
    }
}


