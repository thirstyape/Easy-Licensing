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
    }
}
