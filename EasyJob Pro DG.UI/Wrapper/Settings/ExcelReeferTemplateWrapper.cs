using EasyJob_ProDG.Model.IO.Excel;

namespace EasyJob_ProDG.UI.Wrapper
{
    public class ExcelReeferTemplateWrapper : ExcelTemplateWrapper<ExcelReeferTemplate>
    {

        protected override string[] ColumnTitles => new string[]
        {
            "TemplateName",
            "WorkingSheet",
            "StartRow",
            "Container number",
            "Commodity",
            "Set temp",
            "Vent settings",
            "Special",
            "Remarks"
        };

        public ExcelReeferTemplateWrapper(ExcelReeferTemplate model) : base(model)
        {
            GenerateColumnProperties();
            GetMainTemplateProperties();
        }
    }
}
