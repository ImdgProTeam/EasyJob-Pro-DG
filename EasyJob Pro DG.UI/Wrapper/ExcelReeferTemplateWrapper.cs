using EasyJob_ProDG.Model.IO.Excel;
using EasyJob_ProDG.UI.Wrapper.Dummies;
using System.Collections.ObjectModel;

namespace EasyJob_ProDG.UI.Wrapper
{
    public class ExcelReeferTemplateWrapper : ModelWrapper<ExcelReeferTemplate>
    {
        private string _originalTemplateName;
        private byte _originalStartRow;

        //Constructor
        public ExcelReeferTemplateWrapper(ExcelReeferTemplate model) : base(model)
        {
            GenerateColumnProperties();
            RememberOriginalValues();
        }


        private ObservableCollection<ExcelColumnProperty> _columnProperties;
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


        private bool _isChanged;
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

        internal string GetTemplateString()
        {
            return string.Join(",", Model.GetTemplate);
        }

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
        /// Saves original values in memory
        /// </summary>
        private void RememberOriginalValues()
        {
            _originalStartRow = StartRow;
            _originalTemplateName = TemplateName;
        }

        /// <summary>
        /// Restores original values from memory
        /// </summary>
        private void RestoreOriginalValues()
        {
            StartRow = _originalStartRow;
            TemplateName = _originalTemplateName;
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
            RestoreOriginalValues();
        }

        /// <summary>
        /// Saves column properties values in Model properties
        /// </summary>
        public void UploadChangesFromColumnProperties()
        {
            int i = 1;
            foreach (var updatedProperty in ColumnProperties)
            {
                Model.GetTemplate[i++] = (byte)updatedProperty.Value;
            }
        }



        //Template Properties
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
        public byte StartRow
        {
            get { return GetValue<byte>(); }
            set
            {
                SetValue(value);
                IsChanged = true;
            }
        }
    }
}
