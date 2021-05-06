using System;
using System.IO;
using EasyJob_ProDG.Data;

namespace EasyJob_ProDG.Model.IO.Excel
{
    /// <summary>
    /// Class describes, saves and reads ExcelTemplate to be used to import/export from/to excel.
    /// </summary>
    public class ExcelTemplate
    {
        static bool _templateRead = false;

        #region static fields - default values
        static string _templateName = ProgramDefaultSettingValues.ProgramDirectory + "template.xlt";
        static string _workingSheet = "Dangerous cargo";
        static int _startRow = 2;
        static int _colContNr = 2;
        static int _colLocation = 3;
        static int _colUnno = 4;
        static int _colPOL = 5;
        static int _colPOD = 6;
        static int _colClass = 7;
        static int _colSubclass = 8;
        static int _colName = 9;
        static int _colPkg = 10;
        static int _colFP = 11;
        static int _colMP = 12;
        static int _colLQ = 13;
        static int _colEms = 14;
        static int _colRemarks = 15;
        static int _colNetWt = 16;
        static int _colTechName = 17;
        static int _colPackage = 18;
        static int _colFinalDestination = 19;
        static int _colOperator = 20;
        static int _colEmergencyContact = 21;
        #endregion

        //public properties
        #region public properties
        public string TemplateName { get { return _templateName.Remove(0,_templateName.LastIndexOf('\\') + 1).Replace(".xlt", ""); }
            set { _templateName = ProgramDefaultSettingValues.ProgramDirectory + value + ".xlt"; }
        }
        public string WorkingSheet
        {
            get { return _workingSheet; }
            set { _workingSheet = value; }
        }
        public int StartRow { get { return _startRow; } set { _startRow = value; } }
        public int ColumnContainerNumber { get { return _colContNr; } set { _colContNr = value; } }
        public int ColumnLocation { get { return _colLocation; } set { _colLocation = value; } }
        public int ColumnUnno { get { return _colUnno; } set { _colUnno = value; } }
        public int ColumnPOL { get { return _colPOL; } set { _colPOL = value; } }
        public int ColumnPOD { get { return _colPOD; } set { _colPOD = value; } }
        public int ColumnClass { get { return _colClass; } set { _colClass = value; } }
        public int ColumnSubclass { get { return _colSubclass; } set { _colSubclass = value; } }
        public int ColumnName { get { return _colName; } set { _colName = value; } }
        public int ColumnPkg { get { return _colPkg; } set { _colPkg = value; } }
        public int ColumnFP { get { return _colFP; } set { _colFP = value; } }
        public int ColumnMP { get { return _colMP; } set { _colMP = value; } }
        public int ColumnLQ { get { return _colLQ; } set { _colLQ = value; } }
        public int ColumnEms { get { return _colEms; } set { _colEms = value; } }
        public int ColumnRemark { get { return _colRemarks; } set { _colRemarks = value; } } 
        public int ColumnNetWeight { get { return _colNetWt; } 
            set { _colNetWt = value; } }
        public int ColumnTechName { get { return _colTechName; } set { _colTechName = value; } }
        public int ColumnPackage { get { return _colPackage; } set { _colPackage = value; } }
        public int ColumnFinalDestination { get { return _colFinalDestination; } set { _colFinalDestination = value; } }
        public int ColumnOperator { get { return _colOperator; } set { _colOperator = value; } }
        public int ColumnEmergencyContact { get { return _colEmergencyContact; } set { _colEmergencyContact = value; } }
        #endregion

        public ExcelTemplate()
        {
            _templateRead = false;
        }

