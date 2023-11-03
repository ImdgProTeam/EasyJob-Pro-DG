using System.Collections.Generic;


namespace EasyJob_ProDG.Model.IO.Excel
{
    /// <summary>
    /// Describes Dg template to read from and export to Excel
    /// </summary>
    public class ExcelDgTemplate : ExcelTemplate
    {
        private string[] _template;
        protected override string[] _Template
        {
            get => _template;
            set
            {
                if (value.Length != _propertiesCount + 3)
                    _template = GetDefaultTemplate();
                else
                    _template = value;
            }
        }

        /// <summary>
        /// Number of Dg cargo specific properties coded in <see cref="ExcelDgTemplate"/>.
        /// Template properties are not included.
        /// </summary>
        private const byte _propertiesCount = 20;
        public override string[] GetDefaultTemplate()
        {
            var defaultTemplate = new string[3 + _propertiesCount];

            defaultTemplate[0] = "Dg template";
            defaultTemplate[1] = "Dangerous cargo";
            defaultTemplate[2] = "7";
            defaultTemplate[defaultTemplate.Length - 1] = "v.1.2";

            for (byte i = 3; i < defaultTemplate.Length - 1; i++)
            {
                defaultTemplate[i] = (i - 2).ToString();
            }
            return defaultTemplate;
        }


        #region Excel sheet display properties

        /// <summary>
        /// Column headers to display when exporting to excel
        /// </summary>
        internal List<string> ColumnHeaders => new List<string>()
        {
            "", "", "NN", "Container number","Position", "UNNO","POL","POD","Class","Subclass","Proper shipping name",
            "PKG","FP","MP","LQ","EMS","Remarks","Net weight",
            "Technical Name", "Number and type of packages", "Final destination", "Operator", "Emergency contact"
        };

        /// <summary>
        /// Column widths to be set to excel file columns when exporting
        /// </summary>
        internal List<int> ColumnWidths => new List<int>()
        {
           0,0,6,18,10,0,0,0,0,0,45,5,5,5,5,0,30,15,45,30,0,0,18

        }; 

        #endregion


        #region properties respective order in template

        /// <summary>
        /// Returns value converted to an integer found by its column reference in <see cref="Columns"/> enum.
        /// </summary>
        /// <param name="columns">Value in <see cref="Columns"/> enumeration.</param>
        /// <returns>Value from template converted to an integer.</returns>
        internal int GetIntegerValueFromColumnsEnum(Columns columns)
        {
            return GetIntegerValueByIndex((int)columns);
        }

        internal enum Columns : int
        {
            colContNr = 3,
            colLocation = 4,
            colUnno = 5,
            colPOL = 6,
            colPOD = 7,
            colClass = 8,
            colSubclass = 9,
            colName = 10,
            colPkg = 11,
            colFP = 12,
            colMP = 13,
            colLQ = 14,
            colEms = 15,
            colRemarks = 16,
            colNetWt = 17,
            colTechName = 18,
            colPackage = 19,
            colFinalDestination = 20,
            colOperator = 21,
            colEmergencyContact = 22
        }

        #endregion

    }
}
