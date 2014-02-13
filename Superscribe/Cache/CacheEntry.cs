namespace Superscribe.Cache
{
    using System;

    public class CacheEntry<T>
    {
        public T Info { get; set; }

        public Func<dynamic, object> OnComplete { get; set; }
    }
}
