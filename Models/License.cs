using Easy_Licensing.Interfaces;
using Easy_Licensing.Models.LicenseParts;

using System.Text.Json.Serialization;

namespace Easy_Licensing.Models
{
    /// <summary>
    /// Default implementation of <see cref="ILicense"/>
    /// </summary>
    public class License : ILicense
    {
        /// <inheritdoc/>
        [JsonPropertyName("Product Key")]
        public ProductKey ProductKey { get; set; } = new ProductKey();

        /// <inheritdoc/>
        [JsonPropertyName("Hardware Identity")]
        public HardwareIdentity HardwareIdentity { get; set; } = new HardwareIdentity();

        /// <inheritdoc/>
        [JsonPropertyName("User Identity")]
        public UserIdentity UserIdentity { get; set; } = new UserIdentity();

        /// <inheritdoc/>
        [JsonPropertyName("Time Lock")]
        public TimeLock TimeLock { get; set; } = new TimeLock();
    }
}
