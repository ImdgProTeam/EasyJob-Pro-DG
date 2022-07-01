namespace EasyJob_ProDG.Model.IO.Excel
{
    public class ExcelReeferTemplate
    {
        private static byte[] _template = new byte[] {1,1,2,3,4,5,6};
        public static byte[] ReadTemplate()
        {
            return _template;
        }

        public static void ApplyTemplate(byte[] newTemplate)
        {
            try
            {
                _template = newTemplate;
            }
            catch
            {

            }  
        }
        public static void ApplyTemplate(string newTemplate)
        {
            try
            {
                var result = new byte[_template.Length];
                int i = 0;
                foreach (var item in newTemplate.Split(','))
                {
                    if (!byte.TryParse(item, out var value)) return;
                    result[i++] = value;
                }
                _template = result;
            }
            catch
            {

            }
        }

        public byte StartRow
        {
            get { return _template[0]; }
            set { _template[0] = value; }
        }

        private string _templateName;
        public string TemplateName
        {
            get { return _templateName; }
            set { _templateName = value; }
        }

        public byte[] GetTemplate => _template;
    }
}
