using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.ViewModel.MainWindow;
using EasyJob_ProDG.UI.Wrapper;
using System.Collections.Generic;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.ViewModel
{
    public class DataGridContainersViewModel : DataGridContainerViewModelBase
    {
        #region Constructor
        // ---------- Constructor ---------------
        public DataGridContainersViewModel() : base()
        {
        }

        #endregion

        #region Startup logic

        /// <summary>
        /// Sets data source to View property
        /// </summary>
        protected override void SetDataView()
        {
            SetPlanViewSource(CargoPlan.Containers);
        }

        /// <summary>
        /// Assigns handler methods for commands
        /// </summary>
        protected override void LoadCommands()
        {
            AddNewDgCommand = new DelegateCommand(OnAddNewDg, CanAddNewDg);
        }

        /// <summary>
        /// Registers for messages in DataMessenger
        /// </summary>
        protected override void RegisterInDataMessenger()
        {
        }

        #endregion

        #region Add unit Logic

        protected override void OnAddNewUnit(object obj)
        {
            //Action
            CargoPlan.AddNewContainer(new Model.Cargo.Container()
            {
                ContainerNumber = unitToAddNumber,
                Location = unitToAddLocation.CorrectFormatContainerLocation()
            });

            //Scroll into the new Container
            SelectedUnit = CargoPlan.Containers[CargoPlan.Containers.Count - 1];
            OnPropertyChanged(nameof(SelectedUnit));
        }

        /// <summary>
        /// Switches to add a new Dg in DataGridDg with selected container number.
        /// </summary>
        /// <param name="obj"></param>
        private void OnAddNewDg(object obj)
        {
            ViewModelLocator.MainWindowViewModel.AddNewDgCommand.Execute(obj);
        }

        private bool CanAddNewDg(object obj)
        {
            return SelectedUnit != null;
        }

        #endregion

        #region Methods

        protected override void RemoveUnit(List<string> containerNumbers)
        {
            foreach (var number in containerNumbers)
            {
                CargoPlan.RemoveContainer(number);
            }
        }

        #endregion

        #region Commands
        // ----- Commands -----
        public ICommand AddNewDgCommand { get; private set; }

        #endregion
    }
}
