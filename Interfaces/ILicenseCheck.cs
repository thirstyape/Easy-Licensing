namespace Easy_Licensing.Interfaces
{
    /// <summary>
    /// Defines methods and properties required by license checking classes
    /// </summary>
    public interface ILicenseCheck
    {
        /// <summary>
        /// A message to display to the end user on check failure
        /// </summary>
        public string FailureMessage { get; set; }

        /// <summary>
        /// Container to pass license checking configuration to check
        /// </summary>
        public ILicenseRequirements Settings { get; set; }

        /// <summary>
        /// Executes the check on the provided license
        /// </summary>
        /// <param name="licenseText">The text of the license to check</param>
        public bool CheckLicense(string licenseText);
    }
}
