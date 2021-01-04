using Easy_Licensing.Enums;
using Easy_Licensing.Interfaces;
using Easy_Licensing.Properties;

namespace Easy_Licensing.Checks
{
    /// <summary>
    /// Checks to see that the provided license has not expired
    /// </summary>
    public class CheckTimeLock : ILicenseCheck
    {
        /// <summary>
        /// Prepares check for use
        /// </summary>
        /// <param name="licenseRequirements">Object containing current settings</param>
        public CheckTimeLock(ILicenseRequirements licenseRequirements)
        {
            Settings = licenseRequirements;
        }

        /// <inheritdoc/>
        public string FailureMessage { get; set; }

        /// <inheritdoc/>
        public ILicenseRequirements Settings { get; set; }

        /// <inheritdoc/>
        public bool CheckLicense(string licenseText)
        {
            // Reset
            FailureMessage = null;

            // Check for inactive
            if ((Settings.LicenseType & LicenseTypes.TimeLocked) != 0)
                return true;

            // Run check
            FailureMessage = Resources.FailedTimeLock;

            // Return result
            return false;
        }
    }
}
