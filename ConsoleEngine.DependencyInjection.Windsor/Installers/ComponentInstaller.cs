using DependencyInjection;
using DependencyInjection.Windsor;
using EntityComponent.Components;

namespace ConsoleEngine.DependencyInjection.Windsor.Installers
{
    internal sealed class ComponentInstaller : BaseInstaller
    {
        public override void Install(IServiceCollection serviceCollection)
        {
            InstallAsTransient<IComponent>(serviceCollection.GetContainer());
        }
    }
}
