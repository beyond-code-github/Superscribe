namespace Superscribe.Cache
{
    using System;
    using System.Collections.Concurrent;

    public static class RouteCache
    {
        public static ConcurrentDictionary<string, object> Cache = new ConcurrentDictionary<string, object>();
        
        public static bool TryGet<T>(string url, out CacheEntry<T> entry)
        {
            object result;

            if (Cache.TryGetValue(url, out result))
            {
                entry = (CacheEntry<T>)result;
                return true;
            }

            entry = null;
            return false;
        }
        
        public static void Store<T>(string url, CacheEntry<T> handler)
        {
            Cache.TryAdd(url, handler);
        }
    }
}
