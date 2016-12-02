namespace Caching.Interface
{
    public interface ICacheProvider
    {
        object Get(string key);
        void Add(CacheRequest item);
        void Remove(string key);
    }
}