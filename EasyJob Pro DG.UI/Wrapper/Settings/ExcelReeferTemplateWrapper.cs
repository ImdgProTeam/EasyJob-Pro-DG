using EasyJob_ProDG.Model.IO.Excel;
using EasyJob_ProDG.UI.Wrapper.Dummies;
using System.Collections.ObjectModel;

namespace EasyJob_ProDG.UI.Wrapper
{
    public class ExcelReeferTemplateWrapper : ModelWrapper<ExcelReeferTemplate>
    {
        #region Private fields

        private string _originalTemplateName;
        private byte _StartRow;

        #endregion

        #region Constructor

        //Constructor
        public ExcelReeferTemplateWrapper(ExcelReeferTemplate model) : base(model)
        {
            GenerateColumnProperties();
            GetMainTemplateProperties();
        }

        #endregion


        #region Private methods

        /// <summary>
        /// Adds column properties to ColumnProperties list.
        /// </summary>
        private void GenerateColumnProperties()
        {
            ColumnProperties = null;
            ColumnProperties.Add(new ExcelColumnProperty("Container number", Model.GetTemplate[1]));
            ColumnProperties.Add(new ExcelColumnProperty("Commodity", Model.GetTemplate[2]));
            ColumnProperties.Add(new ExcelColumnProperty("Set temp", Model.GetTemplate[3]));
            ColumnProperties.Add(new ExcelColumnProperty("Vent settings", Model.GetTemplate[4]));
            ColumnProperties.Add(new ExcelColumnProperty("Special", Model.GetTemplate[5]));
            ColumnProperties.Add(new ExcelColumnProperty("Remarks", Model.GetTemplate[6]));

        }

        /// <summary>
        /// Restores original values from memory
        /// </summary>
        private void GetMainTemplateProperties()
        {
            StartRow = Model.GetTemplate[0];
            //Template name not implemented
        }

        /// <summary>
        /// Saves column properties values in Model properties
        /// </summary>
        private void UploadChangesFromColumnProperties()
        {
            int i = 1;
            foreach (var updatedProperty in ColumnProperties)
            {
                Model.GetTemplate[i++] = (byte)updatedProperty.Value;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns template values as a string joined with ','
        /// </summary>
        /// <returns>Template string</returns>
        internal string GetTemplateString()
        {
            return string.Join(",", Model.GetTemplate);
        }

        /// <summary>
        /// Changes status of each property to 'not modified'
        /// </summary>
        public void ResetAllChangeIndicators()
        {
            IsChanged = false;
            foreach (var property in ColumnProperties)
            {
                property.IsModified = false;
            }
        }

        /// <summary>
        /// Restores initial values
        /// </summary>
        public void CancelChanges()
        {
            GenerateColumnProperties();
            GetMainTemplateProperties();
        }

        /// <summary>
        /// Saves all the changes to Model template.
        /// </summary>
        public void UploadTemplateChanges()
        {
            Model.StartRow = StartRow;
            UploadChangesFromColumnProperties();
        }

        #endregion


        #region Public properties

        public ObservableCollection<ExcelColumnProperty> ColumnProperties
        {
            get
            {
                return _columnProperties ??= new ObservableCollection<ExcelColumnProperty>();
            }
            set
            {
                _columnProperties = value;
                OnPropertyChanged();
            }
        }
        private ObservableCollection<ExcelColumnProperty> _columnProperties;

        /// <summary>
        /// Indicates if the template has been changed
        /// </summary>
        public bool IsChanged
        {
            get
            {
                if (_isChanged) return true;
                foreach (var property in ColumnProperties)
                {
                    if (property.IsModified) return true;
                }
                return false;
            }
            private set
            {
                _isChanged = value;
            }
        }
        private bool _isChanged;

        #endregion

        #region Template properties

        //Template Properties
        /// <summary>
        /// Title of Template to use in case of multiple templates.
        /// Not implemented.
        /// </summary>
        public string TemplateName
        {
            get { return GetValue<string>(); }
            set
            {
                SetValue(value);
                OnPropertyChanged();
                IsChanged = true;
            }
        }

        /// <summary>
        /// The Row of excel sheet from which needed to start reading reefers info.
        /// </summary>
        public byte StartRow
        {
            get { return _StartRow; }
            set
            {
                if (_StartRow == value) return;
                _StartRow = value;
                IsChanged = true;
            }
        } 

        #endregion

    }
}
