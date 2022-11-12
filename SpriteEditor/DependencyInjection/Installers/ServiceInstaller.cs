using DependencyInjection;
using DependencyInjection.Windsor;
using SpriteEditor.DependencyInjection;
using SpriteEditor.Services;

namespace ConsoleEngine.DependencyInjection.Windsor.Installers
{
    internal sealed class ServiceInstaller : BaseInstaller
    {
        public override void Install(IServiceCollection serviceCollection)
        {
            InstallAsSingleton<IService>(serviceCollection.GetContainer());
        }
    }
}
