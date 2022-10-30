using Castle.Windsor;
using ConsoleEngine.Services;
using ConsoleEngine.Systems;
using DataModel.Reflection;
using DependencyInjection;
using DependencyInjection.Windsor;
using DataModel.Components;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleEngine.DependencyInjection.Windsor
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
        {
            var touple = GetInstallers(serviceCollection);
            var installers = touple.Item1;
            var installPool = touple.Item2;
            if (installers.Length <= 0)
                return serviceCollection;
            for (int i = 0; i < installers.Length; i++)
            {
                var installer = (BaseInstaller)Activator.CreateInstance(installers[i]);
                installer.LoadAssembly(installPool, installPool);
                installer.Install(serviceCollection);
            }
            return serviceCollection;
        }

        public static async Task<IServiceCollection> AddServicesAsync(this IServiceCollection serviceCollection)
        {
            var touple = GetInstallers(serviceCollection);
            var installers = touple.Item1;
            var installPool = touple.Item2;
            if (installers.Length <= 0)
                return serviceCollection;
            var tasks = new Task[installers.Length];
            for (int i = 0; i < installers.Length; i++)
            {
                var installer = (BaseInstaller)Activator.CreateInstance(installers[i]);
                tasks[i] = Task.Run(() => {
                    installer.LoadAssembly(installPool, installPool);
                    installer.Install(serviceCollection);
                });
            }
            await Task.WhenAll(tasks);
            return serviceCollection;
        }

        private static (Type[], AssemblyPool) GetInstallers(IServiceCollection serviceCollection)
        {
            var collection = serviceCollection as ServiceCollection;
            if (collection == null)
            {
                throw new InvalidOperationException("Incompatible ServiceCollection Provider.");
            }
            var container = collection.GetContainer();

            var assemblyPool = serviceCollection.GetOption<AssemblyPool>("assemblyPool");
            var coreAssembly = typeof(ServiceExtensions).Assembly;
            var installPool = new AssemblyPool();
            installPool.LoadAssemblies(
                typeof(IComponent).Assembly,
                typeof(IService).Assembly,
                typeof(ISystem).Assembly);
            assemblyPool.LoadAssemblies(installPool);
            var installers = typeof(ServiceExtensions).Assembly.GetTypes().
                Where(
                    x =>
                    !x.IsInterface &&
                    !x.IsAbstract &&
                    typeof(BaseInstaller).IsAssignableFrom(x)
                ).ToArray();
            return (installers, installPool);
        }

        public static IWindsorContainer GetContainer(this IServiceCollection serviceCollection)
        {
            return ((ServiceCollection)serviceCollection).GetContainer();
        }
    }
}
