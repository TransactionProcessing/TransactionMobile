using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransactionMobile.Maui.BusinessLogic.Services
{
    using Microsoft.Extensions.Caching.Memory;

    public interface IMemoryCacheService
    {
        bool TryGetValue<T>(string Key, out T cache);
        void Set<T>(string key, T cache);

        void Set<T>(string key,
                    T cache,
                    MemoryCacheEntryOptions options);
    }

    public class MemoryCacheService : IMemoryCacheService
    {
        private readonly IMemoryCache MemoryCache;

        public MemoryCacheService(IMemoryCache memoryCache)
        {
            this.MemoryCache = memoryCache;
        }

        public void Set<T>(string key, T cache)
        {
            this.Set(key, cache, default);
        }

        public void Set<T>(string key, T cache, MemoryCacheEntryOptions options)
        {
            this.MemoryCache.Set(key, cache, options);
        }

        public bool TryGetValue<T>(string Key, out T cache)
        {
            if (this.MemoryCache.TryGetValue(Key, out T cachedItem))
            {
                cache = cachedItem;
                return true;
            }
            cache = default(T);
            return false;
        }
    }
}
