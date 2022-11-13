using Castle.MicroKernel;
using System;

namespace DependencyInjection.Windsor
{
    public sealed class ServiceProvider : IServiceProvider
    {
        private IKernel kernel;

        public ServiceProvider(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public void ReleaseComponent(object component)
        {
            kernel.ReleaseComponent(component);
        }

        public object Resolve(Type type)
        {            
            return kernel.Resolve(type);
        }

        public object Resolve(Type type, object argument)
        {
            return kernel.Resolve(type, argument);
        }

        public bool HasComponent(Type type)
        {
            return kernel.HasComponent(type);
        }

        public T Resolve<T>()
        {
            return kernel.Resolve<T>();
        }
    }
}
