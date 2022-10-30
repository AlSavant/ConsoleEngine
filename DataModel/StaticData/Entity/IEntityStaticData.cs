using DataModel.StaticData.Component;
using System;

namespace DataModel.StaticData.Entity
{
    public interface IEntityStaticData
    {
        string ID { get; set; }
        string DisplayName { get; set; }        
        bool DontDestroyOnSceneLoad { get; }
        IComponentStaticData[] GetComponents();
        IComponentStaticData GetComponent<T>() where T : IComponentStaticData;
        IComponentStaticData GetComponent(Type type);
        bool HasComponent(Type type);        
    }
}
