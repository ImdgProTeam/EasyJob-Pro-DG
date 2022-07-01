using System;


namespace EasyJob_ProDG.Data
{
    public static class Licence
    {
        private static readonly DateTime Endlicence = new DateTime(2022, 12, 31, 23, 59, 59);

        public static DateTime EndLicence => Endlicence;

        public static bool IsValid()
        {
            return DateTime.Now < Endlicence;
        }
    }
}
