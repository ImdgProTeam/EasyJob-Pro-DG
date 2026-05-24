using EasyJob_ProDG.UI.Data;
using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Services.DialogServices;
using EasyJob_ProDG.UI.Settings;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.Wrapper;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace EasyJob_ProDG.UI.ViewModel
{
    public class DataGridDgViewModel : DataGridViewModelBase
    {

        //--------------- Public static properties ----------------------------------
        public static IList<string> StowageCategories => new List<string>() { "", "A", "B", "C", "D", "E", "01", "02", "03", "04", "05" };

        //--------------- Public properties -----------------------------------------
        public ObservableCollection<DgTableColumnSettings> ColumnSettings { get; set; }
        public DgWrapper SelectedDg { get; set; }

        public bool IsTechnicalNameIncluded { get; set; }

        /// <summary>
        /// Property used to bound ContextMenu items and checkboxes.
        /// </summary>
        public DataGridDgContextMenuViewModel ContextMenuViewModel { get; }

        #region Constructor
        //--------------- Constructor -----------------------------------------------
        public DataGridDgViewModel() : base()
        {
            RefreshControls();
            ContextMenuViewModel = new DataGridDgContextMenuViewModel();
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
            AppendProperShippingNameCommand = new DelegateCommand(AppendProperShippingName);
            RestoreProperShippingNameFromIMDGCodeCommand = new DelegateCommand(RestoreProperShippingNameFromIMDGCodeOnExecuted);
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
            if (isPOLIncluded && dg.POL.Replace(" ", "").ToLower().Contains(searchText)) return;
            if (isPODIncluded && dg.POD.Replace(" ", "").ToLower().Contains(searchText)) return;

            e.Accepted = false;
        }

        protected override void OnAdvanceFiltered(object sender, FilterEventArgs e)
        {
            if (filteredContainerNumbers == null) return;

            if (!(e.Item is DgWrapper dg))
            {
                e.Accepted = false;
                return;
            }
            if (filteredContainerNumbers.Contains(dg.Model.ID.ToString())) return;

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
            WorkingCargoPlan.AddDg(new DgWrapper(new Model.Cargo.Dg()
            {
                Unno = unitToAddUnno,
                ContainerNumber = unitToAddNumber,
                Location = location
            }));

            //Recheck dg list
            DataMessenger.Default.Send(new ConflictsToBeCheckedAndUpdatedMessage());

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

        internal void RefreshControls()
        {
            CheckSetTechnicalNameIncluded();
        }

        #endregion

        #region Private methods
        //--------------- Private methods -------------------------------------------

        /// <summary>
        /// Restores Proper Shipping Name as it is stated in IMDG Code
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void RestoreProperShippingNameFromIMDGCodeOnExecuted(object obj)
        {
            if (string.Equals((string)obj, "All"))
            {
                foreach (var dg in WorkingCargoPlan.DgList)
                {
                    dg.RestoreProperShippingName();
                }
                IsTechnicalNameIncluded = false;
            }
            if (string.Equals((string)obj, "Selection"))
            {
                if (selectionObject == null) return;

                foreach (DgWrapper dg in GetSelectionObjectList())
                {
                    if (dg is null) continue;
                    dg.RestoreProperShippingName();
                }
            }
            OnPropertyChanged(nameof(IsTechnicalNameIncluded));
            OnPropertyChanged(nameof(ContextMenuViewModel.IsTechnicalNameOfSelectedDgIncluded));
        }

        /// <summary>
        /// Includes or removes TechnicalName to ProperShippingName of all Dg in WorkingCargoPlan
        /// </summary>
        /// <param name="obj"></param>
        private void IncludeTechnicalNameOnExecuted(object obj)
        {
            if (string.Equals((string)obj, "Selection"))
            {
                if (selectionObject == null) return;

                if (SelectedDg.IsTechnicalNameIncluded)
                {
                    foreach (DgWrapper dg in GetSelectionObjectList())
                    {
                        dg.RemoveTechnicalName();
                    }
                }
                else
                {
                    foreach (DgWrapper dg in GetSelectionObjectList())
                    {
                        dg.IncludeTechnicalName();
                    }
                }
                CheckSetTechnicalNameIncluded();
            }
            // Case for all units
            else if (IsTechnicalNameIncluded)
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
        /// Assisting method to define if any <see cref="DgWrapper"/> has <see cref="DgWrapper.IsTechnicalNameIncluded"/> and sets respectively <see cref="IsTechnicalNameIncluded"/> property.
        /// </summary>
        private void CheckSetTechnicalNameIncluded()
        {
            if (WorkingCargoPlan.DgList.Any(dg => dg.IsTechnicalNameIncluded))
            {
                IsTechnicalNameIncluded = true;
            }
            else
            {
                IsTechnicalNameIncluded = false;
            }
            OnPropertyChanged(nameof(IsTechnicalNameIncluded));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void AppendProperShippingName(object obj)
        {
            ContextMenuViewModel.AppendProperShippingName(obj);
        }

        /// <summary>"RESIDUE LAST CONTAINED" />
        /// Requests "MOLTEN" />user weather to delete selected dg(s) and sends message to WorkingCargoPlan respectively
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

        // ------ Override methods from base abstract class -----
        protected override void OnSelectionChanged(object obj)
        {
            if(_isRefreshing) return;
            base.OnSelectionChanged(obj);

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

            ContextMenuViewModel.SetSelectedDg(SelectedDg);
            ContextMenuViewModel.RefreshAllIsChecked();
        }

        protected override void PostCargoDataUpdated()
        {
            SelectedDg = null;
            OnPropertyChanged(nameof(SelectedDg));
        }
        protected override void SetSelectionStatusBar(object obj)
        {
            StatusBarText = SelectionStatusBarTextGenerator.GetSelectionStatusBarTextForDg(obj);
            OnPropertyChanged(nameof(StatusBarText));
        }
        protected override void SetStatusBarOnFilter()
        {
            int itemsCount = ((CollectionView)UnitsPlanView).Count;
            int unitsCount = ((CollectionView)UnitsPlanView).Cast<DgWrapper>().Select(item => item.ContainerNumber).Distinct().Count();


            StatusBarText = $"Filtered: {itemsCount} dg items in {unitsCount} containers.";
            OnPropertyChanged(nameof(StatusBarText));
        }

        internal override void RefreshStatusBar()
        {
            StatusBarText = SelectionStatusBarTextGenerator.GetSelectionStatusBarTextForDg(GetSelectionObjectList());
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

        public ICommand RestoreProperShippingNameFromIMDGCodeCommand { get; set; }
        public ICommand IncludeTechnicalNameCommand { get; set; }
        public ICommand ToExcel { get; private set; }
        public ICommand DeleteDg { get; private set; }
        public ICommand AddDgCommand { get; private set; }
        public ICommand DisplayAddDgMenuCommand { get; private set; }
        public ICommand AppendProperShippingNameCommand { get; private set; }

        #endregion
    }



}
