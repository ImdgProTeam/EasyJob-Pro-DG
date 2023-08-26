using Microsoft.Win32;
using System.IO;
using System.Text;

namespace EasyJob_ProDG.UI.Services
{
    class ExtensionRegistryHelper
    {
        /// <summary>
        /// Method registers file extension in Windows register
        /// </summary>
        /// <param name="extension"></param>
        /// <param name="progID"></param>
        public static void SetFileAssociation(string extension, string progID)
        {
            SetValue(Registry.ClassesRoot, extension, progID);

            string assemblyFullPath = System.Reflection.Assembly.GetExecutingAssembly().Location.Replace("/", @"\");

            StringBuilder sbShellEntry = new StringBuilder();
            sbShellEntry.AppendFormat("\"{0}\" \"%1\"", assemblyFullPath);

            SetValue(Registry.ClassesRoot, progID + @"\shell\open\command", sbShellEntry.ToString());

            StringBuilder sbDefaultIconEntry = new StringBuilder();
            sbDefaultIconEntry.AppendFormat("\"{0}\",0", assemblyFullPath);
            SetValue(Registry.ClassesRoot, progID + @"\DefaultIcon", sbDefaultIconEntry.ToString());

            SetValue(Registry.ClassesRoot, @"Applications\" + Path.GetFileName(assemblyFullPath), "", "NoOpenWith");
        }

        private static void SetValue(RegistryKey root, string subKey, string keyValue, string valueName = null)
        {
            bool hasSubKey = !string.IsNullOrEmpty(subKey);
            RegistryKey key = root;

            try
            {
                if (hasSubKey) key = root.CreateSubKey(subKey);
                key.SetValue(valueName, keyValue);
            }
            finally
            {
                if (hasSubKey) key?.Close();
            }
        }
    }
}
