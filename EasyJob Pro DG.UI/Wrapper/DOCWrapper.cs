using EasyJob_ProDG.Model.Transport;
using EasyJob_ProDG.UI.Wrapper.Dummies;
using System.Collections.ObjectModel;

namespace EasyJob_ProDG.UI.Wrapper
{
    /// <summary>
    /// Wrapper class uses transposed model of DOC table
    /// </summary>
    public class DOCWrapper : ModelWrapper<DOC>
    {
        public string HoldNumber { get; set; }
        public byte NumberOfHolds => (byte)DOCTable[0].Row.Count;
        public byte NumberOfClasses => (byte)DOCTable.Count;
        public ObservableCollection<DummyDOCRow> DOCTable
        {
            get; set;
        }

        public DOCWrapper(DOC model) : base(model)
        {
        }

        internal void SetToDOCTableObservable(byte[,] docTable)
        {
            DOCTable = ConvertToDOCTableObservable(docTable);
        }
        internal ObservableCollection<DummyDOCRow> ConvertToDOCTableObservable(byte[,] docTable)
        {
            ObservableCollection<DummyDOCRow> _collection = new ObservableCollection<DummyDOCRow>();
            for (byte c = 0; c < docTable.GetLength(1); c++)
            {
                DummyDOCRow newRow = new DummyDOCRow(c);
                for (byte h = 0; h < docTable.GetLength(0); h++)
                {
                    newRow.Add(docTable[h, c]);
                }
                _collection.Add(newRow);
            }
            return _collection;
        }
        internal void SetDOCTableFromModel()
        {
            SetToDOCTableObservable(Model.DOCtable);
        }
        internal void AddNewHold(byte holdNr)
        {
            if (NumberOfHolds - 1 < holdNr)
                for (byte c = 0; c < NumberOfClasses; c++)
                {
                    DOCTable[c].Add(1);
                }

        }
        internal void RemoveLastHold()
        {
            for (byte c = 0; c < NumberOfClasses; c++)
            {
                DOCTable[c].Row.RemoveAt(DOCTable[c].Row.Count - 1);
            }
        }
    }
}
