using EntityComponent.Entity;
using EntityComponent.StaticData.Component;

namespace EntityComponent.Components.Implementations
{
    public abstract class Component : IComponent
    {       
        public IEntity Entity { get; set; }
        public abstract IComponentStaticData StaticDataBase { get; set; }
    }

    public abstract class Component<T> : Component, IComponent<T>
        where T : IComponentStaticData
    {
        public override IComponentStaticData StaticDataBase
        {
            get
            {
                return StaticData;
            }
            set
            {
                if (value is T)
                {
                    StaticData = (T)value;
                }

            }
        }

        public T StaticData { get; set; }
    }
}
