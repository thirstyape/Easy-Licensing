using System;
using System.ComponentModel.DataAnnotations;

namespace Easy_Licensing.Enums
{
    /// <summary>
    /// Used for filtering results to a given set of characters
    /// </summary>
    /// <remarks>
    /// Flag values are resultant of 2^X (i.e. 2^0 = 1, 2^1 = 2, 2^2 = 4, ...)
    /// </remarks>
    [Flags]
    public enum CharacterSets
    {
        /// <summary>
        /// Adds 0 - 9
        /// </summary>
        /// <remarks>
        /// 0123456789
        /// </remarks>
        [Display(Name = "Numeric", Description = "Adds 0 - 9")]
        Numeric = 0b_00000001,

        /// <summary>
        /// Adds A - Z in lower case
        /// </summary>
        /// <remarks>
        /// abcdefghijklmnopqrstuvwxyz
        /// </remarks>
        [Display(Name = "Lower Case", Description = "Adds A - Z in lower case")]
        Lowercase = 0b_00000010,

        /// <summary>
        /// Adds A - Z in upper case
        /// </summary>
        /// <remarks>
        /// ABCDEFGHIJKLMNOPQRSTUVWXYZ
        /// </remarks>
        [Display(Name = "Upper Case", Description = "Adds A - Z in upper case")]
        Uppercase = 0b_00000100,

        /// <summary>
        /// Adds various punctuation marks
        /// </summary>
        /// <remarks>
        /// `~!@#$%^&amp;*()_-=+[]{}|;:,.&lt;&gt;?
        /// </remarks>
        [Display(Name = "Punctuation", Description = "Adds various punctuation marks")]
        Punctuation = 0b_00001000
    }
}
