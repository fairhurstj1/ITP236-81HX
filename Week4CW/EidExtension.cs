namespace Week4CW
{
    /// <summary>
    /// Provides string extension helper methods.
    /// </summary>
    public static class EidExtension
    {
        /// <summary>
        /// Determines whether the first character in the string is uppercase.
        /// </summary>
        /// <param name="value">The input string to evaluate.</param>
        /// <returns>
        /// <c>true</c> when <paramref name="value"/> is not null or empty and starts with an uppercase character; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsCapitalized(this string value)
        {
            return !string.IsNullOrEmpty(value) && char.IsUpper(value[0]);
        }

        /// <summary>
        /// Returns the leftmost characters from a string.
        /// </summary>
        /// <param name="value">The source string.</param>
        /// <param name="length">The number of characters to return.</param>
        /// <returns>
        /// The leftmost substring with up to <paramref name="length"/> characters, or an empty string for null/empty input or non-positive length.
        /// </returns>
        public static string Left(this string value, int length)
        {
            if (string.IsNullOrEmpty(value) || length <= 0)
            {
                return string.Empty;
            }
            if (length >= value.Length)
            {
                return value;
            }
            return value.Substring(0, length);
        }

        /// <summary>
        /// Returns the rightmost characters from a string.
        /// </summary>
        /// <param name="value">The source string.</param>
        /// <param name="length">The number of characters to return.</param>
        /// <returns>
        /// The rightmost substring with up to <paramref name="length"/> characters, or an empty string for null/empty input or non-positive length.
        /// </returns>
        public static string Right(this string value, int length)
        {
            if (string.IsNullOrEmpty(value) || length <= 0)
            {
                return string.Empty;
            }
            if (length >= value.Length)
            {
                return value;
            }
            return value.Substring(value.Length - length, length);
        }
    }

}