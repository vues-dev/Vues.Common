using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Vues.Common.Services;

public class CacheProvider : ICacheProvider
{
    private class EmptyResultClass { }

    private readonly IMemoryCache _memoryCache;
    private readonly int _cacheTime;

    public CacheProvider(IMemoryCache memoryCache, IConfiguration configuration)
    {
        _memoryCache = memoryCache;
        _cacheTime = configuration["DefaultCacheTimeInMinutes"] == null ? 360
                  : configuration.GetValue<int>("DefaultCacheTimeInMinutes");
    }

    public void Remove(string cacheKey)
    {
        _memoryCache.Remove(cacheKey);
    }

    public T Get<T>(string cacheKey, int cacheTimeInMinutes, Func<T> func)
    {
        var cachedObject = _memoryCache.Get(cacheKey);

        if (cachedObject != null)
        {
            if (cachedObject is EmptyResultClass)
                return default!;

            return (T)cachedObject;
        }


        var expensiveObject = func();

        var absoluteExpiration = new DateTimeOffset(DateTime.Now.AddMinutes(cacheTimeInMinutes));

        if (expensiveObject == null)
        {
            _memoryCache.Set(key: cacheKey,
                             value: new EmptyResultClass(),
                             absoluteExpiration: absoluteExpiration);

            return default!;
        }

        _memoryCache.Set(cacheKey, expensiveObject, absoluteExpiration);

        return expensiveObject;
    }

    public async Task<T> GetAsync<T>(string cacheKey, int cacheTimeInMinutes, Func<Task<T>> func)
    {
        var cachedObject = _memoryCache.Get(cacheKey);

        if (cachedObject != null)
        {
            if (cachedObject is EmptyResultClass)
                return default!;

            return (T)cachedObject;
        }


        var expensiveObject = await func().ConfigureAwait(false);

        var absoluteExpiration = new DateTimeOffset(DateTime.Now.AddMinutes(cacheTimeInMinutes));

        if (expensiveObject == null)
        {
            _memoryCache.Set(key: cacheKey,
                             value: new EmptyResultClass(),
                             absoluteExpiration: absoluteExpiration);

            return default!;
        }

        _memoryCache.Set(cacheKey, expensiveObject, absoluteExpiration);

        return expensiveObject;
    }

    public T Get<T>(string cacheKey, Func<T> func)
        => Get(cacheKey: cacheKey,
               cacheTimeInMinutes: _cacheTime,
               func: func);

    public async Task<T> GetAsync<T>(string cacheKey, Func<Task<T>> func)
        => await GetAsync(cacheKey: cacheKey,
                          cacheTimeInMinutes: _cacheTime,
                          func: func);
}
