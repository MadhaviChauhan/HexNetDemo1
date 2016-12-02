using System;

namespace Caching.Interface
{
    public interface ICacheMetaInfoStore
    {
        CacheMethodMetaInfo GetCacheInfo(Type implementation, string methodName);
    }
}