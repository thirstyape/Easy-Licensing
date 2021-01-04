using System.Text.Json.Serialization;

namespace Easy_Licensing.Models.LicenseParts
{
    /// <summary>
    /// Model to define properties used in Hardware Identity based licensing
    /// </summary>
    public class HardwareIdentity
    {
        /// <summary>
        /// Stores the CPU serial number for the licensed hardware
        /// </summary>
        [JsonPropertyName("CPU Serial Number")]
        public string CpuSerialNumber { get; set; }

        /// <summary>
        /// Stores the drive serial number for the licensed hardware (operating system drive)
        /// </summary>
        [JsonPropertyName("Drive Serial Number")]
        public string DriveSerialNumber { get; set; }

        /// <summary>
        /// Stores the ethernet MAC address for the licensed hardware (first wired interface)
        /// </summary>
        [JsonPropertyName("Ethernet MAC Address")]
        public string EthernetMacAddress { get; set; }

        /// <summary>
        /// Stores the wireless MAC address for the licensed hardware (first wireless interface)
        /// </summary>
        [JsonPropertyName("Wireless MAC Address")]
        public string WirelessMacAddress { get; set; }

        /// <summary>
        /// Specifies whether the license is to be used on a virtual machine
        /// </summary>
        [JsonPropertyName("Is Virtual Machine")]
        public bool IsVirtualMachine { get; set; }
    }
}
