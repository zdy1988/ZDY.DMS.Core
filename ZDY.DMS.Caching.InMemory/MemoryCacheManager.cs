using System;
using Microsoft.Extensions.Caching.Memory;

namespace ZDY.DMS.Caching.InMemory
{
    /// <summary>
    /// Represents a MemoryCache
    /// </summary>
    public partial class MemoryCacheManager : ICacheManager
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheManager(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the specified key.</returns>
        public T Get<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }

        /// <summary>
        /// Adds the specified key and object to the cache.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="data">Data</param>
        /// <param name="cacheTime">Cache time</param>
        public void Set(string key, object data, int cacheTime)
        {
            if (data == null)
                return;

            _memoryCache.Set(key, data, new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(DateTime.Now + TimeSpan.FromMinutes(cacheTime)));
        }

        /// <summary>
        /// Gets a value indicating whether the value associated with the specified key is cached
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>Result</returns>
        public bool IsSet(string key)
        {
            object data;
            return (_memoryCache.TryGetValue(key, out data));
        }

        /// <summary>
        /// Removes the value with the specified key from the cache
        /// </summary>
        /// <param name="key">/key</param>
        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        /// <summary>
        /// Removes items by pattern
        /// </summary>
        /// <param name="pattern">pattern</param>
        public void RemoveByPattern(string pattern)
        {
            //var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            //var keysToRemove = new List<String>();

            //foreach (var item in _memoryCache.)
            //    if (regex.IsMatch(item.Key))
            //        keysToRemove.Add(item.Key);

            //foreach (string key in keysToRemove)
            //{
            //    Remove(key);
            //}

            throw new NotSupportedException();
        }

        /// <summary>
        /// Clear all cache data
        /// </summary>
        public void Clear()
        {
            //foreach (var item in Cache)
            //    Remove(item.Key);
            throw new NotSupportedException();
        }
    }
}
