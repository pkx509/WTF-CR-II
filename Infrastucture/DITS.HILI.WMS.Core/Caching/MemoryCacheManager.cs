using System;
using System.Runtime.Caching;

namespace DITS.HILI.WMS.Core.Caching
{

    public class MemoryCacheManager : ICacheManager
    {
        protected ObjectCache Cache => MemoryCache.Default;

        public virtual T Get<T>(string key)
        {
            return (T)Cache[key];
        }

        public virtual void Set(string key, object data, int? cacheTime = null)
        {
            if (data == null)
            {
                return;
            }

            CacheItemPolicy policy = new CacheItemPolicy();
            if (cacheTime.HasValue)
            {
                policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime.Value);
            }

            Cache.Add(new CacheItem(key, data), policy);
        }

        public virtual bool IsSet(string key)
        {
            return (Cache.Contains(key));
        }

        public virtual void Remove(string key)
        {
            Cache.Remove(key);
        }

        public virtual void Clear()
        {
            foreach (System.Collections.Generic.KeyValuePair<string, object> item in Cache)
            {
                Remove(item.Key);
            }
        }
    }
}
