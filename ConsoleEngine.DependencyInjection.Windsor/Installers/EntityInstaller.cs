using Castle.MicroKernel.Registration;
using DependencyInjection;
using DependencyInjection.Windsor;
using EntityComponent.Entity;
using EntityComponent.Entity.Implementations;

namespace ConsoleEngine.DependencyInjection.Windsor.Installers
{
    internal sealed class EntityInstaller : BaseInstaller
    {
        public override void Install(IServiceCollection serviceCollection)
        {
            serviceCollection.GetContainer().Register(Component.For<IEntity>().ImplementedBy<Entity>().LifestyleTransient());
        }
    }
}
