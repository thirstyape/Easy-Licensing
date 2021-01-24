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
        /// Specifies whether to check the licensed machine's CPU serial number
        /// </summary>
        public bool CheckCpuSerial { get; set; }

        /// <summary>
        /// Specifies whether to check the licensed machine's primary disk serial number
        /// </summary>
        public bool CheckDiskSerial { get; set; }

        /// <summary>
        /// Specifies whether to check the licensed machine's primary Ethernet card MAC address
        /// </summary>
        public bool CheckEthernetMac { get; set; }

        /// <summary>
        /// Specifies whether to check the licensed machine's primary wireless adapter MAC address
        /// </summary>
        public bool CheckWirelessMac { get; set; }

        /// <summary>
        /// Specifies whether to check the licensed machine's virtualization status
        /// </summary>
        public bool CheckVirtualMachine { get; set; }
    }
}
