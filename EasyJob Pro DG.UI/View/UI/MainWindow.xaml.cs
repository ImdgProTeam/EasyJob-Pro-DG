using EasyJob_ProDG.UI.ViewModel;
using System.ComponentModel;
using System.Windows;

namespace EasyJob_ProDG.UI.View.UI
{
    public partial class MainWindow
    {
        /// <summary>
        /// Cargo condition file path to be used when the program started.
        /// </summary>
        public string StartupFilePath;


        public MainWindow(string path = null)
        {
            StartupFilePath = path;

            InitializeComponent();

            SetWindowLocationOnStartup();
            RestoreConflictColumnWidth();
        }

        #region Window location
        /// <summary>
        /// Sets Window size and location from settings. 
        /// </summary>
        private void SetWindowLocationOnStartup()
        {
            try
            {
                if (Properties.Settings.Default.WindowStateMaximized)
                {
                    WindowState = WindowState.Maximized;
                }
                else
                {
                    Left = Properties.Settings.Default.WindowPosition.Left;
                    Top = Properties.Settings.Default.WindowPosition.Top;
                    Width = Properties.Settings.Default.WindowPosition.Width;
                    Height = Properties.Settings.Default.WindowPosition.Height;
                }
            }
            catch
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
        }

        /// <summary>
        /// Saves window location and state to settings.
        /// </summary>
        private void SaveCurrentWindowLocationToSettings()
        {
            Properties.Settings.Default.WindowStateMaximized = this.WindowState == WindowState.Maximized;
            Properties.Settings.Default.WindowPosition = this.RestoreBounds;
            Properties.Settings.Default.Save();
        }
        #endregion

        #region DesignMatters
        private void SaveConflictColumnWidth()
        {
            Properties.Settings.Default.ConflictsWidth = WorkingGrid.ColumnDefinitions[1].ActualWidth;
        }

        private void RestoreConflictColumnWidth()
        {
            WorkingGrid.ColumnDefinitions[1].Width=new GridLength( Properties.Settings.Default.ConflictsWidth);
        }
        #endregion

        #region Window event handlers
        private void ClosingApplication(object sender, CancelEventArgs e)
        {
            OnWindowClosingEventHandler.Invoke();

            SaveConflictColumnWidth();
            SaveCurrentWindowLocationToSettings();
        }


        // ----- Logic to dim the MainWindow if not focused -----
        private void Window_Activated(object sender, System.EventArgs e)
        {
            (DataContext as MainWindowViewModel).IsDimmedOverlayVisible = false;
        }

        private void Window_Deactivated(object sender, System.EventArgs e)
        {
            (DataContext as MainWindowViewModel).IsDimmedOverlayVisible = true;
        } 

        #endregion


        // --------- Events -----------------------------------------------
        public delegate void WindowClosing();
        public static event WindowClosing OnWindowClosingEventHandler = null;

    }
}
