using Castle.MicroKernel.Registration;
using ConsoleEngine.Systems;
using DataModel.Reflection;
using DependencyInjection;
using DependencyInjection.Windsor;

namespace ConsoleEngine.DependencyInjection.Windsor.Installers
{
    internal sealed class SystemInstaller : BaseInstaller
    {
        public override void Install(IServiceCollection serviceCollection)
        {
            InstallAsSingleton<ISystem>(
                serviceCollection.GetContainer(),
                Dependency.OnValue<AssemblyPool>(serviceCollection.GetOption<AssemblyPool>("assemblyPool")));
        }
    }
}
