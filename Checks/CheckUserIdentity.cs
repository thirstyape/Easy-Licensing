using Easy_Licensing.Enums;
using Easy_Licensing.Interfaces;
using Easy_Licensing.Properties;

namespace Easy_Licensing.Checks
{
    /// <summary>
    /// Checks to see that the provided license details match the current user details
    /// </summary>
    public class CheckUserIdentity : ILicenseCheck
    {
        /// <summary>
        /// Prepares check for use
        /// </summary>
        /// <param name="licenseRequirements">Object containing current settings</param>
        public CheckUserIdentity(ILicenseRequirements licenseRequirements)
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
            if ((Settings.LicenseType & LicenseTypes.UserIdentity) != 0)
                return true;

            // Run check
            FailureMessage = Resources.FailedUserIdentity;

            // Return result
            return false;
        }
    }
}
