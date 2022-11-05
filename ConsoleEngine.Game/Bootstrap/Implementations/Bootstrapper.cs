using Castle.MicroKernel.Registration;
using ConsoleEngine.DependencyInjection.Windsor;
using DependencyInjection.Windsor;

namespace ConsoleEngine.Game.Bootstrap.Implementations
{
    internal class Bootstrapper
    {
        public ICompositionRoot BootstrapCompositionRoot()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.GetContainer().Register(
                Component.For<ICompositionRoot>()
                .ImplementedBy<CompositionRoot>()
                .LifestyleSingleton()
                .PropertiesIgnore((model, propertyInfo) => true));

            var provider = serviceCollection
                .AddServices()
                .BuildProvider();

            return provider.Resolve<ICompositionRoot>();
        }
    }
}
