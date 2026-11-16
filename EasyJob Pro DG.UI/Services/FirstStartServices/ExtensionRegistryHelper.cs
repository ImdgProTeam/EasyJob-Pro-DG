using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;

namespace EasyJob_ProDG.UI.Services
{
    class ExtensionRegistryHelper
    {
        /// <summary>
        /// Method registers file extension in Windows register
        /// </summary>
        /// <param name="extension"></param>
        /// <param name="progID"></param>
        public static void SetFileAssociation(string extension, string applicationPath)
        {
            //RegistryKey FileReg = Registry.CurrentUser.CreateSubKey("software\\Classes\\" + extension);

            //FileReg.CreateSubKey("shell\\open\\command").SetValue("", $"{applicationPath}%1");
            //FileReg.Close();

            // 3. Notify Windows of the association change (SHCNE_ASSOCCHANGED = 0x08000000)
            SHChangeNotify(0x08000000, 0x000, IntPtr.Zero, IntPtr.Zero);
        }

        // Tells Windows Explorer to refresh file icons immediately
        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);

        /// <summary>
        /// Method registers file extension in Windows register
        /// </summary>
        /// <param name="extension">Extension to be registred</param>
        /// <param name="progId">Program ID for registry</param>
        /// <param name="description">Free file description</param>
        /// <param name="exePath">Program execution path</param>
        public static void Register(string extension, string progId, string description, string exePath)
        {
            // 1. Associate the extension with your ProgID
            using (RegistryKey extKey = Registry.CurrentUser.CreateSubKey($@"Software\Classes\{extension}"))
            {
                extKey.SetValue("", progId);
            }

            // 2. Define the ProgID, description, icon, and launch command
            using (RegistryKey progIdKey = Registry.CurrentUser.CreateSubKey($@"Software\Classes\{progId}"))
            {
                progIdKey.SetValue("", description);

                using (RegistryKey iconKey = progIdKey.CreateSubKey("DefaultIcon"))
                {
                    // Uses the main icon embedded in your .exe
                    iconKey.SetValue("", $"\"{exePath}\",0");
                }

                using (RegistryKey commandKey = progIdKey.CreateSubKey(@"shell\open\command"))
                {
                    // %1 passes the clicked file's full path as a command-line argument
                    commandKey.SetValue("", $"\"{exePath}\" \"%1\"");
                }
            }
        }

    }
}
