using System;


namespace EasyJob_ProDG.Data
{
    public static class License
    {
        private static readonly DateTime Endlicense = new DateTime(2027, 06, 30, 23, 59, 59);

        /// <summary>
        /// Gets the end of license date and time.
        /// </summary>
        public static DateTime EndLicense => Endlicense;

        /// <summary>
        /// Checking the validity of the license.
        /// <see langword="true"/> if license is valid.
        /// </summary>
        /// <returns>True if license is valid.</returns>
        public static bool IsValid()
        {
            return DateTime.Now < Endlicense;
        }

        /// <summary>
        /// license text that is displayed to users.
        /// </summary>
        public static string LicenseText
            => "\tThe users are free to utilize this version of EasyJob ProDG Pro software aquired from the official sources in their professional or any other tasks within the license period without any limitations and at their own risk.\n"
            + "\tNo parts of the code as well as no files forming part of this software can be reproduces or published anywhere without written permission received from any email registred in @imdg.pro domain.\n"
            + "\tThe users are encouraged to share their experience in use of this software with other users as well as with imdg.pro team, and inform other potential users of the software.";
    }
}
