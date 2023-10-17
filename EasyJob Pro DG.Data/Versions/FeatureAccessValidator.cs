namespace EasyJob_ProDG.Data
{
    /// <summary>
    /// Defines what features are enabled depending on licence version.
    /// </summary>
    public class FeatureAccessValidator
    {
        private static FeatureAccessValidator _validator;
        private string _inputVersion;
        private Versions _version;


        // ----- Public properties -----






        /// <summary>
        /// Defines ProDG version from string version.
        /// </summary>
        /// <param name="version"></param>
        /// <returns>Version as one of <see cref="Versions"/> enum item.</returns>
        private Versions DefineVersion(string version)
        {
            return Versions.Undefined;
        }


        // ----- Create a singleton instance ---

        /// <summary>
        /// Method to create and get access to <see cref="FeatureAccessValidator"/>
        /// </summary>
        /// <returns>Access to <see cref="FeatureAccessValidator"/> the only instance.</returns>
        public static FeatureAccessValidator CreateValidator (string version)
        {
            if (_validator == null)
            {
                _validator = new FeatureAccessValidator(version);
            }
            return _validator;
        }


        // ----- Constructors -----
        private FeatureAccessValidator(string version)
        {
            _inputVersion = version;
            _version = DefineVersion(version);
        }
    }
}
