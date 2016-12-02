using Caching.Interface;
using log4net;
using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace Caching
{
    public class NetCacheProvider : ICacheProvider
    {
        protected MemoryCache _dataCache = new MemoryCache("CachingProvider");

        private static ILog Log
        {
            get { return LogManager.GetLogger(typeof(NetCacheProvider)); }
        }


        public object Get(string key)
        {
            return _dataCache.Get(key);
        }

        public void Add(CacheRequest request)
        {
            int timeout;

            if (request.ExpirationType == ExpirationType.Sliding)
                timeout = request.ExpiryInMinutes;
            else
                timeout = request.ExpiryInMinutes;
            //    timeout = DateTimeOffset.UtcNow.AddMinutes(request.ExpiryInMinutes) - DateTime.UtcNow;

            if (request.Value == null) return;

            try
            {
                Insert(request, timeout);
            }
            catch (Exception ex)
            {

                // temporal failure, ignore and continue
                Log.DebugFormat("Exception encountered  {0} trying to insert of '{1}'", ex.Message, request.Key);


                throw;
            }
        }

        public void Insert(CacheRequest request, int timeout)
        {

            var dependentValue = new List<string>();

            ObjectCache cache = MemoryCache.Default;
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration =
                DateTimeOffset.Now.AddMinutes(timeout);



            if (!string.IsNullOrEmpty(request.DependentKey))
            {
                if (_dataCache.Get(request.DependentKey) == null)
                {
                    Log.DebugFormat("DependentKey '{0}' was not found for insert of '{1}'. Inserting a new object for the DependentKey", request.DependentKey, request.Key);
                    dependentValue.Add(request.Key);
                    _dataCache.Add(request.DependentKey, dependentValue, policy);
                }
                else
                {
                    dependentValue = (List<string>)_dataCache.Get(request.DependentKey);
                    if (dependentValue.Contains(request.Key) == false)
                    {
                        dependentValue.Add(request.Key);
                        _dataCache.Add(request.DependentKey, dependentValue, policy);
                    }
                }
            }

            _dataCache.Add(request.Key, request.Value, policy);
        }

        public void Remove(string dependentkey)
        {
            var dependentKeys = (List<string>)_dataCache.Get(dependentkey);

            //On Cache removal request all dependent cached objects will also be removed
            if (dependentKeys != null)
            {
                Log.DebugFormat("Dependent cache '{0}' found. Removing dependent objects {1}.", dependentkey, dependentKeys);
                dependentKeys.ForEach(x => _dataCache.Remove(x));
                _dataCache.Remove(dependentkey);
            }
        }


    }
}
