namespace EasyJob_ProDG.Model.IO.Excel
{
    /// <summary>
    /// Extension class to work with ExcelTemplates and their various use
    /// </summary>
    public static class ExcelTemplateSetter
    {
        /// <summary>
        /// Sets excel template value for reefers to <see cref="WithXlReefers"/> class.
        /// </summary>
        /// <param name="template"></param>
        public static void SetExcelReeferTemplate(this ExcelReeferTemplate template)
        {
            WithXlReefers.SetTemplate(template);
        }

        /// <summary>
        /// Sets excel template value for dg to <see cref="WithXlDg"/> class.
        /// </summary>
        /// <param name="template"></param>
        public static void SetExcelDgTemplate(this ExcelDgTemplate template)
        {
            WithXlDg.SetTemplate(template);
        }
    }
}
