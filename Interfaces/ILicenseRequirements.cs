using Easy_Licensing.Enums;

namespace Easy_Licensing.Interfaces
{
    /// <summary>
    /// Interface to define elements required by built-in license checks
    /// </summary>
    public interface ILicenseRequirements
    {
        /// <summary>
        /// Specifies the type of license to check
        /// </summary>
        /// <remarks>
        /// To use multiple types set value as follows: <see cref="LicenseTypes.ProductKey"/> | <see cref="LicenseTypes.TimeLocked"/>
        /// </remarks>
        public LicenseTypes LicenseType { get; set; }

        /// <summary>
        /// Specifies whether provided licenses will have an associated hash file to verify tampering has not occurred
        /// </summary>
        public bool IsHashed { get; set; }

        /// <summary>
        /// Specifies whether provided licenses are encrypted and require decryption prior to checking
        /// </summary>
        public bool IsEncrypted { get; set; }

        /// <summary>
        /// Specifies whether to check the licensed machine's virtualization status
        /// </summary>
        public bool CheckVirtualMachine { get; set; }
    }
}
