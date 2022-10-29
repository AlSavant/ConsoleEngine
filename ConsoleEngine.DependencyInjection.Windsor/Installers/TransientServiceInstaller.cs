using ConsoleEngine.Services;
using DependencyInjection;
using DependencyInjection.Windsor;

namespace ConsoleEngine.DependencyInjection.Windsor.Installers
{
    internal sealed class TransientServiceInstaller : BaseInstaller
    {
        public override void Install(IServiceCollection serviceCollection)
        {
            InstallAsTransient<ITransientService>(serviceCollection.GetContainer());
        }
    }
}