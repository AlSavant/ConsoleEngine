using EntityComponent.Entity;
using EntityComponent.StaticData.Component;

namespace EntityComponent.Components
{
    public interface IComponent
    {
        IComponentStaticData StaticDataBase { get; set; }
        IEntity Entity { get; set; }
    }

    public interface IComponent<T> : IComponent
        where T : IComponentStaticData
    {
        T StaticData { get; set; }
    }
}
