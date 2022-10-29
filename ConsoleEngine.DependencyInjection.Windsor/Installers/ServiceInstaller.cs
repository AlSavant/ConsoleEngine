using Castle.MicroKernel.Registration;
using ConsoleEngine.Services;
using DataModel.Reflection;
using DependencyInjection;
using DependencyInjection.Windsor;

namespace ConsoleEngine.DependencyInjection.Windsor.Installers
{
    internal sealed class ServiceInstaller : BaseInstaller
    {
        public override void Install(IServiceCollection serviceCollection)
        {
            InstallAsSingleton<IService>(
                serviceCollection.GetContainer(),
                Dependency.OnValue<AssemblyPool>(serviceCollection.GetOption<AssemblyPool>("assemblyPool")));
        }
    }
}
