//****************************************************//
//                                                    //
//   Public class to access data from Model.          //
//   * Provides access to ship profile                //
//   * Provides access to cargo                       //
//   *  //
//                                                    //
//****************************************************//

using EasyJob_ProDG.Model;
using EasyJob_ProDG.Model.Cargo;
using EasyJob_ProDG.Model.Transport;


namespace EasyJob_ProDG.UI.Data
{
    /// <summary>
    /// Provides program data access to current <see cref="CargoPlan"/>, 
    /// <see cref="ShipProfile"/> and condition file name in use.
    /// </summary>
    internal class CurrentProgramData : ICurrentProgramData
    {
        #region Private fields
        // ----- Private fields -----

        private static CargoPlan _cargoPlan;
        private static string _conditionFileName;
        private static ShipProfile _shipProfile => ProgramFiles.OwnShipProfile;


        #endregion

        #region Public properties
        // ----- Public use program files -----
        
        /// <summary>
        /// Current working <see cref="CargoPlan"/>
        /// </summary>
        CargoPlan ICurrentProgramData.CargoPlan => _cargoPlan;

        /// <summary>
        /// Current working condition file name
        /// </summary>
        public string ConditionFileName => _conditionFileName;

        #endregion


        #region Setters methods
        // ----- Setters methods -----

        /// <summary>
        /// Sets <see cref="ConditionFileName"/> property value.
        /// </summary>
        /// <param name="name">New condition file name.</param>
        void ICurrentProgramData.SetConditionFileName(string name)
        {
            _conditionFileName = name;
        }

        /// <summary>
        /// Sets <see cref="CargoPlan"/> given as parameter as working one.
        /// </summary>
        /// <param name="cargoPlan"></param>
        void ICurrentProgramData.SetCargoPlan(CargoPlan cargoPlan)
        {
            _cargoPlan = cargoPlan;
        }

        #endregion


        #region Get data methods
        // ----- Get data methods -----

        CargoPlan ICurrentProgramData.GetCargoPlan()
        {
            return _cargoPlan;
        }

        public ShipProfile GetShipProfile()
        {
            return _shipProfile;
        }

        #endregion


        #region Constructors and Singleton
        // ---------------- Constructors --------------------------------------------------------

        /// <summary>
        /// Empty constructor
        /// </summary>
        private CurrentProgramData()
        {

        }

        private static CurrentProgramData instance = null;
        internal static CurrentProgramData GetCurrentProgramData()
        {
            if (instance == null)
                instance = new CurrentProgramData();
            return instance;
        }

        #endregion
    }
}
