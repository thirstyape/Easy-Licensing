using System;
using System.Text.Json.Serialization;

namespace Easy_Licensing.Models.LicenseParts
{
    /// <summary>
    /// Model to define properties used in Time Lock based licensing
    /// </summary>
    public class TimeLock
    {
        /// <summary>
        /// Stores the date at which the license will expire and cease operation
        /// </summary>
        [JsonPropertyName("License Expiry")]
        public DateTime? LicenseExpiry { get; set; }
    }
}
