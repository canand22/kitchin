using System;
using System.Runtime.Caching;

namespace KitchIn.Core.Services.Cache
{
    public static class KitchInCache
    {
        private static ObjectCache cache = MemoryCache.Default; 
        private static CacheItemPolicy policy = null; 
        private static CacheEntryRemovedCallback callback = null;

        public static void AddToCache(String cacheKeyName, Object cacheItem, CachePriority cacheItemPriority) 
        { 
            callback = CachedItemRemovedCallback; 
            policy = new CacheItemPolicy(); 
            policy.Priority = (cacheItemPriority == CachePriority.Default) 
                ? CacheItemPriority.Default 
                : CacheItemPriority.NotRemovable; 

            policy.AbsoluteExpiration = DateTimeOffset.Now.AddHours(12.00); 
            policy.RemovedCallback = callback; 
            cache.Set(cacheKeyName, cacheItem, policy); 
        }

        public static Object GetCachedItem(String cacheKeyName) 
        { 
            return cache[cacheKeyName] as Object; 
        }

        public static void RemoveCachedItem(String cacheKeyName) 
        { 
            if (cache.Contains(cacheKeyName)) 
            { 
                cache.Remove(cacheKeyName); 
            } 
        }

        private static void CachedItemRemovedCallback(CacheEntryRemovedArguments arguments) 
        { 
            // Log these values from arguments list 
            String strLog = String.Concat("Reason: ", arguments.RemovedReason.ToString(), "| Key-Name: ", arguments.CacheItem.Key, " | Value-Object: ", 
                arguments.CacheItem.Value.ToString()); 
        }    
    }
}
