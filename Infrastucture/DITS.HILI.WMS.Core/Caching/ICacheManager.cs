namespace DITS.HILI.WMS.Core.Caching
{
    public interface ICacheManager
    {
        T Get<T>(string key);
        void Set(string key, object data, int? cacheTime = null);
        bool IsSet(string key);
        void Remove(string key);
        void Clear();
    }
}
