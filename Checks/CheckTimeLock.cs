using Easy_Licensing.Enums;
using Easy_Licensing.Interfaces;
using Easy_Licensing.Properties;

using System;

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
        public bool CheckLicense(ILicense license)
        {
            // Reset
            FailureMessage = null;

            // Check for inactive
            if ((Settings.LicenseType & LicenseTypes.TimeLocked) == 0)
                return true;

            // Return result
            var pass = license.TimeLock.LicenseExpiry != null && license.TimeLock.LicenseExpiry < DateTime.Now;

            if (pass == false)
                FailureMessage = string.Format(Resources.FailedTimeLock, license.TimeLock.LicenseExpiry?.ToString("yyyy-MM-dd HH:mm:ss"));

            return pass;
        }
    }
}
