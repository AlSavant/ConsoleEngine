using DependencyInjection;
using DependencyInjection.Windsor;
using SpriteEditor.DependencyInjection;
using SpriteEditor.ViewModels;

namespace ConsoleEngine.DependencyInjection.Windsor.Installers
{
    internal sealed class ViewModelInstaller : BaseInstaller
    {
        public override void Install(IServiceCollection serviceCollection)
        {
            InstallAsSingleton<IViewModel>(serviceCollection.GetContainer());
        }
    }
}
