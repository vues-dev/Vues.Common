using System;
namespace Vues.Common.Extensions
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Check if source contains element
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="element">Element</param>
        /// <param name="source">Source</param>
        /// <returns>True if source contains element</returns>
        public static bool In<T>(this T element, IEnumerable<T> source)
        => source.Contains(element);

        /// <summary>
        /// Check if source contains element
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="element">Element</param>
        /// <param name="source">Source</param>
        /// <returns>True if source contains element</returns>
        public static bool In<T>(this T element, params T[] source)
            => source.Contains(element);

        /// <summary>
        /// Check if source doesn't contain element
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="element">Element</param>
        /// <param name="source">Source</param>
        /// <returns>True if source doesn't contain element</returns>
        public static bool NotIn<T>(this T element, IEnumerable<T> source)
            => !source.Contains(element);

        /// <summary>
        /// Check if source doesn't contain element
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="element">Element</param>
        /// <param name="source">Source</param>
        /// <returns>True if source doesn't contain element</returns>
        public static bool NotIn<T>(this T element, params T[] source)
            => !source.Contains(element);

        /// <summary>
        /// Check if source is null or it is empty
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="source">Source</param>
        /// <returns>True if source is null or empty</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
            => source == null || source.Count() == 0;
    }
}

