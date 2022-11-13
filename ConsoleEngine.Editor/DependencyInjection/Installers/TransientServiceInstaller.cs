using ConsoleEngine.Editor.Services;
using DependencyInjection;
using DependencyInjection.Windsor;

namespace ConsoleEngine.Editor.DependencyInjection.Installers
{
    internal sealed class TransientServiceInstaller : BaseInstaller
    {
        public override void Install(IServiceCollection serviceCollection)
        {
            InstallAsTransient<ITransientService>(serviceCollection.GetContainer());
        }
    }
}
