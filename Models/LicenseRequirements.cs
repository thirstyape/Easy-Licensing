using Easy_Licensing.Enums;
using Easy_Licensing.Interfaces;

using System.Text.Json.Serialization;

namespace Easy_Licensing.Models
{
    /// <summary>
    /// Default implementation of <see cref="ILicenseRequirements"/>
    /// </summary>
    public class LicenseRequirements : ILicenseRequirements
    {
        /// <inheritdoc/>
        [JsonPropertyName("License Type")]
        public LicenseTypes LicenseType { get; set; } = LicenseTypes.ProductKey;

        /// <inheritdoc/>
        [JsonPropertyName("Is Hashed")]
        public bool IsHashed { get; set; }

        /// <inheritdoc/>
        [JsonPropertyName("Is Encrypted")]
        public bool IsEncrypted { get; set; }

        /// <inheritdoc/>
        [JsonPropertyName("Check CPU Serial")]
        public bool CheckCpuSerial { get; set; }

        /// <inheritdoc/>
        [JsonPropertyName("Check Disk Serial")]
        public bool CheckDiskSerial { get; set; }

        /// <inheritdoc/>
        [JsonPropertyName("Check Ethernet MAC")]
        public bool CheckEthernetMac { get; set; }

        /// <inheritdoc/>
        [JsonPropertyName("Check Wireless MAC")]
        public bool CheckWirelessMac { get; set; }

        /// <inheritdoc/>
        [JsonPropertyName("Check Virtual Machine")]
        public bool CheckVirtualMachine { get; set; }
    }
}
