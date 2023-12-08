using EasyJob_ProDG.UI.Utility;

namespace EasyJob_ProDG.UI.Wrapper
{
    public abstract class ModelByteDummy : Observable
    {
        private byte _value;
        private byte _originalValue;

        public byte Index1;
        public byte Index2;

        public bool IsChanged => _originalValue != _value;
        public byte Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged(null);
                OnDummyValueChanged?.Invoke(_value, Index1, Index2);
            }
        }

        /// <summary>
        /// Used to manipulate original value property in order to get correct <see cref="IsChanged"/> property.
        /// </summary>
        /// <param name="value"></param>
        public void ChangeOriginalValue(byte value)
        {
            _originalValue = value;
        }

        public virtual void RejectChanges()
        {
            _value = _originalValue;
            OnPropertyChanged(nameof(Value));
            OnPropertyChanged(nameof(IsChanged));
        }

        public virtual void AcceptChanges()
        {
            _originalValue = _value;
            OnPropertyChanged(nameof(IsChanged));
        }

        public delegate void DummyValueChanged(byte newValue, byte index1, byte index2 = 0);
        public event DummyValueChanged OnDummyValueChanged = null;

        public ModelByteDummy(byte value, byte index1, byte index2 = 0)
        {
            _value = value;
            _originalValue = value;
            Index1 = index1;
            Index2 = index2;
        }
    }
}
