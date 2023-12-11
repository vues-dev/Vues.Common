using System;
using System.Text;
using System.Globalization;

namespace Vues.Common.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Trim string and check if string is null or empty
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>true if trimmed string is null or empty</returns>
        public static bool IsNullOrEmptyTrim(this string? str)
        => string.IsNullOrEmpty(str?.Trim());

        /// <summary>
        /// Encode string to base64
        /// </summary>
        /// <param name="data">Data</param>
        /// <returns>Encoded base64 string</returns>
        public static string ToBase64(this string data)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(data);
            return Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Decode base64 string
        /// </summary>
        /// <param name="base64EncodedData">Base64 string</param>
        /// <returns>Data</returns>
        public static string FromBase64(this string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        /// <summary>
        /// Converts a string to Camel Case.
        /// </summary>
        /// <param name="input">The input string to convert.</param>
        /// <returns>The string converted to Camel Case.</returns>
        public static string ToCamelCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            string[] words = input.Split(' ');
            StringBuilder result = new StringBuilder(words[0].ToLower());

            for (int i = 1; i < words.Length; i++)
            {
                result.Append(char.ToUpper(words[i][0]) + words[i].Substring(1));
            }

            return result.ToString();
        }

        /// <summary>
        /// Converts a string to Pascal Case.
        /// </summary>
        /// <param name="input">The input string to convert.</param>
        /// <returns>The string converted to Pascal Case.</returns>
        public static string ToPascalCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(input.Replace(' ', '_').Replace('-', '_')).Replace("_", "");
        }

        /// <summary>
        /// Converts a string to Snake Case.
        /// </summary>
        /// <param name="input">The input string to convert.</param>
        /// <returns>The string converted to Snake Case.</returns>
        public static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return input.Replace(' ', '_').ToLower();
        }

        /// <summary>
        /// Converts a string to Kebab Case.
        /// </summary>
        /// <param name="input">The input string to convert.</param>
        /// <returns>The string converted to Kebab Case.</returns>
        public static string ToKebabCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            return input.Replace(' ', '-').ToLower();
        }
    }
}