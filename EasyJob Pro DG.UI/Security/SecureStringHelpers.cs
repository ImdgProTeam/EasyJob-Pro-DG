using System;
using System.Runtime.InteropServices;
using System.Security;

namespace EasyJob_ProDG.UI.Security
{
    public static class SecureStringHelpers
    {
        public static string Unsecure(this SecureString securePassword)
        {
            if (securePassword == null)
                return string.Empty;

            //Get a pointer for an unsecure string in memory
            var unmanagedString = IntPtr.Zero;

            try
            {
                //unsecure and return password
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                //Clean up memory allocation
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}
