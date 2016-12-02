using Caching.Interface;
using Castle.DynamicProxy;
using Test_Solution1.Common.Attributes;
using log4net;
using System;
using System.Linq;
using CacheBehavior = Test_Solution1.Common.Attributes.CacheableAttribute.CacheBehavior;

namespace Caching
{
    public class CachingInterceptor : ICachingInterceptor
    {
        private readonly ICacheProvider _cacheProvider;
        private readonly ICacheMetaInfoStore _infoStore;

        public CachingInterceptor(ICacheProvider cacheProvider, ICacheMetaInfoStore infoStore)
        {
            _cacheProvider = cacheProvider;
            _infoStore = infoStore;
        }

        private static ILog Log
        {
            get { return LogManager.GetLogger(typeof(CachingInterceptor)); }
        }

        public virtual void Intercept(IInvocation invocation)
        {
            var methodInfo = invocation.MethodInvocationTarget ?? invocation.Method;
            var cacheable = Attribute.GetCustomAttributes(methodInfo, typeof(CacheableAttribute), true).FirstOrDefault();
            var cacheableAttribute = cacheable != null ? cacheable as CacheableAttribute : null;

            // If the method is not marked as Cacheable then proceed and return
            if (cacheableAttribute == null)
            {
                invocation.Proceed();
                return;
            }

            // If the method is marked cacheable for Add, but there is no return type, 
            // there will be nothing to cache.
            if (cacheableAttribute.Action == CacheBehavior.Add && invocation.Method.ReflectedType == typeof(void))
            {
                Log.WarnFormat("Method '{0}' was marked Cacheable Add, but does not have a return type to cache.", methodInfo.Name);
                invocation.Proceed();
                return;
            }

            // TOBE FIXED

            // If the method is marked as a cache Remove, there must be a DependentKey to remove.
            //if (cacheableAttribute.Action == CacheBehavior.Remove && cacheableAttribute.DependencyKey.IsNullOrEmpty())
            //{
            //    Log.WarnFormat("Method '{0}' was marked Cacheable Remove, but does not have a DependentKey to remove.", methodInfo.Name);
            //    invocation.Proceed();
            //    return;
            //}

            // If we get to here, this is either:
            //   1. A cache Add request
            //   2. A cache Remove request with a DependentKey

            if (cacheableAttribute.Action == CacheBehavior.Add)
            {
                Log.DebugFormat("Intercepting {0}.{1} to grab the data from the Cache.", invocation.TargetType.Name,
                                invocation.Method.Name);

                var cacheKey = BuildCacheKeyFrom(invocation);

                //try get the return value from the cache provider
                Log.DebugFormat("Retrieving cached values at: {0}", cacheKey);

                try
                {
                    var item = _cacheProvider.Get(cacheKey);

                    if (item != null)
                    {
                        invocation.ReturnValue = item;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Log.Warn(string.Format("Unable to read key {0} from the cache.", cacheKey), ex);
                }

                Log.DebugFormat("Cached values at: {0} not found, invocation proceed", cacheKey);

                //call the intercepted method
                invocation.Proceed();

                // if data returned, then cache it for next usage
                if (invocation.ReturnValue == null)
                    return;

                var cacheInfo = _infoStore.GetCacheInfo(invocation.TargetType, methodInfo.Name);

                var request = new CacheRequest
                {
                    Key = cacheKey,
                    Value = invocation.ReturnValue,
                    ExpiryInMinutes = cacheInfo.ExpirationInMinutes,
                    ExpirationType = cacheInfo.ExpirationType,
                    DependentKey = cacheableAttribute.DependencyKey,
                };
                Log.DebugFormat("Caching per the following request: {0}", request);

                _cacheProvider.Add(request);
            }
            else
            {
                _cacheProvider.Remove(cacheableAttribute.DependencyKey);
                // invoke the original call
                invocation.Proceed();

                Log.DebugFormat(
                    "{0} action requires items to be removed from cache; removing items dependent on this key: {1}",
                    invocation.Method.Name, cacheableAttribute.DependencyKey);

            }
        }

        private static string BuildCacheKeyFrom(IInvocation invocation)
        {
            var methodName = invocation.Method.Name;
            var arguments = (from a in invocation.Arguments select a.ToString()).ToArray();
            var argsString = string.Join(",", arguments);
            var cacheKey = string.Format("{0}-{1}-{2}", invocation.TargetType.FullName, methodName, argsString);
            return cacheKey;
        }
    }
}