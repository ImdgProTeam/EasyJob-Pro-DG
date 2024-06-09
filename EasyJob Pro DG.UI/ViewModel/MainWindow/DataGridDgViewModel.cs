using EasyJob_ProDG.UI.Data;
using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Services.DialogServices;
using EasyJob_ProDG.UI.Settings;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.ViewModel.MainWindow;
using EasyJob_ProDG.UI.Wrapper;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.ViewModel
{
    public class DataGridDgViewModel : DataGridViewModelBase
    {
        //--------------- Public static properties ----------------------------------
        public static IList<char> StowageCategories => new List<char>() { 'A', 'B', 'C', 'D', 'E' };

        //--------------- Public properties -----------------------------------------
        public ObservableCollection<DgTableColumnSettings> ColumnSettings { get; set; }
        public DgWrapper SelectedDg { get; set; }
        public bool IsTechnicalNameIncluded { get; set; }


        #region Constructor
        //--------------- Constructor -----------------------------------------------
        public DataGridDgViewModel() : base()
        {
            LoadServices();
        }
        #endregion

        #region StartUp Logic

        /// <summary>
        /// Registers for messages in DataMessenger
        /// </summary>
        protected override void RegisterInDataMessenger()
        {
            DataMessenger.Default.Register<DgListSelectedItemUpdatedMessage>(this, OnCargoPlanSelectedItemUpdatedMessage, "selectionpropertyupdated");
        }

        /// <summary>
        /// Assigns handler methods for commands
        /// </summary>
        protected override void LoadCommands()
        {
            AddDgCommand = new DelegateCommand(OnAddNewUnit);
            DeleteDg = new DelegateCommand(OnDgDeleteRequested);
            IncludeTechnicalNameCommand = new DelegateCommand(IncludeTechnicalNameOnExecuted);
            DisplayAddDgMenuCommand = new DelegateCommand(OnDisplayAddDgMenu);
        }

        /// <summary>
        /// Sets data source to View property
        /// </summary>
        protected override void SetDataView()
        {
            SetPlanViewSource(WorkingCargoPlan.DgList);
        }

        #endregion

        #region Filter Logic
        // ----------- Filter logic ----------------

        /// <summary>
        /// Implements logic to filter content
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnUnitsListFiltered(object sender, FilterEventArgs e)
        {
            // Checks section

            if (string.IsNullOrEmpty(textToFilter)) return;

            if (!(e.Item is DgWrapper dg) || dg.ContainerNumber is null)
            {
                e.Accepted = false;
                return;
            }

            //Logic section

            var searchText = textToFilter.ToLower().Replace(" ", "");

            if (dg.ContainerNumber.ToLower().Contains(searchText)) return;
            if (dg.Unno.ToString().Contains(searchText)) return;
            if (dg.Location.Replace(" ", "").Contains(searchText)) return;

            e.Accepted = false;
        }

        #endregion

        #region AddDg Logic
        public override bool CanUserAddUnit => !string.IsNullOrEmpty(UnitToAddNumber) && UnitToAddUnno > 0;

        ushort unitToAddUnno;
        public ushort UnitToAddUnno
        {
            get => unitToAddUnno;
            set
            {
                unitToAddUnno = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanUserAddUnit));
            }
        }

        protected override void OnAddNewUnit(object obj)
        {
            //Correct location
            string location = unitToAddLocation.CorrectFormatContainerLocation();

            //Existing unno
            if (!DataHelper.CheckForExistingUnno(unitToAddUnno))
                return;

            //Action
            WorkingCargoPlan.AddDg(new Model.Cargo.Dg()
            {
                Unno = unitToAddUnno,
                ContainerNumber = unitToAddNumber,
                Location = location
            }); ;

            //Scroll into the new Container
            SelectedDg = WorkingCargoPlan.DgList[WorkingCargoPlan.DgList.Count - 1];
            OnPropertyChanged(nameof(SelectedDg));
        }

        /// <summary>
        /// Actions on displaying AddDg menu (on click 'Add' button)
        /// </summary>
        /// <param name="obj"></param>
        internal void OnDisplayAddDgMenu(object obj = null)
        {
            var container = (Model.Cargo.Container)obj;

            UnitToAddNumber = container == null ? SelectedDg?.ContainerNumber : container.ContainerNumber;
            UnitToAddLocation = container == null ? SelectedDg?.Location : container.Location;

            MenuVisibility = Visibility.Visible;
            OnPropertyChanged(nameof(MenuVisibility));
        }

        #endregion

        #region Public methods
        //--------------- Public methods -------------------------------------------

        /// <summary>
        /// Method changes SelectedDg to match with the selected DgID (e.g. with ConflictPanelItem object)
        /// </summary>
        /// <param name="obj">Selected dg id</param>
        internal void SelectDg(int id)
        {
            SelectedDg = null;
            OnPropertyChanged(nameof(SelectedDg));

            //Set new selection
            foreach (DgWrapper dg in UnitsPlanView)
            {
                if (dg.Model.ID == id)
                {
                    SelectedDg = dg;
                    break;
                }
            }
            OnPropertyChanged(nameof(SelectedDg));
        }

        #endregion

        #region Private methods
        //--------------- Private methods -------------------------------------------

        /// <summary>
        /// Includes or removes TechnicalName to ProperShippingName of all Dg in WorkingCargoPlan
        /// </summary>
        /// <param name="obj"></param>
        private void IncludeTechnicalNameOnExecuted(object obj)
        {
            if (IsTechnicalNameIncluded)
            {
                foreach (var dg in WorkingCargoPlan.DgList)
                {
                    dg.RemoveTechnicalName();
                }

                IsTechnicalNameIncluded = false;
            }
            else
            {
                foreach (var dg in WorkingCargoPlan.DgList)
                {
                    dg.IncludeTechnicalName();
                }

                IsTechnicalNameIncluded = true;
            }
            OnPropertyChanged(nameof(IsTechnicalNameIncluded));
        }

        /// <summary>
        /// Requests user weather to delete selected dg(s) and sends message to WorkingCargoPlan respectively
        /// </summary>
        /// <param name="obj"></param>
        private void OnDgDeleteRequested(object obj)
        {
            if (SelectedDg == null) return;

            if (_messageDialogService.ShowYesNoDialog($"Do you want to delete selected Dg(s) ({((ICollection)obj).Count})?", "Delete cargo")
                == MessageDialogResult.No) return;

            List<DgWrapper> selectedDgArray = new List<DgWrapper>();
            foreach (var data in (ICollection)obj)
            {
                var dg = data as DgWrapper;
                selectedDgArray.Add(dg);
            }

            DataMessenger.Default.Send<UpdateCargoPlan>(new UpdateCargoPlan(selectedDgArray), "Remove dg");
        }

        protected override void OnSelectionChanged(object obj)
        {
            SetSelectionStatusBar(obj);

            if (SelectedDg is null) return;

            if (MenuVisibility == Visibility.Visible)
            {
                if (SelectedDg.ContainerNumber != UnitToAddNumber)
                {
                    UnitToAddNumber = SelectedDg?.ContainerNumber;
                    UnitToAddLocation = SelectedDg?.Location;
                }
            }
            selectionObject = obj;
        }

        protected override void SetSelectionStatusBar(object obj)
        {
            StatusBarText = SelectionStatusBarSetter.GetSelectionStatusBarTextForDg(obj);
            OnPropertyChanged(nameof(StatusBarText));
        }

        // ----- Methods called by received messages

        /// <summary>
        /// Called to update <see cref="StatusBarText"/> when Dg property changes
        /// </summary>
        /// <param name="message"></param>
        private void OnCargoPlanSelectedItemUpdatedMessage(DgListSelectedItemUpdatedMessage message)
        {
            SetSelectionStatusBar(selectionObject);
        }

        #endregion

        #region Commands
        //--------------- Commands --------------------------------------------------

        public ICommand IncludeTechnicalNameCommand { get; set; }
        public ICommand ToExcel { get; private set; }
        public ICommand DeleteDg { get; private set; }
        public ICommand AddDgCommand { get; private set; }
        public ICommand DisplayAddDgMenuCommand { get; private set; }

        #endregion
    }



}
