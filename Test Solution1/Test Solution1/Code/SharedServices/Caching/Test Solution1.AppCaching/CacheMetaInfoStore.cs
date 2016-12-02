using Caching.Interface;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Caching
{
    public class CacheMetaInfoStore : ICacheMetaInfoStore
    {
        private const string CacheConfigurationSectionName = "cacheConfiguration";
        private const int DefaultCacheExpiryMinutes = 15;
        private const ExpirationType DefaultExpirationType = ExpirationType.Absolute;

        // This dictionary needs to be static and thread-safe.
        private static readonly Dictionary<Type, CacheTypeMetaInfo> TypeMetaInfo = new Dictionary<Type, CacheTypeMetaInfo>();
        private static readonly object Mutex = new object();

        private static ILog Log
        {
            get { return LogManager.GetLogger(typeof(CacheMetaInfoStore)); }
        }

        public CacheMethodMetaInfo GetCacheInfo(Type implementation, string methodName)
        {
            CacheMethodMetaInfo methodDetails;
            lock (Mutex)
            {
                var typeDetails = GetMetaFor(implementation);

                methodDetails = typeDetails.Methods.FirstOrDefault(
                    x => x.MethodName.Equals(methodName, StringComparison.InvariantCultureIgnoreCase));

                // If method details aren't configured, log a warning and cache the default.
                if (methodDetails == null)
                {
                    Log.WarnFormat(
                        "Cache policy was not found for method {0}.{1}. Default caching policy is being used.",
                        implementation.FullName, methodName);
                    methodDetails = DefaultCacheMethodMetaInfo(methodName);
                    TypeMetaInfo[implementation].Methods.Add(methodDetails);
                }
            }
            return methodDetails;
        }

        private CacheMethodMetaInfo DefaultCacheMethodMetaInfo(string methodName)
        {
            return new CacheMethodMetaInfo
            {
                ExpirationInMinutes = DefaultCacheExpiryMinutes,
                MethodName = methodName,
                ExpirationType = DefaultExpirationType,
            };
        }

        private CacheTypeMetaInfo GetMetaFor(Type implementation)
        {
            // Check for information in memory first, if not found, load it from config and add it.
            if (!TypeMetaInfo.ContainsKey(implementation))
            {
                try
                {
                    var configSection = (CacheConfigurationSection)ConfigurationManager.GetSection(CacheConfigurationSectionName);
                    var match = configSection.Types[implementation.FullName];

                    if (match != null)
                    {
                        Log.DebugFormat("Adding CacheTypeMetaInfo for [{0}]", implementation);
                        var newInfo = new CacheTypeMetaInfo { Type = implementation };
                        for (var iMethod = 0; iMethod < match.Methods.Count; iMethod++)
                        {
                            var element = match.Methods[iMethod];
                            ExpirationType expirationType;
                            Enum.TryParse(element.ExpirationType, out expirationType);
                            var newMethod = new CacheMethodMetaInfo
                            {
                                MethodName = element.Name,
                                ExpirationInMinutes = element.ExpirationValue,
                                ExpirationType = expirationType,
                            };
                            newInfo.Methods.Add(newMethod);
                            Log.DebugFormat("Adding CacheMethodMetaInfo: {0}", newMethod);
                        }
                        TypeMetaInfo[implementation] = newInfo;
                        return newInfo;
                    }
                    Log.WarnFormat(
                        "Cache policy was not found for type {0}. Review cache.config and verify the configured type policy matches the implementation.",
                        implementation.FullName);
                }
                catch (Exception ex)
                {
                    Log.Error(
                        string.Format("Exception encountered trying to read cache policy for {0}.", implementation),
                        ex);
                }
                // Cache empty placeholder so we don't keep hitting the config.
                TypeMetaInfo[implementation] = new CacheTypeMetaInfo { Type = implementation };
            }
            return TypeMetaInfo[implementation];
        }
    }
}