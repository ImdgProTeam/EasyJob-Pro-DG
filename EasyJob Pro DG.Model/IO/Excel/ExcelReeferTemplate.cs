namespace EasyJob_ProDG.Model.IO.Excel
{
    /// <summary>
    /// Describes Reefer template to read from and export to Excel
    /// </summary>
    public class ExcelReeferTemplate : ExcelTemplate
    {
        private static string[] _templateStatic;
        protected override string[] _Template 
        { 
            get => _templateStatic;
            set
            {
                if (value.Length != _propertiesCount)
                    _templateStatic = GetDefaultTemplate();
                else
                    _templateStatic = value;
                ExcelTemplateSetter.SetExcelReeferTemplate(this);
            }
        }

        /// <summary>
        /// Total number properties coded in <see cref="ExcelReeferTemplate"/>
        /// </summary>
        private const byte _propertiesCount = 10;

        /// <summary>
        /// Returns default value of template
        /// </summary>
        /// <returns></returns>
        public override string[] GetDefaultTemplate()
        {
            return new string[] { "Reefer template", "Reefers", "1", "1", "2", "3", "4", "5", "6", "v.1.2" };
        }


    }
}
