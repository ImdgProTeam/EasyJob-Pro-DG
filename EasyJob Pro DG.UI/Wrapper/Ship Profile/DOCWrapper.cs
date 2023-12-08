using EasyJob_ProDG.Model.Transport;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.Wrapper.Dummies;
using System.Collections.ObjectModel;
using System.Linq;

namespace EasyJob_ProDG.UI.Wrapper
{
    /// <summary>
    /// Wrapper class uses transposed model of DOC table
    /// </summary>
    public class DOCWrapper : Observable
    {
        #region Private fields

        /// <summary>
        /// Value assist to get correct IsChanged if the number of holds changed
        /// </summary>
        private bool _holdsChanged;

        /// <summary>
        /// Value assist to keep IsChanged as true if original number of holds has been reduced thus reset all the values to default after increase the number.
        /// </summary>
        private bool _everReducedNumberOfHolds;

        /// <summary>
        /// States the original number of cargo holds listed in DOC.
        /// </summary>
        private byte _originalNumberOfHolds;

        #endregion

        #region Public properties

        public byte NumberOfHolds => (byte)DOCTable[0].Row.Count;
        public byte NumberOfClasses => (byte)DOCTable.Count;
        public DOC Model { get; }
        public bool IsChanged => DOCTable.Any(r => r.IsChanged) || _holdsChanged;

        public ObservableCollection<DummyDOCRow> DOCTable
        {
            get; private set;
        }

        #endregion


        #region Internal methods

        /// <summary>
        /// Adds new cargo hold entry with default values to DOCTable
        /// </summary>
        /// <param name="holdNr"></param>
        internal void AddNewHold(byte holdNr)
        {
            if (NumberOfHolds - 1 < holdNr)
                for (byte c = 0; c < NumberOfClasses; c++)
                {
                    DOCTable[c].Add(1, c, holdNr);
                }
            _holdsChanged = true;
        }

        /// <summary>
        /// Removes the last hold entry from DOCTable
        /// </summary>
        internal void RemoveLastHold()
        {
            for (byte c = 0; c < NumberOfClasses; c++)
            {
                DOCTable[c].Row.RemoveAt(DOCTable[c].Row.Count - 1);
            }

            // logic to maintain IsChanged correct when number of holds changes
            if (_originalNumberOfHolds == NumberOfHolds && !_everReducedNumberOfHolds)
                _holdsChanged = false;
            else _holdsChanged = true;
            if (NumberOfHolds < _originalNumberOfHolds) _everReducedNumberOfHolds = true;
        }

        internal void AcceptChanges()
        {
            if (!IsChanged) return;

            foreach (DummyDOCRow row in DOCTable)
                foreach (var item in row.Row)
                {
                    item.AcceptChanges();
                }
            Model.DOCtable = CreatePlainDOCTable();
        }

        internal void RejectChanges()
        {
            if (!IsChanged) return;

            foreach (DummyDOCRow row in DOCTable)
                foreach (var item in row.Row)
                {
                    item.RejectChanges();
                }
        } 

        #endregion

        #region Private methods

        /// <summary>
        /// Sets <see cref="DOCTable"/> from provided docTable
        /// </summary>
        /// <param name="docTable"></param>
        private void SetToDOCTableObservable(byte[,] docTable)
        {
            DOCTable = ConvertToDOCTableObservable(docTable);
        }

        /// <summary>
        /// Creates <see cref="ObservableCollection{DummyDOCRow}"/> from provided docTable
        /// </summary>
        /// <param name="docTable"></param>
        /// <returns></returns>
        private ObservableCollection<DummyDOCRow> ConvertToDOCTableObservable(byte[,] docTable)
        {
            ObservableCollection<DummyDOCRow> _collection = new ObservableCollection<DummyDOCRow>();
            for (byte c = 0; c < docTable.GetLength(1); c++)
            {
                DummyDOCRow newRow = new DummyDOCRow(c);
                for (byte h = 0; h < docTable.GetLength(0); h++)
                {
                    newRow.Add(docTable[h, c], h, c);
                }
                _collection.Add(newRow);
            }
            return _collection;
        }

        /// <summary>
        /// Creates plain (Model) DOCTable from existing values
        /// </summary>
        /// <returns></returns>
        private byte[,] CreatePlainDOCTable()
        {
            var newArray = new byte[NumberOfHolds, NumberOfClasses];
            for (int hold = 0; hold < NumberOfHolds; hold++)
            {
                for (int classColumn = 0; classColumn < NumberOfClasses; classColumn++)
                {
                    newArray[hold, classColumn] = DOCTable[classColumn].Row[hold].Value;
                }
            }
            return newArray;
        } 

        #endregion


        #region Constructor

        public DOCWrapper(DOC model)
        {
            Model = model;
            SetToDOCTableObservable(model.DOCtable);
            _originalNumberOfHolds = NumberOfHolds;
        } 

        #endregion
    }
}
