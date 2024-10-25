using System;


namespace EasyJob_ProDG.Data
{
    public static class Licence
    {
        private static readonly DateTime Endlicence = new DateTime(2024, 12, 31, 23, 59, 59);

        /// <summary>
        /// Gets the end of licence date and time.
        /// </summary>
        public static DateTime EndLicence => Endlicence;

        /// <summary>
        /// Checking the validity of the licence.
        /// <see langword="true"/> if licence is valid.
        /// </summary>
        /// <returns>True if licence is valid.</returns>
        public static bool IsValid()
        {
            return DateTime.Now < Endlicence;
        }

        /// <summary>
        /// Licence text that is displayed to users.
        /// </summary>
        public static string LicenceText
            => "\tThe users are free to utilize this version of EasyJob ProDG Pro software aquired from the official sources in their professional or any other tasks within the licence period without any limitations and at their own risk.\n"
            + "\tNo parts of the code as well as no files forming part of this software can be reproduces or published anywhere without written permission received from any email registred in @imdg.pro domain.\n"
            + "\tThe users are encouraged to share their experience in use of this software with other users as well as with imdg.pro team, and inform other potential users of the software.";
    }
}
