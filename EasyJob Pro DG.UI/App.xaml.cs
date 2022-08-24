using EasyJob_ProDG.Data;
using EasyJob_ProDG.UI.View.UI;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Windows;

namespace EasyJob_ProDG.UI
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public MainWindow ApplicationMainWindow;
        private string path;

        public void Application_Startup(object sender, StartupEventArgs e)
        {
            LogWriter.StartLog();
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            #region Checking Assembly Version
#if (DEBUG)
            Debug.WriteLine("------> Checking AssemblyVersion");
            CheckAssemblyVersion();
            Debug.WriteLine("------> Assembly version checked");
#endif 
            #endregion

            CheckLicence();

            FirstTimeStart();

            if (e.Args.Length > 0)
            {
                path = e.Args[0];
            }

            LogWriter.Write($"Start file path = {path}");

            ApplicationMainWindow = new MainWindow(path);
            LogWriter.Write($"Application MainWindow created");
            path = null;

            ApplicationMainWindow.Show();


        }

        /// <summary>
        /// Actions required when the program is started for the very first time.
        /// </summary>
        private void FirstTimeStart()
        {
            //UI.Properties.Settings.Default.FirstTimeStart = true;
            if (EasyJob_ProDG.UI.Properties.Settings.Default.FirstTimeStart)
            {
                Services.FirstStartService.DoFirstStart();
                EasyJob_ProDG.UI.Properties.Settings.Default.FirstTimeStart = false;
            }
        }

        /// <summary>
        /// Checking the licence is valid
        /// </summary>
        private void CheckLicence()
        {
            LogWriter.Write($"Checking licence");

            if (!Licence.IsValid())
            {
                LogWriter.Write("Licence expired.");

                MessageBox.Show("Your licence has expired.\nPlease contact\n\nfeedback@imdg.pro\n\nto renew your licence.", "Invalid licence");
                Environment.Exit(0);
            }
            LogWriter.Write("-----> Licence is valid.");
        }

        /// <summary>
        /// Stops the App if Data project version does not match UI project version
        /// </summary>
        private void CheckAssemblyVersion()
        {
            string dataVersion = ProgramDefaultSettingValues.ReleaseVersion;
            var ver = typeof(App).Assembly.GetName().Version ?? throw new ArgumentNullException("thisAssemName.Version"); ;

            if (string.Equals(ver.ToString(), dataVersion)) return;

            Debug.WriteLine($"------> Assembly version check failed");
            MessageBox.Show("Version of Data project does not match UI project version.\nApplication will be stopped.",
                "Attention!", MessageBoxButton.OK);

            this.Shutdown();
        }
    }
}
