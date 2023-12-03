using System;
using System.ComponentModel;

namespace Vues.Common.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Get string from Description Attribute of enum value
        /// </summary>
        /// <param name="enumValue">Enum value</param>
        /// <returns>String from description attribute</returns>
        public static string GetEnumDescription(this Enum enumValue)
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            var descriptionAttributes = (DescriptionAttribute[])fieldInfo!.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return descriptionAttributes.Length > 0 ? descriptionAttributes[0].Description : string.Empty;
        }
    }
}