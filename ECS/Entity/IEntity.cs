using System;
using System.Collections.Generic;
using EntityComponent.Components;
using EntityComponent.StaticData.Entity;

namespace EntityComponent.Entity
{
    public interface IEntity
    {
        IEntityStaticData StaticData { get; set; }
        Dictionary<Type, IComponent> Components { get; set; }
        void AddComponent(Type type, IComponent component);
        T GetComponent<T>() where T : IComponent;
        bool HasComponent<T>() where T : IComponent;
        object GetFirstComponentOfType(Type type);
        void Destroy();
    }
}
