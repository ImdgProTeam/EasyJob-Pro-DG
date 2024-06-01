using EasyJob_ProDG.UI.Services.DialogServices;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.ViewModel.MainWindow;
using EasyJob_ProDG.UI.Wrapper;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.ViewModel
{
    public class DataGridContainersViewModel : DataGridViewModelBase
    {
        // ----- Public properties -----
        public ContainerWrapper SelectedContainer { get; set; }


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
            AddNewContainerCommand = new DelegateCommand(OnAddNewUnit);
            AddNewDgCommand = new DelegateCommand(OnAddNewDg, CanAddNewDg);
            DisplayAddContainerMenuCommand = new DelegateCommand(OnDisplayAddContainerMenu);
            DeleteContainerCommand = new DelegateCommand(OnDeleteContainersRequested);
        }

        /// <summary>
        /// Registers for messages in DataMessenger
        /// </summary>
        protected override void RegisterInDataMessenger()
        {

        }

        #endregion

        #region AddContainer Logic

        /// <summary>
        /// Method changes SelectedUnit to match with the selected number (e.g. with ConflictPanelItem object)
        /// </summary>
        /// <param name="obj">Selected container number</param>
        internal void SelectContainer(string containerNumber)
        {
            SelectedContainer = null;
            OnPropertyChanged(nameof(SelectedContainer));

            //Set new selection
            foreach (ContainerWrapper container in UnitsPlanView)
            {
                if (string.Equals(container.ContainerNumber, containerNumber))
                {
                    SelectedContainer = container;
                    break;
                }
            }
            OnPropertyChanged(nameof(SelectedContainer));
        }

        /// <summary>
        /// Actions on displaying AddDg menu (on click 'Add' button)
        /// </summary>
        /// <param name="obj"></param>
        internal void OnDisplayAddContainerMenu(object obj = null)
        {
            UnitToAddNumber = SelectedContainer?.ContainerNumber;
            UnitToAddLocation = SelectedContainer?.Location;

            MenuVisibility = System.Windows.Visibility.Visible;
            OnPropertyChanged(nameof(MenuVisibility));
        }

        protected override void OnAddNewUnit(object obj)
        {
            //Action
            CargoPlan.AddNewContainer(new Model.Cargo.Container()
            {
                ContainerNumber = unitToAddNumber,
                Location = unitToAddLocation.CorrectFormatContainerLocation()
            });

            //Scroll into the new Container
            SelectedContainer = CargoPlan.Containers[CargoPlan.Containers.Count - 1];
            OnPropertyChanged(nameof(SelectedContainer));
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
            return SelectedContainer != null;
        }

        #endregion

        #region DataGrid interactions

        private void OnDeleteContainersRequested(object obj)
        {
            if (SelectedContainer == null) return;
            var count = ((ICollection)obj).Count;

            if (_messageDialogService.ShowYesNoDialog($"Do you want to delete selected container" + (count > 1 ? $"s ({count})" : "") + "?", "Delete cargo")
                == MessageDialogResult.No) return;

            var list = new List<string>();
            foreach (ContainerWrapper item in (ICollection)obj)
            {
                list.Add(item.ContainerNumber);
            }

            foreach (var number in list)
            {
                CargoPlan.RemoveContainer(number);
            }
        }

        protected override void OnSelectionChanged(object obj)
        {
            SetSelectionStatusBar(obj);

            if (SelectedContainer is null) return;

            if (MenuVisibility == System.Windows.Visibility.Visible)
            {
                if (SelectedContainer.ContainerNumber != unitToAddNumber)
                {
                    UnitToAddNumber = SelectedContainer?.ContainerNumber;
                    UnitToAddLocation = SelectedContainer?.Location;
                }
            }
            selectionObject = obj;
        }

        #endregion

        #region Commands
        //--------------- Commands ----------------------------------------
        public ICommand AddNewContainerCommand { get; private set; }
        public ICommand AddNewDgCommand { get; private set; }
        public ICommand DisplayAddContainerMenuCommand { get; private set; }
        public ICommand DeleteContainerCommand { get; private set; }
        #endregion
    }
}
