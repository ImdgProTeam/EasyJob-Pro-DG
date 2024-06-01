using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Services.DialogServices;
using EasyJob_ProDG.UI.Settings;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.ViewModel.MainWindow;
using EasyJob_ProDG.UI.Wrapper;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace EasyJob_ProDG.UI.ViewModel
{
    public class DataGridReefersViewModel : Observable
    {
        //--------------- Private fields --------------------------------------------
        IMessageDialogService _messageDialogService => MessageDialogService.Connect();

        private readonly CollectionViewSource reeferPlanView = new CollectionViewSource();
        Dispatcher dispatecher;

        //--------------- Public properties -----------------------------------------
        public CargoPlanWrapper CargoPlan
        {
            get { return ViewModelLocator.MainWindowViewModel.WorkingCargoPlan; }
            set
            {
            }
        }
        /// <summary>
        /// Used for ReeferDataGrid binding 
        /// </summary>
        public ICollectionView ReeferPlanView => reeferPlanView?.View;
        public ContainerWrapper SelectedReefer { get; set; }
        private object _selectionObject;

        public string StatusBarText { get; private set; } = "None";
        public UserUISettings.DgSortOrderPattern ReeferSortOrderDirection { get; set; }


        // ---------- Constructor ---------------
        public DataGridReefersViewModel()
        {
            dispatecher = Dispatcher.CurrentDispatcher;

            SetDataView();
            RegisterInDataMessenger();
            LoadCommands();
            SetVisualElements();

            reeferPlanView.Filter += OnReeferListFiltered;
        }

        #region StartUp logic

        /// <summary>
        /// Sets data source to View property
        /// </summary>
        private void SetDataView()
        {
            reeferPlanView.Source = CargoPlan.Reefers;
        }

        /// <summary>
        /// Sets required properties values of various visual elements
        /// </summary>
        private void SetVisualElements()
        {
            SetInitialAddMenuProperties();
        }

        /// <summary>
        /// Assigns handler methods for commands
        /// </summary>
        private void LoadCommands()
        {
            AddNewReeferCommand = new DelegateCommand(OnAddNewReefer);
            DisplayAddReeferMenuCommand = new DelegateCommand(OnDisplayAddReeferMenu);
            SelectionChangedCommand = new DelegateCommand(OnSelectionChanged);
            DeleteReefersCommand = new DelegateCommand(OnDeleteReefersRequested);
        }

        /// <summary>
        /// Subscribe for messages in DataMessenger
        /// </summary>
        private void RegisterInDataMessenger()
        {
            DataMessenger.Default.Register<CargoDataUpdated>(this, OnCargoDataUpdated, "cargodataupdated");
            DataMessenger.Default.Register<CargoDataUpdated>(this, OnReeferInfoUpdated, "reeferinfoupdated");
            DataMessenger.Default.Register<CargoPlanUnitPropertyChanged>(this, OnCargoPlanUnitPropertyChanged);

        }

        private void OnCargoPlanUnitPropertyChanged(CargoPlanUnitPropertyChanged changed)
        {
            SetSelectionStatusBar(_selectionObject);
        }

        private void SetSelectionStatusBar(object obj)
        {
            StatusBarText = SelectionStatusBarSetter.GetSelectionStatusBarTextForContainer(obj);
            OnPropertyChanged(nameof(StatusBarText));
        }

        #endregion

        #region Filter Logic
        // ----------- Filter logic ----------------
        private string textToFilter;

        public string TextToFilter
        {
            get { return textToFilter; }
            set
            {
                if (textToFilter == value) return;
                textToFilter = value;
                ReeferPlanView.Refresh();
            }
        }

        /// <summary>
        /// Implements logic to filter content
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnReeferListFiltered(object sender, FilterEventArgs e)
        {
            // Checks section

            if (string.IsNullOrEmpty(textToFilter)) return;

            if (!(e.Item is ContainerWrapper c) || c.ContainerNumber is null)
            {
                e.Accepted = false;
                return;
            }

            //Logic section

            var searchText = textToFilter.ToLower().Replace(" ", "");

            if (c.ContainerNumber.ToLower().Contains(searchText)) return;
            if (c.Location.Replace(" ", "").Contains(searchText)) return;

            e.Accepted = false;
        }
        #endregion

        #region AddReefer Logic
        public bool CanUserAddReefer => !string.IsNullOrEmpty(ReeferToAddNumber);

        string reeferToAddNumber;
        public string ReeferToAddNumber
        {
            get => reeferToAddNumber;
            set
            {
                reeferToAddNumber = value?.Trim();
                OnPropertyChanged();
                OnPropertyChanged(nameof(CanUserAddReefer));
            }
        }

        string reeferToAddLocation;
        public string ReeferToAddLocation
        {
            get => reeferToAddLocation;
            set
            {
                reeferToAddLocation = value.LimitMaxContainerLocationInput();
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Used to set visibility of AddMenu
        /// </summary>
        public System.Windows.Visibility MenuVisibility { get; set; }

        private void OnAddNewReefer(object obj)
        {
            //Correct location
            string location = reeferToAddLocation.CorrectFormatContainerLocation();

            //Action
            CargoPlan.AddNewReefer(new Model.Cargo.Container()
            {
                ContainerNumber = reeferToAddNumber,
                Location = location
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
            ReeferToAddNumber = SelectedReefer?.ContainerNumber;
            ReeferToAddLocation = SelectedReefer?.Location;

            MenuVisibility = System.Windows.Visibility.Visible;
            OnPropertyChanged(nameof(MenuVisibility));
        }

        /// <summary>
        /// Sets initial view properties of AddDg menu
        /// </summary>
        private void SetInitialAddMenuProperties()
        {
            MenuVisibility = System.Windows.Visibility.Collapsed;
        }

        #endregion

        /// <summary>
        /// Method changes SelectedReefer to match with the selected number (e.g. with ConflictPanelItem object)
        /// </summary>
        /// <param name="obj">Selected container number</param>
        internal void SelectReefer(string containerNumber)
        {
            SelectedReefer = null;
            OnPropertyChanged(nameof(SelectedReefer));

            //Set new selection
            foreach (ContainerWrapper container in ReeferPlanView)
            {
                if (string.Equals(container.ContainerNumber, containerNumber))
                {
                    SelectedReefer = container;
                    break;
                }
            }
            OnPropertyChanged(nameof(SelectedReefer));
        }

        #region Private methods
        //-------------- Private methods --------------------------------------------

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
        /// Invokes OnPropertyChanged method for relevant properties.
        /// </summary>
        /// <param name="obj">none</param>
        private void OnCargoDataUpdated(CargoDataUpdated obj)
        {
            dispatecher.Invoke(() =>
            {
                SetDataView();
                OnPropertyChanged(nameof(CargoPlan));
                OnPropertyChanged(nameof(ReeferPlanView));
                ReeferPlanView.Refresh();
            });
        }

        /// <summary>
        /// Updates reeferView update after import manifest info
        /// </summary>
        /// <param name="obj"></param>
        private void OnReeferInfoUpdated(CargoDataUpdated obj)
        {
            dispatecher.Invoke(() =>
            {
                SetDataView();
                OnPropertyChanged(nameof(ReeferPlanView));
                ReeferPlanView.Refresh();
            });
        }

        private void OnSelectionChanged(object obj)
        {
            SetSelectionStatusBar(obj);
            if (SelectedReefer is null) return;

            if (MenuVisibility == System.Windows.Visibility.Visible)
            {
                if (SelectedReefer.ContainerNumber != reeferToAddNumber)
                {
                    ReeferToAddNumber = SelectedReefer?.ContainerNumber;
                    ReeferToAddLocation = SelectedReefer?.Location;
                }
            }
            _selectionObject = obj;
        }
        #endregion


        #region Commands
        //--------------- Commands ----------------------------------------
        public ICommand SelectionChangedCommand { get; private set; }
        public ICommand AddNewReeferCommand { get; private set; }
        public ICommand DisplayAddReeferMenuCommand { get; private set; }
        public ICommand DeleteReefersCommand { get; private set; }
        #endregion
    }
}
