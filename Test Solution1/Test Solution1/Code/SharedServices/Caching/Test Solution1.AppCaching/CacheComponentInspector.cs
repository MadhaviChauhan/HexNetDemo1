using Caching.Interface;
using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.ModelBuilder.Inspectors;

namespace Caching
{
    public class CacheComponentInspector : MethodMetaInspector
    {
        protected override string ObtainNodeName()
        {
            return "cache";
        }

        public override void ProcessModel(IKernel kernel, ComponentModel model)
        {
            if (typeof(ICacheable).IsAssignableFrom(model.Implementation))
            {
                var interceptor = kernel.Resolve<ICachingInterceptor>();
                if (null != interceptor)
                    model.Interceptors.AddFirst(new InterceptorReference(interceptor.GetType()));
            }
            base.ProcessModel(kernel, model);
        }
    }
}