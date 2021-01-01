using Easy_Licensing.Interfaces;
using Easy_Licensing.Properties;

namespace Easy_Licensing.Checks
{
    /// <summary>
    /// Checks to see that the provided license details match the active hardware details
    /// </summary>
    public class CheckHardwareIdentity : ILicenseCheck
    {
        /// <summary>
        /// Prepares check for use
        /// </summary>
        /// <param name="licenseRequirements">Object containing current settings</param>
        public CheckHardwareIdentity(ILicenseRequirements licenseRequirements)
        {
            Settings = licenseRequirements;
        }

        /// <inheritdoc/>
        public string FailureMessage { get; set; }

        /// <inheritdoc/>
        public ILicenseRequirements Settings { get; set; }

        /// <inheritdoc/>
        public bool CheckLicense()
        {
            FailureMessage = Resources.FailedHardwareIdentity;

            return false;
        }
    }
}
