using DependencyInjection;
using DependencyInjection.Windsor;
using DataModel.Components;

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
