using Easy_Licensing.Models.LicenseParts;

namespace Easy_Licensing.Interfaces
{
    /// <summary>
    /// Defines parts contained within a valid license model
    /// </summary>
    public interface ILicense
    {
        /// <summary>
        /// Stores product key license details
        /// </summary>
        public ProductKey ProductKey { get; set; }

        /// <summary>
        /// Stores hardware identity license details
        /// </summary>
        public HardwareIdentity HardwareIdentity { get; set; }

        /// <summary>
        /// Stores user identity license details
        /// </summary>
        public UserIdentity UserIdentity { get; set; }

        /// <summary>
        /// Stores time lock license details
        /// </summary>
        public TimeLock TimeLock { get; set; }
    }
}
