using EasyJob_ProDG.UI.ViewModel;

namespace EasyJob_ProDG.UI.Utility
{
    public class ViewModelLocator
    {
        // Creation on the application start

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

        private static DataGridReefersViewModel dataGridReefersViewModel = new DataGridReefersViewModel();
        public static DataGridReefersViewModel DataGridReefersViewModel
        {
            get
            {
                return dataGridReefersViewModel;
            }
        }


        // Creation on demand

        private static SettingsWindowVM settingsWindowVM;
        public static SettingsWindowVM SettingsWindowVM
        {
            get
            {
                if (settingsWindowVM is null)
                    settingsWindowVM = new SettingsWindowVM();
                return settingsWindowVM;
            }
        }
    }
}
