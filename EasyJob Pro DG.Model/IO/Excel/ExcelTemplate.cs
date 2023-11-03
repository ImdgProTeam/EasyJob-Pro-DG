using EasyJob_ProDG.Data;
using System.Linq;

namespace EasyJob_ProDG.Model.IO.Excel
{
    /// <summary>
    /// Contains common properties and methods to be implemented in all excel templates
    /// </summary>
    public abstract class ExcelTemplate
    {
        protected abstract string[] _Template { get; set; }

        /// <summary>
        /// Returns default template
        /// </summary>
        /// <returns></returns>
        public abstract string[] GetDefaultTemplate();

        public string[] GetTemplate() => _Template ?? GetDefaultTemplate();
        public string GetTemplateString()
        {
            return string.Join(",", GetTemplate());
        }


        #region Template public Properties

        /// <summary>
        /// Represents template Name in settings.settings
        /// </summary>
        public string TemplateSettingsName;

        /// <summary>
        /// Display template name
        /// </summary>
        public string TemplateName
        {
            get { return _Template[0]; }
            set { _Template[0] = value; }
        }

        /// <summary>
        /// Excel working sheet to be used when reading/writing to excel workbook.
        /// </summary>
        public string WorkingSheet
        {
            get { return _Template[1]; }
            set { _Template[1] = value; }
        }

        /// <summary>
        /// A row number from which cargo info reading will start
        /// </summary>
        public byte StartRow
        {
            get { return byte.Parse(_Template[2]); }
            set { _Template[2] = value.ToString(); }
        }

        /// <summary>
        /// Version of the template (the last value in the template)
        /// </summary>
        public string TemplateVersion
        {
            get { return _Template[_Template.Length - 1]; }
            set { _Template[_Template.Length - 1] = value.ToString(); }
        } 

        #endregion


        public void CreateTemplate(string[] newTemplate)
        {
            try
            {
                _Template = newTemplate;
            }
            catch
            {
                LogWriter.Write("Failed to apply excel template"
                    + (newTemplate?.Count() > 1 ? $" {newTemplate[0]}." : ". Template is empty."));
            }
        }

        public void CreateTemplate(string newTemplate)
        {
            CreateTemplate(newTemplate.Split(','));
        }

        public void CreateDefaultTemplate()
        {
            _Template = GetDefaultTemplate();
        }


        /// <summary>
        /// Sets value of property by its index in the template.
        /// </summary>
        /// <param name="index">Index of property in the template.</param>
        /// <param name="value">Set value.</param>
        public void SetTemplatePropertyByIndex(int index, string value)
        {
            _Template[index] = value;
        }


        /// <summary>
        /// Searches for the index of column value in this template.
        /// Literally, searches for index of property in template that should be displayed in column number 'value'
        /// </summary>
        /// <param name="value">Value to be found</param>
        /// <returns>Index of lookup value. -1 if not found.</returns>
        internal int SearchValueIndex(int value)
        {
            for (int i = 3; i < _Template.Length; i++)
            {
                if (string.Equals(_Template[i], value.ToString()))
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Returns maximum column number stated in the template.
        /// </summary>
        /// <returns></returns>
        internal byte GetMaxColumnNumber()
        {
            byte value, maxValue = 0;
            for (byte i = 3; i < _Template.Length; i++)
            {
                if (byte.TryParse(_Template[i], out value))
                {
                    maxValue = maxValue < value ? value : maxValue;
                }
            }
            return maxValue;
        }

        /// <summary>
        /// Returns value converted to an integer found by its index.
        /// </summary>
        /// <param name="index">Index of value in template sequence</param>
        /// <returns>Value from template converted to an integer.</returns>
        internal int GetIntegerValueByIndex(int index)
        {
            if (int.TryParse(this[index], out var value))
            {
                return value;
            }
            return -1;
        }

        internal string this[int index] => _Template[index];

    }
}
