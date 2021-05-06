using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyJob_ProDG.UI.Services;
using EasyJob_ProDG.UI.Services.DialogServices;
using EasyJob_ProDG.UI.Settings;
using EasyJob_ProDG.UI.Utility;
using EasyJob_ProDG.UI.Wrapper;

namespace EasyJob_ProDG.UI.ViewModel
{
    public class DataGridContainersViewModel : Observable
    {
        //--------------- Private fields --------------------------------------------
        SettingsService uiSettings;
        IMessageDialogService _messageDialogService;

        //--------------- Public properties -----------------------------------------
        public CargoPlanWrapper CargoPlan
        {
            get { return ViewModelLocator.MainWindowViewModel.WorkingCargoPlan; }
            set
            {
                ViewModelLocator.MainWindowViewModel.WorkingCargoPlan = value;
                OnPropertyChanged();
            }
        }

        public ContainerWrapper SelectedContainer { get; set; }
        public List<ContainerWrapper> SelectedContainerArray { get; set; }
        public UserUISettings.DgSortOrderPattern ContainerSortOrderDirection { get; set; }
    }
}
