using System.Collections.ObjectModel;
using System.Linq;
using EasyJob_ProDG.Model.IO.Excel;
using EasyJob_ProDG.UI.Wrapper.Dummies;

namespace EasyJob_ProDG.UI.Wrapper
{
    public class ExcelTemplateWrapper : ModelWrapper<ExcelTemplate>
    {
        private string[] _ignorableProperties = new string[] { "Model", "Item", "ColumnProperties", "Error", "TemplateName", "StartRow", "WorkingSheet", "IsChanged", "HasErrors" };
        private string _originalWorkingSheet;
        private string _originalTemplateName;
        private int _originalStartRow;

        //Properties
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

        //Constructor
        public ExcelTemplateWrapper(ExcelTemplate model) : base(model)
        {
            AutoGenerateColumnProperties();
            RememberOriginalValues();
        }

        //Methods

        /// <summary>
        /// Adds column properties to ColumnProperties list.
        /// </summary>
        private void AutoGenerateColumnProperties()
        {
            ColumnProperties = null;
            foreach (var property in this.GetType().GetProperties())
            {
                //ignorrable properties
                if (_ignorableProperties.Contains(property.Name)) continue;
                ColumnProperties.Add(new ExcelColumnProperty(property.Name, (int)property.GetValue(this)));
            }
        }

        /// <summary>
        /// Saves original values in memory
        /// </summary>
        private void RememberOriginalValues()
        {
            _originalStartRow = StartRow;
            _originalTemplateName = TemplateName;
            _originalWorkingSheet = WorkingSheet;
        }

        /// <summary>
        /// Restores original values from memory
        /// </summary>
        private void RestoreOriginalValues()
        {
            StartRow = _originalStartRow;
            TemplateName = _originalTemplateName;
            WorkingSheet = _originalWorkingSheet;
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
            AutoGenerateColumnProperties();
            RestoreOriginalValues();
        }

        /// <summary>
        /// Saves column properties values in Model properties
        /// </summary>
        public void UploadChangesFromColumnProperties()
        {
            foreach (var updatedProperty in ColumnProperties)
            {
                foreach (var modelProperty in Model.GetType().GetProperties())
                {
                    if (modelProperty.Name == updatedProperty.PropertyName)
                        typeof(ExcelTemplate).GetProperty(modelProperty.Name)?.SetValue(Model, updatedProperty.Value);
                }
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
        public int StartRow
        {
            get { return GetValue<int>(); }
            set
            {
                SetValue(value);
                IsChanged = true;
            }
        }
        public string WorkingSheet
        {
            get { return GetValue<string>(); }
            set
            {
                SetValue(value);
                IsChanged = true;
            }
        }

        //Column properties
        #region Column properties

        public int ColumnContainerNumber
        {
            get { return GetValue<int>(); }
            set
            {
                SetValue(value);
            }
        }
        public int ColumnLocation
        {
            get { return GetValue<int>(); }
            set
            {
                SetValue(value);
            }
        }
        public int ColumnUnno
        {
            get { return GetValue<int>(); }
            set
            {
                SetValue(value);
            }
        }
        public int ColumnPOL
        {
            get { return GetValue<int>(); }
            set
            {
                SetValue(value);
            }
        }
        public int ColumnPOD
        {
            get { return GetValue<int>(); }
            set
            {
                SetValue(value);
            }
        }
        public int ColumnClass
        {
            get { return GetValue<int>(); }
            set
            {
                SetValue(value);
            }
        }
        public int ColumnSubclass
        {
            get { return GetValue<int>(); }
            set
            {
                SetValue(value);
            }
        }
        public int ColumnName
        {
            get { return GetValue<int>(); }
            set
            {
                SetValue(value);
            }
        }
        public int ColumnPkg
        {
            get { return GetValue<int>(); }
            set
            {
                SetValue(value);
            }
        }
        public int ColumnFP
        {
            get { return GetValue<int>(); }
            set
            {
                SetValue(value);
            }
        }
        public int ColumnMP
        {
            get { return GetValue<int>(); }
            set
            {
                SetValue(value);
            }
        }
        public int ColumnLQ
        {
            get { return GetValue<int>(); }
            set
            {
                SetValue(value);
            }
        }
        public int ColumnEms
        {
            get { return GetValue<int>(); }
            set
            {
                SetValue(value);
            }
        }
        public int ColumnRemark
        {
            get { return GetValue<int>(); }
            set
            {
                SetValue(value);
            }
        }
        public int ColumnNetWeight
        {
            get { return GetValue<int>(); }
            set
            {
                SetValue(value);
            }
        }
        public int ColumnTechName
        {
            get { return GetValue<int>(); }
            set
            {
                SetValue(value);
            }
        }
        public int ColumnPackage
        {
            get { return GetValue<int>(); }
            set
            {
                SetValue(value);
            }
        }
        public int ColumnFinalDestination
        {
            get { return GetValue<int>(); }
            set
            {
                SetValue(value);
            }
        }
        public int ColumnOperator
        {
            get { return GetValue<int>(); }
            set
            {
                SetValue(value);
            }
        }
        public int ColumnEmergencyContact
        {
            get { return GetValue<int>(); }
            set
            {
                SetValue(value);
            }
        }
        #endregion
    }
}
