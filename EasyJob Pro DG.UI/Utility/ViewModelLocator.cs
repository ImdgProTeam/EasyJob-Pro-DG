using EasyJob_ProDG.UI.ViewModel;

namespace EasyJob_ProDG.UI.Utility
{
    public class ViewModelLocator
    {
        private static MainWindowViewModel mainWindowViewModel = new MainWindowViewModel();
        public static MainWindowViewModel MainWindowViewModel
        {
            get
            {
                return mainWindowViewModel;
            }
        }

        private static ConflictListViewModel conflictListViewModel = new ConflictListViewModel();
        public static ConflictListViewModel ConflictListViewModel => conflictListViewModel;

        private static DataGridDgViewModel dataGridDgViewModel = new DataGridDgViewModel();
        public static DataGridDgViewModel DataGridDgViewModel 
        {
            get
            {
                return dataGridDgViewModel;
            }
        }

        private static DataGridContainersViewModel dataGridContainersViewModel = new DataGridContainersViewModel();
        public static DataGridContainersViewModel DataGridContainersViewModel
        {
            get
            {
                return dataGridContainersViewModel;
            }
        }

        private static DataGridReefesViewModel dataGridReefersViewModel = new DataGridReefesViewModel();
        public static DataGridReefesViewModel DataGridReefersViewModel
        {
            get
            {
                return dataGridReefersViewModel;
            }
        }

        private static ShipProfileWindowVM shipProfileWindowVM = new ShipProfileWindowVM();
        public static ShipProfileWindowVM ShipProfileWindowVM
        {
            get
            {
                return shipProfileWindowVM;
            }
        }

        private static SettingsWindowVM settingsWindowVM = new SettingsWindowVM();
        public static SettingsWindowVM SettingsWindowVM
        {
            get
            {
                return settingsWindowVM;
            }
        }
    }
}
