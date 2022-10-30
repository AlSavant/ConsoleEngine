using DataModel.StaticData.Component;
using DataModel.StaticData.Component.Implementations;
using System;
using System.Collections.Generic;

namespace DataModel.StaticData.Entity.Implementations
{
    public sealed class EntityStaticData : IEntityStaticData
    {
        public string ID { get; set; }
        
        public string displayName;
        public string DisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(displayName) || string.IsNullOrWhiteSpace(displayName))
                {
                    return "Missing Name";
                }
                return displayName;
            }
            set
            {
                displayName = value;
            }
        }        

        public bool dontDestroyOnSceneLoad;
        public bool DontDestroyOnSceneLoad { get { return dontDestroyOnSceneLoad; } }


        public List<ComponentStaticData> components;

        public IComponentStaticData[] GetComponents()
        {
            var arr = new IComponentStaticData[components.Count];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = components[i];
            }
            return arr;
        }

        public IComponentStaticData GetComponent<T>() where T : IComponentStaticData
        {
            if (components == null || components.Count <= 0)
                return default;
            foreach (var component in components)
            {
                if (component is T)
                {
                    return component;
                }
            }
            return default;
        }

        public IComponentStaticData GetComponent(Type type)
        {
            if (components == null || components.Count <= 0)
                return default;
            foreach (var component in components)
            {
                var compType = component.GetType();
                if (compType.IsAssignableFrom(type) || type.IsAssignableFrom(compType))
                {
                    return component;
                }
            }
            return default;
        }

        public bool HasComponent(Type type)
        {
            if (components == null || components.Count <= 0)
                return false;
            foreach (var component in components)
            {
                try
                {
                    if (type.IsAssignableFrom(component.GetType()))
                    {
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }        
    }
}
