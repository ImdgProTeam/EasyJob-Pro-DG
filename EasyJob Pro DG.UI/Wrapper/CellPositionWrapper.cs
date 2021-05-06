using EasyJob_ProDG.Model.Transport;
using System.Collections.Generic;

namespace EasyJob_ProDG.UI.Wrapper
{
    public class CellPositionWrapper : ModelWrapper<CellPosition>
    {
        public CellPositionWrapper() : base(new CellPosition())
        {

        }

        public string DisplayPosition
        {
            get
            {
                return Model.ToString();
            }
            set
            {
                HasError = !Model.TryChangeCellPosition(value);
                if (IsEmpty()) HasError = true;
            }
        }

        private byte numberInList;
        public byte NumberInList
        {
            get { return numberInList; }
            set
            {
                numberInList = value;
                OnPropertyChanged();
            }
        }
        public bool HasError { get; set; }

        protected override IEnumerable<string> ValidateProperty(string propertyName)
        {
            if (propertyName == "DisplayPosition")
            {
                if (HasError)
                    yield return "Enter a valid position";
                else if (IsEmpty())
                    yield return "Enter a valid position";
                else
                    yield return string.Empty;
            }
            else
            {
                yield return string.Empty;
            }
        }

        public CellPositionWrapper(CellPosition model) : base(model)
        {
            //this.Bay = model.Bay;
            //this.Row = model.Row;
            //this.Tier = model.Tier;
            //this.HoldNr = model.HoldNr;
            //this.Underdeck = model.Underdeck;
        }

        public CellPosition ToCellPosition()
        {
            return Model;
        }

        public override string ToString()
        {
            return Model.ToString();
        }
        
        internal bool IsEmpty()
        {
            if (Model.Bay == 0 && Model.Row == 99 && Model.Tier == 0 && Model.HoldNr == 0 && Model.Underdeck == 0)
                return true;
            else return false;
        }
    }
}
