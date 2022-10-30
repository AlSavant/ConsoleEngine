using System;
using System.Collections.Generic;
using DataModel.ComponentModel;
using DataModel.Components;
using DataModel.StaticData.Entity;

namespace DataModel.Entity
{
    public interface IEntity : INotifyPropertyChanged    
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
