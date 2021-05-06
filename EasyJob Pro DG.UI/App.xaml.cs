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
            Debug.WriteLine("------> Start Application_Startup");
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

#if(DEBUG)
            Debug.WriteLine("------> Checking AssemblyVersion");
            CheckAssemblyVersion();
            Debug.WriteLine("------> Assembly version checked");
#endif

            if (e.Args.Length > 0)
            {
                path = e.Args[0];
            }
            Debug.WriteLine($"------> Start file path = {path}");

            ApplicationMainWindow = new MainWindow(path);
            Debug.WriteLine($"------> Application MainWindow created");
            path = null;

            ApplicationMainWindow.Show();
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
