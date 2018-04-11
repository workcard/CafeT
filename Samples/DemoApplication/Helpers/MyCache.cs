using System;
using System.Reflection;
using System.Runtime.Caching;

namespace DemoApplication.Helpers
{
    /// <summary>
    /// Taken from http://www.codeproject.com/Articles/290935/UsingMemoryCacheinNet with some modification
    /// </summary>
    public static class MyCache
    {
        private static readonly ObjectCache cache = MemoryCache.Default;
        private static CacheItemPolicy _policy;
        private static CacheEntryRemovedCallback _callback;

        public static void AddToMyCache(string cacheKeyName, PropertyInfo[] cacheItem,
            CacheItemPriority cacheItemPriority)
        {
            _callback = MyCachedItemRemovedCallback;
            _policy = new CacheItemPolicy
            {
                Priority = cacheItemPriority,
                AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(1000.00),
                RemovedCallback = _callback
            };
            cache.Set(cacheKeyName, cacheItem, _policy);
        }

        public static PropertyInfo[] GetMyCachedItem(String cacheKeyName)
        {
            return (PropertyInfo[]) cache[cacheKeyName];
        }

        public static void RemoveMyCachedItem(String cacheKeyName)
        {
            if (cache.Contains(cacheKeyName))
            {
                cache.Remove(cacheKeyName);
            }
        }

        private static void MyCachedItemRemovedCallback(CacheEntryRemovedArguments arguments)
        {
            var strLog = String.Concat("Reason: ", arguments.RemovedReason.ToString(), "| Key‐Name:",
                arguments.CacheItem.Key, " | Value‐Object:", arguments.CacheItem.Value.ToString());
        }
    }
}