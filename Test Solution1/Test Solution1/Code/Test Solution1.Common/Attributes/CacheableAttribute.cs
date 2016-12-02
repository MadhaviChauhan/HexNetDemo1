using System;

namespace Test_Solution1.Common.Attributes
{
    public class CacheableAttribute : Attribute
    {
        public enum CacheBehavior
        {
            Add,
            Remove
        }

        public CacheableAttribute()
        {
        }

        public CacheableAttribute(CacheBehavior action, string dependencyKey)
        {
            Action = action;
            DependencyKey = dependencyKey;
        }

        public CacheBehavior Action { get; set; }
        public string DependencyKey { get; set; }
    }
}