using System;
using System.ComponentModel.DataAnnotations;

namespace Easy_Licensing.Enums
{
    /// <summary>
    /// A list of pre-defined licensing models
    /// </summary>
    /// <remarks>
    /// Flag values are resultant of 2^X (i.e. 2^0 = 1, 2^1 = 2, 2^2 = 4, ...)
    /// </remarks>
    [Flags]
    public enum LicenseTypes
    {
        /// <summary>
        /// A license that is based on a product key code
        /// </summary>
        [Display(Name = "Product Key", Description = "A license that is based on a product key code")]
        ProductKey = 0b_00000001,

        /// <summary>
        /// A license that uses hardware serial numbers
        /// </summary>
        [Display(Name = "Hardware Identity", Description = "A license that uses hardware serial numbers")]
        HardwareIdentity = 0b_00000010,

        /// <summary>
        /// A license that uses information about the licensee
        /// </summary>
        [Display(Name = "User Identity", Description = "A license that uses information about the licensee")]
        UserIdentity = 0b_00000100,

        /// <summary>
        /// A license that expires after a predefined interval of time
        /// </summary>
        [Display(Name = "Time Locked", Description = "A license that expires after a predefined interval of time")]
        TimeLocked = 0b_00001000
    }
}
