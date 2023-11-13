using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.Data.Info_data;
using System.Collections.ObjectModel;
using System.Linq;

namespace EasyJob_ProDG.UI.Wrapper.Dummies
{
    /// <summary>
    /// Dummy row contains all values for each hold for one class
    /// </summary>
    public class DummyDOCRow : Observable
    {
        public string ClassDescription { get; set; }
        public string ClassToolTip { get; set; }
        public ObservableCollection<DummyRowValue> Row { get; set; }

        public DummyDOCRow()
        {
            Row = new ObservableCollection<DummyRowValue>();
        }

        public DummyDOCRow(byte[] row)
        {
            Row = new ObservableCollection<DummyRowValue>();
            AddBytesArrayToRow(row);
            //OnPropertyChanged("Row");
            //OnPropertyChanged("DummyRowValue");
            //OnPropertyChanged();
        }
        public DummyDOCRow(byte holdNr)
        {
            Row = new ObservableCollection<DummyRowValue>();
            SetClassDescription(holdNr);
        }

        public DummyDOCRow(byte[] row, byte holdNr)
        {
            Row = new ObservableCollection<DummyRowValue>();
            AddBytesArrayToRow(row);
            SetClassDescription(holdNr);
        }
        public void Add(byte value)
        {
            Row.Add(new DummyRowValue(value));
            //OnPropertyChanged("Row");
            //OnPropertyChanged("DummyRowValue");
        }
        private void SetClassDescription(byte classNr)
        {
            ClassDescription = IMDGCode.DOCClassesDictionary.Keys.ElementAt(classNr);
            ClassToolTip = IMDGCode.DOCClassesDictionary[ClassDescription];
        }
        private void AddBytesArrayToRow(byte[] row)
        {
            foreach (byte value in row)
                Row.Add(new DummyRowValue(value));
        }

    }

    public class DummyRowValue : Observable
    {
        private byte _value;
        public byte Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged();
            }
        }

        public DummyRowValue()
        {

        }
        public DummyRowValue(byte value)
        {
            _value = value;
            OnPropertyChanged();
        }
        public override string ToString()
        {
            return _value.ToString();
        }
    }
}
