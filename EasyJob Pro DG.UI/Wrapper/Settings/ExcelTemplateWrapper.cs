using EasyJob_ProDG.Model.IO.Excel;
using EasyJob_ProDG.UI.Wrapper.Dummies;
using System.Collections.ObjectModel;

namespace EasyJob_ProDG.UI.Wrapper
{
    public abstract class ExcelTemplateWrapper<T> : ModelWrapper<T>
        where T : ExcelTemplate
    {
        protected abstract string[] ColumnTitles { get; }

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

        public string TemplateNameInSettings => Model.TemplateSettingsName;
        public string TemplateString => Model.GetTemplateString();


        /// <summary>
        /// Title of Template to use in case of multiple templates.
        /// </summary>
        public string TemplateName
        {
            get { return _templateName; }
            set
            {
                if (_templateName != value)
                {
                    _templateName = value;
                    OnPropertyChanged();
                    IsChanged = true;
                }
            }
        }
        private string _templateName;

        /// <summary>
        /// Working sheet name.
        /// </summary>
        public string WorkingSheet
        {
            get { return _workingSheet; }
            set
            {
                if (_workingSheet != value)
                {
                    _workingSheet = value;
                    OnPropertyChanged();
                    IsChanged = true;
                }
            }
        }
        private string _workingSheet;

        /// <summary>
        /// The Row of excel sheet from which needed to start reading cargo info.
        /// </summary>
        public byte StartRow
        {
            get { return _startRow; }
            set
            {
                if (_startRow != value)
                {
                    _startRow = value;
                    OnPropertyChanged();
                    IsChanged = true;
                }
            }
        }
        private byte _startRow;

        #endregion


        #region Public methods

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
            UploadMainTemplateProperties();
            UploadChangesFromColumnProperties();
        }

        #endregion


        #region Private methods

        /// <summary>
        /// Adds column properties to ColumnProperties list.
        /// </summary>
        protected void GenerateColumnProperties()
        {
            if (ColumnTitles == null || ColumnTitles?.Length == 0) return;

            ColumnProperties = null;
            for (int i = 3; i < Model.GetTemplate().Length - 1; i++)
            {
                ColumnProperties.Add(new ExcelColumnProperty(ColumnTitles[i], int.Parse(Model.GetTemplate()[i])));
            }
        }

        /// <summary>
        /// Restores original values of main properties from model
        /// </summary>
        protected void GetMainTemplateProperties()
        {
            StartRow = Model.StartRow;
            TemplateName = Model.TemplateName;
            WorkingSheet = Model.WorkingSheet;
        }

        /// <summary>
        /// Uploads main properties to model
        /// </summary>
        private void UploadMainTemplateProperties()
        {
            Model.TemplateName = TemplateName;
            Model.WorkingSheet = WorkingSheet;
            Model.StartRow = StartRow;
        }

        /// <summary>
        /// Saves column properties values in Model properties
        /// </summary>
        private void UploadChangesFromColumnProperties()
        {
            int i = 3;
            foreach (var updatedProperty in ColumnProperties)
            {
                Model.GetTemplate()[i++] = updatedProperty.Value.ToString();
            }
        }

        #endregion


        #region Constructor

        //Constructor
        public ExcelTemplateWrapper(T model) : base(model)
        {

        }

        #endregion
    }
}
