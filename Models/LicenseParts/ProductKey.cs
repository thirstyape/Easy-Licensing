using System.Text.Json.Serialization;

namespace Easy_Licensing.Models.LicenseParts
{
    /// <summary>
    /// Model to define properties used in Product Key based licensing
    /// </summary>
    public class ProductKey
    {
        /// <summary>
        /// Stores the key code for the product key
        /// </summary>
        [JsonPropertyName("Key")]
        public string Key { get; set; }
    }
}
