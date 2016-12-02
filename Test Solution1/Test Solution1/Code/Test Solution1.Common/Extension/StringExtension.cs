using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Test_Solution1.Common
{
    /// <summary>
    ///     Represents extensions to String types.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        ///     Returns true if the entity is empty. Can be used safely on null values of String.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this String text)
        {
            return string.IsNullOrEmpty(text);
        }

        /// <summary>
        ///     Returns the string's value, or String.Empty if the value is null (guarantees a non-null result).
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static String ToStringOrEmpty(this String text)
        {
            if (String.IsNullOrEmpty(text))
                return String.Empty;
            return text;
        }

        /// <summary>
        ///     Overwritten method that combines the string array into a single string.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static String ToString(this String[] array, String delimiter)
        {
            if (array != null && array.Length > 0)
            {
                return String.Join(delimiter, array);
            }

            return String.Empty;
        }

        /// <summary>
        ///     Overwritten method that combines the string lists into a single string.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static String ToString(this IEnumerable<string> list, String delimiter)
        {
            var enumerable = list as IList<string> ?? list.ToList();
            if (list != null && enumerable.Count() > 0)
            {
                return String.Join(delimiter, enumerable);
            }

            return String.Empty;
        }

        /// <summary>
        ///     Takes a concatenated string and split is via Pascal case into a string with spaces.  For example with enums,
        ///     it would split "UserAcceptedMessage" and return a string with "User Accepted Message".
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static String SplitToProperName(this String value)
        {
            return Regex.Replace(value, "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))", "$1 ");
        }

        /// <summary>
        ///     Scrubs a string to a safe and clean url.
        /// </summary>
        /// <param name="stringToParse"></param>
        /// <returns></returns>
        public static String ToSafeUrl(this String stringToParse)
        {
            var safeUrl = String.Empty;

            if (false == String.IsNullOrEmpty(stringToParse))
            {
                var regex = new Regex("[^a-zA-Z0-9 ]");

                safeUrl = regex.Replace(stringToParse, "");
                safeUrl = safeUrl.Trim();
                safeUrl = safeUrl.Replace(" ", "-");

                while (safeUrl.IndexOf("--") != -1) //remove double -
                {
                    safeUrl = safeUrl.Replace("--", "-");
                }
            }
            return safeUrl.ToLowerInvariant();
        }

        /// <summary>
        ///     Scrubs a domain username string to a safe and clean name.
        ///     Windows domainname allows only numbers, alphanumeric, period (.) and dash (-)
        /// </summary>
        /// <param name="domainName"></param>
        /// <returns></returns>
        public static String ToSafeDomainName(this String domainName)
        {
            var safeName = String.Empty;

            if (false == String.IsNullOrEmpty(domainName))
            {
                var userName = domainName.Contains("\\")
                                   ? domainName.Split('\\')[1]
                                   : domainName;

                var regex = new Regex("[^a-zA-Z0-9.-]");

                safeName = regex.Replace(userName, "");
                safeName = safeName.Trim();
            }
            return safeName.ToLowerInvariant();
        }
    }
}