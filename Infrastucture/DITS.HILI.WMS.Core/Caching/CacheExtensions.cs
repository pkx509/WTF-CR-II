using System;

namespace DITS.HILI.WMS.Core.Caching
{
    public static class CacheExtensions
    {
        public static T Get<T>(this ICacheManager cacheManager, string key, Func<T> acquire)
        {
            return Get2(cacheManager, key, null, acquire);
        }


        public static T Get2<T>(this ICacheManager cacheManager, string key, int? cacheTime, Func<T> acquire)
        {
            if (cacheManager.IsSet(key))
            {
                return cacheManager.Get<T>(key);
            }
            else
            {
                T result = acquire();
                if (!cacheTime.HasValue || cacheTime.Value > 0)
                {
                    cacheManager.Set(key, result, cacheTime);
                }

                return result;
            }
        }
    }
}
