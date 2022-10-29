using ConsoleEngine.Services;
using DependencyInjection;
using DependencyInjection.Windsor;

namespace ConsoleEngine.DependencyInjection.Windsor.Installers
{
    internal sealed class PooledServiceInstaller : BaseInstaller
    {
        public override void Install(IServiceCollection serviceCollection)
        {
            InstallAsPooled<IPooledService>(serviceCollection.GetContainer());
        }
    }
}
