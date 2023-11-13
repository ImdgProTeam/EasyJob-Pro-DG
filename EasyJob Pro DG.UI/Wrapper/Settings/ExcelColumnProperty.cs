using EasyJob_ProDG.UI.Utility;

namespace EasyJob_ProDG.UI.Wrapper.Settings
{
    /// <summary>
    /// Contains parameters of column property for excel template modification.
    /// </summary>
    public class ExcelColumnProperty : Observable
    {
        int _value;
        int _valueOriginal;
        private readonly int _maxValue = Model.IO.Excel.WithXl.Columns.Count - 1;

        public ExcelColumnProperty(string propertyName, int value)
        {
            PropertyName = propertyName;
            _value = value;
            _valueOriginal = value;
        }

        public bool IsModified => _value != _valueOriginal;
        public string PropertyName { get; }
        public int Value
        {
            get { return _value; }
            set
            {
                if (_value == value) return;
                _value = ValidateValue(value);
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsModified));
            }
        }

        public void RejectChanges()
        {
            _value = _valueOriginal;
            OnPropertyChanged(nameof(Value));
            OnPropertyChanged(nameof(IsModified));
        }

        public void AcceptChanges()
        {
            _valueOriginal = _value;
            OnPropertyChanged(nameof(IsModified));
        }


        /// <summary>
        /// Validates user input.
        /// </summary>
        /// <param name="value">Maximum allowed value for input.</param>
        /// <returns>Validated input value.</returns>
        private int ValidateValue(int value)
        {
            if (value < 0)
                return 0;
            if (value > _maxValue)
                return value = _maxValue;
            return value;
        }
    }
}

