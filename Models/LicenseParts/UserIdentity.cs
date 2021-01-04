using System.Text.Json.Serialization;

namespace Easy_Licensing.Models.LicenseParts
{
    /// <summary>
    /// Model to define properties used in User Identity based licensing
    /// </summary>
    public class UserIdentity
    {
        /// <summary>
        /// Stores the username for the licensed user
        /// </summary>
        [JsonPropertyName("Username")]
        public string Username { get; set; }

        /// <summary>
        /// Stores the domain for the licensed user
        /// </summary>
        [JsonPropertyName("Domain")]
        public string Domain { get; set; }

        /// <summary>
        /// Stores the email address for the licensed user
        /// </summary>
        [JsonPropertyName("Email Address")]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Stores the home folder address for the licensed user
        /// </summary>
        [JsonPropertyName("Home Folder")]
        public string HomeFolder { get; set; }
    }
}
