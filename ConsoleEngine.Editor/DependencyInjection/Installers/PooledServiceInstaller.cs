using ConsoleEngine.Editor.Services;
using DependencyInjection;
using DependencyInjection.Windsor;

namespace ConsoleEngine.Editor.DependencyInjection.Installers
{
    internal sealed class PooledServiceInstaller : BaseInstaller
    {
        public override void Install(IServiceCollection serviceCollection)
        {
            InstallAsPooled<IPooledService>(serviceCollection.GetContainer());
        }
    }
}
