using Easy_Licensing.Enums;
using Easy_Licensing.Interfaces;
using Easy_Licensing.Properties;

using System.Collections.Generic;
using System.Management;
using System.Runtime.InteropServices;

namespace Easy_Licensing.Checks
{
    /// <summary>
    /// Checks to see that the provided license details match the active hardware details
    /// </summary>
    public class CheckHardwareIdentity : ILicenseCheck
    {
        private readonly List<string> Failures = new List<string>();

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
        public bool CheckLicense(ILicense license)
        {
            // Reset
            FailureMessage = null;
            Failures.Clear();

            // Check for inactive
            if ((Settings.LicenseType & LicenseTypes.HardwareIdentity) == 0)
                return true;

            // Run check
            var pass = false;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                pass = CheckLicenseWindows(license);
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                pass = CheckLicenseOsx(license);
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                pass = CheckLicenseLinux(license);

            // Return result
            if (pass == false)
                FailureMessage = string.Format(Resources.FailedHardwareIdentity, string.Join(',', Failures));

            return pass;
        }

        /// <summary>
        /// Checks for a valid license on Linux machines
        /// </summary>
        private bool CheckLicenseLinux(ILicense license)
        {
            var pass = false;

            // Check for running in virtual machine
            if (Settings.CheckVirtualMachine)
            {

            }

            return pass;
        }

        /// <summary>
        /// Checks for a valid license on Mac OSX machines
        /// </summary>
        private bool CheckLicenseOsx(ILicense license)
        {
            var pass = false;

            // Check for running in virtual machine
            if (Settings.CheckVirtualMachine)
            {

            }

            return pass;
        }

        /// <summary>
        /// Checks for a valid license on Windows machines
        /// </summary>
        private bool CheckLicenseWindows(ILicense license)
        {
            var pass = true;

            // Check for running in virtual machine
            if (Settings.CheckVirtualMachine)
            {
                var isVm = false;

                using (var searcher = new ManagementObjectSearcher("Select * from Win32_ComputerSystem"))
                {
                    using var items = searcher.Get();

                    foreach (var item in items)
                    {
                        var manufacturer = item["Manufacturer"].ToString().ToLower();
                        var model = item["Model"].ToString();

                        if (
                            (manufacturer == "microsoft corporation" && model.ToUpperInvariant().Contains("VIRTUAL")) ||
                            manufacturer.Contains("vmware") ||
                            model == "VirtualBox"
                        )
                        {
                            isVm = true;
                        }
                    }
                }

                if (license.HardwareIdentity.IsVirtualMachine != isVm)
                {
                    pass = false;
                    Failures.Add(isVm ? Resources.FailedHardwareVirtualBlocked : Resources.FailedHardwareVirtualRequired);
                }
            }

            return pass;
        }
    }
}
