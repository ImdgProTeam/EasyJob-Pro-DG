using EasyJob_ProDG.UI.Data;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.Wrapper;

namespace EasyJob_ProDG.UI.ViewModel
{
    public abstract class ConflictPanelItemViewModel : Observable
    {
        #region Internal members

        // --------------- Internal properties ------------------------------------

        public ConflictTypes ConflictType { get; protected set; }

        public string Code { get; protected set; }

        #endregion


        #region Display properties

        /// <summary>
        /// Textual description of the conflict to be displayed in Conflict item.
        /// </summary>
        public string DisplayText
        {
            get
            {
                return CreateDisplayText();
            }
            set
            {
                OnPropertyChanged();
            }
        }

        public virtual string DisplayConflictHeader { get; protected set; }

        /// <summary>
        /// Titles for group spoilers in Conflict list
        /// </summary>
        public abstract string ConflictGroupTitle { get; }

        #endregion





        #region Methods

        /// <summary>
        /// Calls OnPropertyChanged for its properties
        /// </summary>
        /// <param name="obj"></param>
        internal virtual void RefreshConflictText()
        {
            OnPropertyChanged(nameof(DisplayText));
            OnPropertyChanged(nameof(DisplayConflictHeader));
        }

        protected abstract string CreateDisplayText();

        #endregion

        #region Override system methods
        // ---------------- Overrided system methods ----------------------------

        public override string ToString()
        {
            string result = DisplayConflictHeader + ": " + DisplayText;
            return result;
        }

        public virtual bool Equals(ConflictPanelItemViewModel conflict)
        {
            return this == conflict;
        }
        #endregion

        #region Constructor 

        // --------------- Public constructors ----------------------------------

        public ConflictPanelItemViewModel()
        {

        }
        #endregion
    }
}
