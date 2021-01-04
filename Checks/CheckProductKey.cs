using Easy_Licensing.Enums;
using Easy_Licensing.Interfaces;
using Easy_Licensing.Properties;

namespace Easy_Licensing.Checks
{
    /// <summary>
    /// Checks to see that the provided license includes a valid product key
    /// </summary>
    public class CheckProductKey : ILicenseCheck
    {
        /// <summary>
        /// Prepares check for use
        /// </summary>
        /// <param name="licenseRequirements">Object containing current settings</param>
        public CheckProductKey(ILicenseRequirements licenseRequirements)
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
            if ((Settings.LicenseType & LicenseTypes.ProductKey) != 0)
                return true;

            // Run check
            FailureMessage = Resources.FailedProductKey;

            // Return result
            return false;
        }
    }
}
