using System;

namespace DependencyInjection
{
    public interface IServiceProvider
    {
        object Resolve(Type type);
        object Resolve(Type type, object arguments);
        T Resolve<T>();
        bool HasComponent(Type type);
        void ReleaseComponent(object component);
    }
}
