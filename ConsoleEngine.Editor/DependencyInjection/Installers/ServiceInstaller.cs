using ConsoleEngine.Editor.Services;
using DependencyInjection;
using DependencyInjection.Windsor;

namespace ConsoleEngine.Editor.DependencyInjection.Installers
{
    internal sealed class ServiceInstaller : BaseInstaller
    {
        public override void Install(IServiceCollection serviceCollection)
        {
            InstallAsSingleton<IService>(serviceCollection.GetContainer());
        }
    }
}
