namespace Superscribe.Cache
{
    public class UselessCache : IRouteCache
    {
        public bool TryGet<T>(string url, out CacheEntry<T> entry)
        {
            entry = null;
            return false;
        }

        public void Store<T>(string url, CacheEntry<T> handler)
        {
            // Do nothing
        }
    }
}
