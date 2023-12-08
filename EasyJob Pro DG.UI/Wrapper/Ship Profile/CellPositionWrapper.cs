using EasyJob_ProDG.Model.Transport;
using EasyJob_ProDG.UI.Utility;
using System.Collections.Generic;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.Wrapper
{
    public class CellPositionWrapper : ModelWrapper<CellPosition>
    {
        #region Public properties

        public string DisplayPosition
        {
            get
            {
                return Model.ToString();
            }
            set
            {
                HasError = !Model.TryChangeCellPosition(value);
                if (IsEmpty) HasError = true;
            }
        }

        /// <summary>
        /// Used to display consequtive number in list.
        /// Also is used to remove the cell from list.
        /// </summary>
        public byte NumberInList
        {
            get { return numberInList; }
            set
            {
                numberInList = value;
                OnPropertyChanged();
            }
        }
        private byte numberInList;

        public bool IsEmpty => Model.Bay == 0 && Model.Row == 99 && Model.Tier == 0 && Model.HoldNr == 0 && Model.Underdeck == 0;
        public bool HasError { get; private set; }
        public bool HasErrorOrEmpty => HasError || Model.IsEmpty;

        #endregion

        #region Public methods and override methods

        public CellPosition ToCellPosition()
        {
            return Model;
        }

        public override string ToString()
        {
            return Model.ToString();
        } 

        #endregion


        #region Validation

        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            if (propertyName == "DisplayPosition")
            {
                if (HasError)
                    yield return "Enter a valid position";
                else if (IsEmpty)
                    yield return "Enter a valid position";
                else
                    yield return string.Empty;
            }
            else
            {
                yield return string.Empty;
            }
        } 

        #endregion

        #region Constructors

        public CellPositionWrapper(CellPosition model) : base(model)
        {
            InitializeCommands();
        }

        public CellPositionWrapper() : base(new CellPosition())
        {
            InitializeCommands();
        }

        #endregion

        #region Commands

        public ICommand RemoveCellCommand { get; private set; }
        private void InitializeCommands()
        {
            RemoveCellCommand = new DelegateCommand(RemoveCellPosition);
        }

        public delegate void OnRemoveCellRequested(byte numberInList);
        public event OnRemoveCellRequested RemoveCellRequested;

        private void RemoveCellPosition(object numberInList)
        {
            RemoveCellRequested?.Invoke((byte)numberInList);
        }

        #endregion
    }
}