        /// <summary>
        /// Reads template file to generate column settings.
        /// <returns>True if template has been read successfully.</returns>
        /// </summary>
        public bool ReadTemplate()
        {
            if (_templateRead) return true;

            //reading xl template
            if (!File.Exists(_templateName))
            {
                //TODO: TO BE IMPLEMENTED OUTPUT MESSAGE
                Output.ThrowMessage("Template file not found. Do you wish to create it? Y/N");
                return false;
                //throw new NotImplementedException();
            }
            else
            {
                using (var reader = new StreamReader(_templateName))
                {
                    while (!reader.EndOfStream)
                    {
                        try
                        {
                            string line = reader.ReadLine();
                            if (string.IsNullOrEmpty(line) || !line.Contains("=")) continue;
                            string descriptor = line.Remove(line.IndexOf("=", StringComparison.Ordinal)).Trim();
                            string value = line.Substring(line.IndexOf("=", StringComparison.Ordinal) + 1).Trim();

                            switch (descriptor)
                            {
                                case "worksheet":
                                    value = value.Replace("\"", "");
                                    _workingSheet = value;
                                    break;
                                case "startRow":
                                    _startRow = int.Parse(value);
                                    if (_startRow < 2) _startRow = 2;
                                    break;
                                case "container number":
                                    _colContNr = int.Parse(value);
                                    break;
                                case "container location":
                                    _colLocation = int.Parse(value);
                                    break;
                                case "unno":
                                    _colUnno = int.Parse(value);
                                    break;
                                case "pol":
                                    _colPOL = int.Parse(value);
                                    break;
                                case "pod":
                                    _colPOD = int.Parse(value);
                                    break;
                                case "class":
                                    _colClass = int.Parse(value);
                                    break;
                                case "subclass":
                                    _colSubclass = int.Parse(value);
                                    break;
                                case "psn":
                                    _colName = int.Parse(value);
                                    break;
                                case "pkg":
                                    _colPkg = int.Parse(value);
                                    break;
                                case "fp":
                                    _colFP = int.Parse(value);
                                    break;
                                case "mp":
                                    _colMP = int.Parse(value);
                                    break;
                                case "lq":
                                    _colLQ = int.Parse(value);
                                    break;
                                case "ems":
                                    _colEms = int.Parse(value);
                                    break;
                                case "remarks":
                                    _colRemarks = int.Parse(value);
                                    break;
                                case "net weight":
                                    _colNetWt = int.Parse(value);
                                    break;
                                case "technical name":
                                    _colTechName = int.Parse(value);
                                    break;
                                case "package":
                                    _colPackage = int.Parse(value);
                                    break;
                                case "final destination":
                                    _colFinalDestination = int.Parse(value);
                                    break;
                                case "operator":
                                    _colOperator = int.Parse(value);
                                    break;
                                case "emergency contact":
                                    _colEmergencyContact = int.Parse(value);
                                    break;
                            }
                        }
                        catch
                        {
                            Output.ThrowMessage("Template file is damaged or modified. Please create a new template");
                            _templateRead = false;
                            return false;
                        }
                    }

                    _templateRead = true;
                    return true;
                }
            }
        }

        /// <summary>
        /// Writes template to xl template file.
        /// </summary>
        /// <param name="template">Template to be saved.</param>
        public static void WriteTemplate(ExcelTemplate template)
        {
            _templateRead = false;

            //MessageBoxResult result = MessageBox.Show("Do you wish to save changes and update your template for further use? Y/N", String.Empty, MessageBoxButton.YesNo);
            //if (result == MessageBoxResult.Yes)
            //    Output.ThrowMessage("Your changes will be saved and template updated");
            using (StreamWriter writer = new StreamWriter(File.Create(template.TemplateName)))
            {
                writer.WriteLine("***Template for excel import and export ***");
                writer.WriteLine("");
                writer.WriteLine("worksheet = " + template.WorkingSheet);
                writer.WriteLine("startRow = " + template.StartRow);
                writer.WriteLine("container number = " + template.ColumnContainerNumber);
                writer.WriteLine("container location = " + template.ColumnLocation);
                writer.WriteLine("unno = " + template.ColumnUnno);
                writer.WriteLine("pol = " + template.ColumnPOL);
                writer.WriteLine("pod = " + template.ColumnPOD);
                writer.WriteLine("class = " + template.ColumnClass);
                writer.WriteLine("subclass = " + template.ColumnSubclass);
                writer.WriteLine("psn = " + template.ColumnName);
                writer.WriteLine("pkg = " + template.ColumnPkg);
                writer.WriteLine("fp = " + template.ColumnFP);
                writer.WriteLine("mp = " + template.ColumnMP);
                writer.WriteLine("lq = " + template.ColumnLQ);
                writer.WriteLine("ems = " + template.ColumnEms);
                writer.WriteLine("remarks = " + template.ColumnRemark);
                writer.WriteLine("net weight = " + template.ColumnNetWeight);
                writer.WriteLine("technical name = " + template.ColumnTechName);
                writer.WriteLine("package = " + template.ColumnPackage);
                writer.WriteLine("final destination = " + template.ColumnFinalDestination);
                writer.WriteLine("operator = " + template.ColumnOperator);
                writer.WriteLine("emergency contact = " + template.ColumnEmergencyContact);
                writer.WriteLine("");
                writer.WriteLine("Template version: 1.1");
            }
        }

        /// <summary>
        /// Write present template to file.
        /// </summary>
        private void WriteTemplate()
        {
            WriteTemplate(this);
        }
    }
}
