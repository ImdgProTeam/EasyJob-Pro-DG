using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Services.DialogServices;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.ViewModel.MainWindow;
using EasyJob_ProDG.UI.Wrapper;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.ViewModel
{
    public class DataGridReefersViewModel : DataGridViewModelBase
    {
        //--------------- Public properties -----------------------------------------
        public ContainerWrapper SelectedReefer { get; set; }

        #region Constructor

        // ---------- Constructor ---------------
        public DataGridReefersViewModel() : base()
        {
        }

        #endregion

        #region StartUp logic

        /// <summary>
        /// Sets data source to View property
        /// </summary>
        protected override void SetDataView()
        {
            SetPlanViewSource(CargoPlan.Reefers);
        }

        /// <summary>
        /// Assigns handler methods for commands
        /// </summary>
        protected override void LoadCommands()
        {
            AddNewReeferCommand = new DelegateCommand(OnAddNewUnit);
            DisplayAddReeferMenuCommand = new DelegateCommand(OnDisplayAddReeferMenu);
            DeleteReefersCommand = new DelegateCommand(OnDeleteReefersRequested);
        }

        /// <summary>
        /// Subscribe for messages in DataMessenger
        /// </summary>
        protected override void RegisterInDataMessenger()
        {
            DataMessenger.Default.Register<CargoDataUpdated>(this, OnReeferInfoUpdated, "reeferinfoupdated");
        }

        #endregion

        #region Filter Logic
        // ----------- Filter logic ----------------

        #endregion

        #region AddReefer Logic

        protected override void OnAddNewUnit(object obj)
        {
            //Action
            CargoPlan.AddNewReefer(new Model.Cargo.Container()
            {
                ContainerNumber = UnitToAddNumber,
                Location = UnitToAddLocation.CorrectFormatContainerLocation()
            });

            //Scroll into the new Container
            SelectedReefer = CargoPlan.Reefers[CargoPlan.Reefers.Count - 1];
            OnPropertyChanged(nameof(SelectedReefer));
        }

        /// <summary>
        /// Actions on displaying AddDg menu (on click 'Add' button)
        /// </summary>
        /// <param name="obj"></param>
        internal void OnDisplayAddReeferMenu(object obj = null)
        {
            UnitToAddNumber = SelectedReefer?.ContainerNumber;
            UnitToAddLocation = SelectedReefer?.Location;

            MenuVisibility = System.Windows.Visibility.Visible;
            OnPropertyChanged(nameof(MenuVisibility));
        }

        #endregion


        #region Methods
        // ----- Methods -----

        /// <summary>
        /// Method changes SelectedReefer to match with the selected number (e.g. with ConflictPanelItem object)
        /// </summary>
        /// <param name="obj">Selected container number</param>
        internal void SelectReefer(string containerNumber)
        {
            SelectedReefer = null;
            OnPropertyChanged(nameof(SelectedReefer));

            //Set new selection
            foreach (ContainerWrapper container in UnitsPlanView)
            {
                if (string.Equals(container.ContainerNumber, containerNumber))
                {
                    SelectedReefer = container;
                    break;
                }
            }
            OnPropertyChanged(nameof(SelectedReefer));
        }

        private void OnDeleteReefersRequested(object obj)
        {
            if (SelectedReefer == null) return;
            var count = ((ICollection)obj).Count;

            if (_messageDialogService.ShowYesNoDialog($"Do you want to delete selected reefer" + (count > 1 ? $"s ({count})" : "") + "?", "Delete cargo")
                == MessageDialogResult.No) return;

            List<string> list = new List<string>();
            foreach (ContainerWrapper item in (ICollection)obj)
            {
                list.Add(item.ContainerNumber);
            }

            foreach (var number in list)
            {
                CargoPlan.RemoveReefer(number, toUpdateInCargoPlan: true);
            }
        }

        /// <summary>
        /// Updates reeferView update after import manifest info
        /// </summary>
        /// <param name="obj"></param>
        private void OnReeferInfoUpdated(CargoDataUpdated obj)
        {
            dispatcher.Invoke(() =>
            {
                SetDataView();
                OnPropertyChanged(nameof(UnitsPlanView));
            });
        }

        protected override void OnSelectionChanged(object obj)
        {
            SetSelectionStatusBar(obj);
            if (SelectedReefer is null) return;

            if (MenuVisibility == System.Windows.Visibility.Visible)
            {
                if (SelectedReefer.ContainerNumber != UnitToAddNumber)
                {
                    UnitToAddNumber = SelectedReefer?.ContainerNumber;
                    UnitToAddLocation = SelectedReefer?.Location;
                }
            }
            selectionObject = obj;
        }

        #endregion


        #region Commands
        //--------------- Commands ----------------------------------------
        public ICommand AddNewReeferCommand { get; private set; }
        public ICommand DisplayAddReeferMenuCommand { get; private set; }
        public ICommand DeleteReefersCommand { get; private set; }
        #endregion
    }
}
