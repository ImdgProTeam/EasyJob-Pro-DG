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
            RegistryKey FileReg = Registry.CurrentUser.CreateSubKey("software\\Classes\\" + extension);

            FileReg.CreateSubKey("shell\\open\\command").SetValue("", $"{applicationPath}%1");
            FileReg.Close();

            SHChangeNotify(0x08000000, 0x000, IntPtr.Zero, IntPtr.Zero);
        }

        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void SHChangeNotify(uint wEventId, uint uFlags, IntPtr dwItem1, IntPtr dwItem2);
    }
}
