using Castle.MicroKernel.Registration;
using Castle.Windsor;
using DataModel.Reflection;
using System.Linq;

namespace DependencyInjection.Windsor
{
    public abstract class BaseInstaller
    {
        private enum ELifestyle
        {
            Transient,
            Singleton,
            Pooled
        }

        private AssemblyPool interfacePool;
        private AssemblyPool implementationPool;

        public void LoadAssembly(AssemblyPool interfacePool, AssemblyPool implementationPool)
        {
            this.interfacePool = interfacePool;
            this.implementationPool = implementationPool;
        }

        public abstract void Install(IServiceCollection serviceCollection);

        protected void InstallAsSingleton<T>(IWindsorContainer container, params Dependency[] dependencies)
        {
            Install<T>(container, ELifestyle.Singleton, dependencies);
        }

        protected void InstallAsTransient<T>(IWindsorContainer container, params Dependency[] dependencies)
        {
            Install<T>(container, ELifestyle.Transient, dependencies);
        }

        protected void InstallAsPooled<T>(IWindsorContainer container, params Dependency[] dependencies)
        {
            Install<T>(container, ELifestyle.Pooled, dependencies);
        }

        private void Install<T>(IWindsorContainer container, ELifestyle lifestyle, params Dependency[] dependencies)
        {
            var baseType = typeof(T);

            var interfaces =
                interfacePool.
                GetTypes().
                Where(x => x != baseType && x.IsInterface && !x.IsGenericType && baseType.
                IsAssignableFrom(x));

            foreach (var type in interfaces)
            {
                if (container.Kernel.HasComponent(type))
                {
                    continue;
                }

                var targetName = type.Name;
                if (targetName.ToLower().StartsWith("i"))
                {
                    targetName = targetName.Substring(1);
                }

                var targetType = implementationPool.GetTypes().
                    FirstOrDefault(x =>
                    x.Name == targetName &&
                    !x.IsInterface &&
                    !x.IsGenericType &&
                    !x.IsAbstract &&
                    type.IsAssignableFrom(x));

                if (targetType == null)
                {
                    continue;
                }

                try
                {
                    if (lifestyle == ELifestyle.Transient)
                    {
                        if (dependencies == null)
                            container.Register(Component.For(type).ImplementedBy(targetType).LifestyleTransient().PropertiesIgnore((model, propertyInfo) => true));
                        else
                            container.Register(Component.For(type).ImplementedBy(targetType).DependsOn(dependencies).LifestyleTransient().PropertiesIgnore((model, propertyInfo) => true));
                    }
                    else if (lifestyle == ELifestyle.Singleton)
                    {
                        if (dependencies == null)
                            container.Register(Component.For(type).ImplementedBy(targetType).LifestyleSingleton().PropertiesIgnore((model, propertyInfo) => true));
                        else
                            container.Register(Component.For(type).ImplementedBy(targetType).DependsOn(dependencies).LifestyleSingleton().PropertiesIgnore((model, propertyInfo) => true));
                    }
                    else if (lifestyle == ELifestyle.Pooled)
                    {
                        if (dependencies == null)
                            container.Register(Component.For(type).ImplementedBy(targetType).LifestylePooled().PropertiesIgnore((model, propertyInfo) => true));
                        else
                            container.Register(Component.For(type).ImplementedBy(targetType).DependsOn(dependencies).LifestylePooled().PropertiesIgnore((model, propertyInfo) => true));
                    }
                }
                catch
                {
                    continue;
                }
            }
        }
    }
}
