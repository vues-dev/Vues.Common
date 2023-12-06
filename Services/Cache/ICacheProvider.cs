using System;
namespace Vues.Common.Services;

/// <summary>
/// Провайдер кэширования объектов в приложении
/// </summary>
public interface ICacheProvider
{
    void Remove(string cacheKey);
    T Get<T>(string cacheKey, int cacheTimeInMinutes, Func<T> func);
    T Get<T>(string cacheKey, Func<T> func);
    Task<T> GetAsync<T>(string cacheKey, Func<Task<T>> func);
    Task<T> GetAsync<T>(string cacheKey, int cacheTimeInMinutes, Func<Task<T>> func);
}
