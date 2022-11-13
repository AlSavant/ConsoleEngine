using ConsoleEngine.Editor.ViewModels;
using DependencyInjection;
using DependencyInjection.Windsor;

namespace ConsoleEngine.Editor.DependencyInjection.Installers
{
    internal sealed class ViewModelInstaller : BaseInstaller
    {
        public override void Install(IServiceCollection serviceCollection)
        {
            InstallAsSingleton<IViewModel>(serviceCollection.GetContainer());
        }
    }
}
