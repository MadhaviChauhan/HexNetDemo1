using Caching.Interface;
using Castle.MicroKernel.Facilities;
using Castle.MicroKernel.Registration;

namespace Caching
{
    public class CacheFacility : AbstractFacility
    {
        protected override void Init()
        {
            Kernel.Register(Component.For<ICachingInterceptor>().ImplementedBy<CachingInterceptor>());
            Kernel.Register(Component.For<ICacheMetaInfoStore>().ImplementedBy<CacheMetaInfoStore>().LifeStyle.Singleton);
            Kernel.ComponentModelBuilder.AddContributor(new CacheComponentInspector());
        }
    }
}