using Castle.Windsor;
using DataModel.Reflection;
using System.Collections.Generic;
using System.Reflection;

namespace DependencyInjection.Windsor
{
    public sealed class ServiceCollection : IServiceCollection
    {
        private readonly WindsorContainer windsorContainer;
        private readonly Dictionary<string, object> options;

        public ServiceCollection()
        {
            options = new Dictionary<string, object>();
            var assemblyPool = new AssemblyPool();
            assemblyPool.LoadAssemblies(Assembly.GetCallingAssembly(), Assembly.GetExecutingAssembly());
            options.Add("assemblyPool", assemblyPool);
            windsorContainer = new WindsorContainer();
            windsorContainer.Register(Castle.MicroKernel.Registration.Component.
                For(typeof(IServiceProvider)).
                ImplementedBy(typeof(ServiceProvider)).LifeStyle.Singleton);
        }

        public WindsorContainer GetContainer()
        {
            return windsorContainer;
        }

        public IServiceProvider BuildProvider()
        {
            return windsorContainer.Resolve<IServiceProvider>();
        }

        public T GetOption<T>(string key)
        {
            if (!options.ContainsKey(key))
                return default;
            return (T)options[key];
        }

        public object GetOption(string key)
        {
            if (!options.ContainsKey(key))
                return default;
            return options[key];
        }

        public IServiceCollection AddOption(string key, object value)
        {
            options[key] = value;
            return this;
        }
    }
}
