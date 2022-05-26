using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using EasyJob_ProDG.UI.Messages;
using EasyJob_ProDG.UI.Services;
using EasyJob_ProDG.UI.Services.DialogServices;
using EasyJob_ProDG.UI.Settings;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.Wrapper;

namespace EasyJob_ProDG.UI.ViewModel
{
    public class DataGridReefersViewModel : Observable
    {
        //--------------- Private fields --------------------------------------------
        SettingsService uiSettings;
        IMessageDialogService _messageDialogService;
        private readonly CollectionViewSource reeferPlanView = new CollectionViewSource();

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
        public List<ContainerWrapper> SelectedReeferArray { get; set; }
        public UserUISettings.DgSortOrderPattern ReeferSortOrderDirection { get; set; }


        // ---------- Constructor ---------------
        public DataGridReefersViewModel()
        {
            SetDataView();
            RegisterInDataMessenger();
            reeferPlanView.Filter += OnReeferListFiltered;
        }

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

        /// <summary>
        /// Sets data source to View property
        /// </summary>
        private void SetDataView()
        {
            reeferPlanView.Source = CargoPlan.Reefers;
        }

        /// <summary>
        /// Invokes OnPropertyChanged method for relevant properties.
        /// </summary>
        /// <param name="obj">none</param>
        private void OnCargoDataUpdated(CargoDataUpdated obj)
        {
            SetDataView();
            OnPropertyChanged($"CargoPlan");
            OnPropertyChanged("ReeferPlanView");
            ReeferPlanView.Refresh();
        }

        private void OnReeferInfoUpdated(CargoDataUpdated obj)
        {
            reeferPlanView.Source = null;
            SetDataView();
            OnPropertyChanged("ReeferPlanView");
        }
        /// <summary>
        /// Registers for messages in DataMessenger
        /// </summary>
        private void RegisterInDataMessenger()
        {
            DataMessenger.Default.Register<CargoDataUpdated>(this, OnCargoDataUpdated, "cargodataupdated");
            DataMessenger.Default.Register<CargoDataUpdated>(this, OnReeferInfoUpdated, "reeferinfoupdated");
        }

    }
}
