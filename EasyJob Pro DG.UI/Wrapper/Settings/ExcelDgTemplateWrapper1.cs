using EasyJob_ProDG.Model.IO.Excel;

namespace EasyJob_ProDG.UI.Wrapper
{
    public class ExcelDgTemplateWrapper : ExcelTemplateWrapper<ExcelDgTemplate>
    {

        protected override string[] ColumnTitles => new string[]
        {
            "TemplateName",
            "WorkingSheet",
            "StartRow",
            "Container Number",
            "Location",
            "UN no",
            "POL",
            "POD",
            "Dg class",
            "Sub class",
            "Proper shipping name",
            "Packing group",
            "Flash point",
            "Marine pollutant",
            "Limited quantity",
            "EmS",
            "Remarks",
            "Net weight",
            "Technical Name",
            "Number and type of package",
            "Final destination",
            "Operator",
            "Emergency contacts"
        };

        public ExcelDgTemplateWrapper(ExcelDgTemplate model) : base(model)
        {
            GenerateColumnProperties();
            GetMainTemplateProperties();
        }
    }
}
