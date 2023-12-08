using EasyJob_ProDG.Data.Info_data;
using EasyJob_ProDG.UI.Utility;
using System.Collections.ObjectModel;
using System.Linq;

namespace EasyJob_ProDG.UI.Wrapper.Dummies
{
    /// <summary>
    /// Dummy row contains all values for each hold for one class
    /// </summary>
    public class DummyDOCRow : Observable
    {
        #region Public properties

        public string ClassDescription { get; set; }
        public string ClassToolTip { get; set; }
        public bool IsChanged => Row.Any(v => v.IsChanged);
        public ObservableCollection<DummyRowValue> Row { get; set; }

        #endregion

        #region Public methods

        public void Add(byte value, byte hold, byte classRow)
        {
            Row.Add(new DummyRowValue(value, hold, classRow));
        }

        private void SetClassDescription(byte classRow)
        {
            ClassDescription = IMDGCode.DOCClassesDictionary.Keys.ElementAt(classRow);
            ClassToolTip = IMDGCode.DOCClassesDictionary[ClassDescription];
        }

        private void AddBytesArrayToRow(byte[] valuesArray, byte classRow)
        {
            byte hold = 0;
            foreach (byte value in valuesArray)
            {
                Row.Add(new DummyRowValue(value, classRow, hold));
                hold++;
            }
        } 

        #endregion

        #region Constructor

        public DummyDOCRow(byte classRow)
        {
            Row = new ObservableCollection<DummyRowValue>();
            SetClassDescription(classRow);
        } 

        #endregion
    }

    /// <summary>
    /// Contains a value for a single class and a single hold.
    /// </summary>
    public class DummyRowValue : ModelByteDummy
    {
        public DummyRowValue(byte value, byte classRow, byte holdNumber) : base(value, classRow, holdNumber)
        {

        }
    }
}
