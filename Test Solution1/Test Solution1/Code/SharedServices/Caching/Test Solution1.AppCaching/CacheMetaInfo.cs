using System;
using System.Collections.Generic;

namespace Caching
{
    public class CacheTypeMetaInfo
    {
        public CacheTypeMetaInfo()
        {
            Methods = new List<CacheMethodMetaInfo>();
        }

        public Type Type { get; set; }

        public IList<CacheMethodMetaInfo> Methods { get; set; }

        public override string ToString()
        {
            return string.Format("{0}:{1} methods", Type.ToString(), Methods == null ? 0 : Methods.Count);
        }
    }

    public class CacheMethodMetaInfo
    {
        public string MethodName { get; set; }
        public int ExpirationInMinutes { get; set; }
        public ExpirationType ExpirationType { get; set; }

        public override string ToString()
        {
            return string.Format("{0}:{1}:{2}", MethodName, ExpirationType, ExpirationInMinutes);
        }
    }

    public enum ExpirationType
    {
        Absolute,
        Sliding
    }
}