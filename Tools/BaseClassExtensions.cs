using System;
using System.Collections.Generic;

namespace Easy_Licensing.Tools
{
    /// <summary>
    /// Class containing extension methods for base classes (ex. string, int, bool)
    /// </summary>
    public static class BaseClassExtensions
    {
        /// <summary>
        /// Iterates over a bitmasked enum value and returns each of the flags that is active
        /// </summary>
        /// <typeparam name="TEnum">The type of the enum to iterate</typeparam>
        /// <param name="value">The value to iterate over</param>
        public static IEnumerable<TEnum> GetFlags<TEnum>(this TEnum value) where TEnum : Enum
        {
            foreach (Enum flag in Enum.GetValues(value.GetType()))
                if (value.HasFlag(flag))
                    yield return (TEnum)flag;
        }

        /// <summary>
        /// Checks whether a byte array is null or empty
        /// </summary>
        /// <param name="value">The value to check</param>
        public static bool IsNullOrEmpty(this byte[] value)
        {
            return value == null || value.Length == 0;
        }
    }
}
