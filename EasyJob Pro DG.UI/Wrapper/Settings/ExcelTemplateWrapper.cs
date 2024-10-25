using EasyJob_ProDG.Model.IO.Excel;
using EasyJob_ProDG.UI.Wrapper.Settings;
using System.Collections.ObjectModel;
using System.Linq;

namespace EasyJob_ProDG.UI.Wrapper
{
    public abstract class ExcelTemplateWrapper<T> : ModelChangeTrackingWrapper<T> where T : ExcelTemplate
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
        public bool HasChanges
        {
            get
            {
                if (IsChanged) return true;
                if (ColumnProperties.Any(p => p.IsModified))
                    return true;
                return false;
            }
        }

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
            get { return GetValue<string>(); }
            set
            {
                SetValue<string>(value);
            }
        }
        public string TemplateNameOriginalValue => GetOriginalValue<string>(nameof(TemplateName));
        public bool TemplateNameIsChanged => GetIsChanged(nameof(TemplateName));

        /// <summary>
        /// Working sheet name.
        /// </summary>
        public string WorkingSheet
        {
            get { return GetValue<string>(); }
            set
            {
                SetValue<string>(value);
            }
        }
        public string WorkingSheetOriginalValue => GetOriginalValue<string>(nameof(WorkingSheet));
        public bool WorkingSheetIsChanged => GetIsChanged(nameof(WorkingSheet));


        /// <summary>
        /// The Row of excel sheet from which needed to start reading cargo info.
        /// </summary>
        public byte StartRow
        {
            get { return GetValue<byte>(); }
            set
            {
                SetValue<byte>(value);
            }
        }
        public byte WStartRowOriginalValue => GetOriginalValue<byte>(nameof(StartRow));
        public bool StartRowIsChanged => GetIsChanged(nameof(StartRow));

        #endregion


        #region Public methods

        public void SaveChanges()
        {
            AcceptChanges();
            WriteColumnPropertiesToTemplate();
        }

        /// <summary>
        /// Saves all the changes to Model template.
        /// </summary>
        public void ResetOriginalValues()
        {
            RejectChanges();
            foreach (var property in ColumnProperties)
            {
                if (property.IsModified)
                    property.RejectChanges();
            }
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

        private void WriteColumnPropertiesToTemplate()
        {
            for (int i = 0; i < ColumnProperties.Count; i++)
            {
                Model.SetTemplatePropertyByIndex(i + 3, ColumnProperties[i].Value.ToString());
                ColumnProperties[i].AcceptChanges();
            }
        }

        #endregion


        #region Constructor

        //Constructor
        public ExcelTemplateWrapper(T model) : base(model)
        {
            GenerateColumnProperties();
        }

        #endregion
    }
}
