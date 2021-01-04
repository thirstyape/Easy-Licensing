using Easy_Licensing.Checks;
using Easy_Licensing.Interfaces;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;

namespace Easy_Licensing
{
    /// <summary>
    /// Main class to check whether licenses are valid
    /// </summary>
    public class LicenseCheckingService
    {
        private readonly List<ILicenseCheck> LicenseChecks;

        /// <summary>
        /// Prepares the checking service for use
        /// </summary>
        /// <param name="licenseRequirements">The parameters to check licenses with</param>
        public LicenseCheckingService(ILicenseRequirements licenseRequirements)
        {
            // Prepare checks
            Settings = licenseRequirements;
            FailureMessages = new List<string>();

            LicenseChecks = new List<ILicenseCheck>()
            {
                new CheckProductKey(licenseRequirements),
                new CheckHardwareIdentity(licenseRequirements),
                new CheckUserIdentity(licenseRequirements),
                new CheckTimeLock(licenseRequirements)
            };
        }

        /// <summary>
        /// The configuration settings to use when checking licenses
        /// </summary>
        public ILicenseRequirements Settings { get; private set; }

        /// <summary>
        /// Contains a listing of any reasons a license failed validation
        /// </summary>
        public IList<string> FailureMessages { get; private set; }

        /// <summary>
        /// Runs specified checks on the provided license
        /// </summary>
        /// <param name="licenseFile">The license file to check</param>
        /// <param name="userInformation">An optional dictionary containing user information to compare against the license</param>
        /// <param name="languageCode">An optional language code used for error text</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public bool CheckAndValidate(FileInfo licenseFile, Dictionary<string, string> userInformation = null, string languageCode = null)
        {
            // Input validation
            if (licenseFile.Exists == false)
                throw new ArgumentException("The specified license file does not exist", nameof(licenseFile));

            // Read license file and check
            var license = File.ReadAllText(licenseFile.FullName);

            return CheckAndValidate(license, userInformation, languageCode);
        }

        /// <summary>
        /// Runs specified checks on the provided license
        /// </summary>
        /// <param name="licenseText">The text of the license to check</param>
        /// <param name="userInformation">An optional dictionary containing user information to compare against the license</param>
        /// <param name="languageCode">An optional language code used for error text</param>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public bool CheckAndValidate(string licenseText, Dictionary<string, string> userInformation = null, string languageCode = null)
        {
            // Input validation
            if (string.IsNullOrEmpty(licenseText))
                throw new ArgumentNullException(nameof(licenseText), "Must provide license to check");

            // Reset
            FailureMessages.Clear();

            // Update error output language
            if (string.IsNullOrEmpty(languageCode) == false)
            {
                if (CheckValidLanguage(languageCode) == false)
                    throw new ArgumentException("Provided language code is not supported", nameof(languageCode));

                Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(languageCode);
            }

            // Run checks
            foreach (var check in LicenseChecks)
            {
                var pass = check.CheckLicense(licenseText);

                if (pass == false)
                    FailureMessages.Add(check.FailureMessage);
            }

            // Return result
            return FailureMessages.Count == 0;
        }

        /// <summary>
        /// Adds a custom license check to the list of checks that will be run against provided licenses
        /// </summary>
        /// <param name="check">The check to add</param>
        public void AddCheck(ILicenseCheck check)
        {
            LicenseChecks.Add(check);
        }

        /// <summary>
        /// Checks to see that the user provided language code is supported
        /// </summary>
        /// <param name="languageCode">The code to check</param>
        private bool CheckValidLanguage(string languageCode)
        {
            if (languageCode.Length == 2)
            {
                return languageCode switch
                {
                    "en" => true,
                    _ => false
                };
            }
            else if (languageCode.Length == 5 && languageCode[2] == '-')
            {
                return CheckValidLanguage(languageCode.Substring(0, 2));
            }
            else
            {
                return false;
            }
        }
    }
}
