using EasyJob_ProDG.UI.Services.DialogServices;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.Wrapper;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.ViewModel.MainWindow
{
    /// <summary>
    /// Common abstract class for DataGridContainerViewModel and DataGridReefersViewModel
    /// </summary>
    public abstract class DataGridContainerViewModelBase : DataGridViewModelBase
    {
        //--------------- Public properties -----------------------------------------
        public ContainerWrapper SelectedUnit { get; set; }

        #region Constructor
        // ----- Constructor ------

        protected DataGridContainerViewModelBase() : base()
        {
            LoadBaseCommands();
        }

        #endregion

        #region Startup Logic

        private void LoadBaseCommands()
        {
            AddNewUnitCommand = new DelegateCommand(OnAddNewUnit);
            DisplayAddUnitMenuCommand = new DelegateCommand(OnDisplayAddUnitMenu);
            DeleteUnitCommand = new DelegateCommand(OnDeleteUnitsRequested);
        }

        #endregion

        #region Add unit logic

        /// <summary>
        /// Actions on displaying AddDg menu (on click 'Add' button)
        /// </summary>
        /// <param name="obj"></param>
        internal void OnDisplayAddUnitMenu(object obj = null)
        {
            UnitToAddNumber = SelectedUnit?.ContainerNumber;
            UnitToAddLocation = SelectedUnit?.Location;

            MenuVisibility = System.Windows.Visibility.Visible;
            OnPropertyChanged(nameof(MenuVisibility));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Method changes SelectedReefer to match with the selected number (e.g. with ConflictPanelItem object)
        /// </summary>
        /// <param name="obj">Selected container number</param>
        internal void SelectUnit(string containerNumber)
        {
            SelectedUnit = null;
            OnPropertyChanged(nameof(SelectedUnit));

            //Set new selection
            foreach (ContainerWrapper container in UnitsPlanView)
            {
                if (string.Equals(container.ContainerNumber, containerNumber))
                {
                    SelectedUnit = container;
                    break;
                }
            }
            OnPropertyChanged(nameof(SelectedUnit));
        }

        protected override void OnSelectionChanged(object obj)
        {
            SetSelectionStatusBar(obj);
            if (SelectedUnit is null) return;

            if (MenuVisibility == System.Windows.Visibility.Visible)
            {
                if (SelectedUnit.ContainerNumber != UnitToAddNumber)
                {
                    UnitToAddNumber = SelectedUnit?.ContainerNumber;
                    UnitToAddLocation = SelectedUnit?.Location;
                }
            }
            selectionObject = obj;
        }

        private void OnDeleteUnitsRequested(object obj)
        {
            if (SelectedUnit == null) return;
            var count = ((ICollection)obj).Count;

            if (_messageDialogService.ShowYesNoDialog($"Do you want to delete selected unit" + (count > 1 ? $"s ({count})" : "") + "?", "Delete cargo")
                == MessageDialogResult.No) return;

            List<string> list = new List<string>();
            foreach (ContainerWrapper item in (ICollection)obj)
            {
                list.Add(item.ContainerNumber);
            }

            RemoveUnit(list);
        }

        /// <summary>
        /// Method to be implemented specifically for containers or reefers to remove items from CargoPlan
        /// </summary>
        /// <param name="list"></param>
        protected abstract void RemoveUnit(List<string> containerNumberList);

        #endregion

        #region Commands
        // ----- Commands ------
        public ICommand AddNewUnitCommand { get; private set; }
        public ICommand DisplayAddUnitMenuCommand { get; private set; }
        public ICommand DeleteUnitCommand { get; private set; }

        #endregion
    }
}
