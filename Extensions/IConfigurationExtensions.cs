using System;
namespace Vues.Common.Extensions
{
    public static class IConfigurationExtensions
    {
        public static T BindSection<T>(this IConfiguration configuration, string key) where T: class, new()
        {
            var bindable = new T();
            configuration.GetSection(key).Bind(bindable);
            return bindable;
        }
    }
}