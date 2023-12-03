using System;
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
    }
}