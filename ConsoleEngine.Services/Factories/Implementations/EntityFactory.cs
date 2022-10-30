using ConsoleEngine.Services.Util.Entity;
using DataModel.Attributes;
using DataModel.ComponentModel;
using DataModel.Components;
using DataModel.Components.Implementations;
using DataModel.Entity;
using DataModel.Reflection;
using DataModel.StaticData.Component;
using DataModel.StaticData.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using IServiceProvider = DependencyInjection.IServiceProvider;

namespace ConsoleEngine.Services.Factories.Implementations
{
    internal sealed class EntityFactory : IEntityFactory
    {
        public Action<INotifyPropertyChanged, IPropertyChangedEventArgs> PropertyChanged { get; set; }

        private List<IEntity> entityPool;

        private readonly IServiceProvider serviceProvider;
        private readonly IUIDService uidService;

        private readonly Dictionary<string, IEntityStaticData> entityStaticData;
        private readonly Dictionary<Type, Type> staticDataToComponentTable;

        public EntityFactory(
            IServiceProvider serviceProvider,   
            IUIDService uidService,
            IStaticDataFactory staticDataFactory,
            AssemblyPool assemblyPool)
        {
            this.serviceProvider = serviceProvider;
            this.uidService = uidService;            
            entityPool = new List<IEntity>();
            entityStaticData = new Dictionary<string, IEntityStaticData>();
            staticDataToComponentTable = new Dictionary<Type, Type>();
            
            var componentTypes = assemblyPool.GetTypes().Where(x =>
            !x.IsAbstract && typeof(Component).IsAssignableFrom(x));
            var staticDataComp = assemblyPool.GetTypes().Where(x =>
            !x.IsAbstract && typeof(IComponentStaticData).IsAssignableFrom(x));
            var l = staticDataComp.ToList();
            foreach (var componentType in componentTypes)
            {
                if (componentType.BaseType == null)
                    continue;
                var args = componentType.BaseType.GetGenericArguments();
                if (args.Length <= 0)
                    continue;
                var staticType = componentType.BaseType.GetGenericArguments()[0];
                if (staticDataToComponentTable.ContainsKey(staticType))
                    continue;
                var compInterf = componentType.GetInterfaces().FirstOrDefault(x => !x.IsGenericType && x != typeof(IComponent) && typeof(IComponent).IsAssignableFrom(x));
                if (compInterf == null)
                    continue;
                staticDataToComponentTable.Add(staticType, compInterf);                
            }

            var data = staticDataFactory.GetEntityStaticData();
            foreach (var entry in data)
            {
                AddStaticData(entry.ID, entry);
            }
            this.Broadcast("InitializationComplete");
        }

        public bool ContainsEntityID(string id)
        {
            return entityStaticData.ContainsKey(id);
        }

        public void AddStaticData(string id, IEntityStaticData data)
        {
            //Populate General Pool
            entityStaticData.Add(id, data);
            PropertyChanged?.Invoke(this, new StringEventArgs("StaticData", id));
        }

        public IEnumerable<string> GetStaticDataIDs()
        {
            return entityStaticData.Keys;
        }

        public IEntityStaticData GetData(string id)
        {
            if (!entityStaticData.ContainsKey(id))
                return null;
            return entityStaticData[id];
        }

        public IEntity CreateInstance(string staticDataID, Vector2 position, float rotation, string scene, int id = -1)
        {
            var entity = CreateInstance(staticDataID);
            var networkComponenet = entity.GetComponent<IUIDComponent>();
            var transformComponent = entity.GetComponent<ITransformComponent>();

            transformComponent.Position = position;
            transformComponent.Rotation = rotation;
            if (id >= 0)
            {
                networkComponenet.ID = id;
                uidService.RegisterUIDForEntity(entity, id);
            }
            else
            {
                networkComponenet.ID = uidService.GetUID(entity);
            }            
            PropertyChanged?.Invoke
                (this,
                new EntityEventArgs(
                    "EntityCreated",
                    entity));
            return entity;
        }

        public IEntity CreateInstance(string staticDataID)
        {
            var entity = serviceProvider.Resolve<IEntity>();
            var data = entityStaticData[staticDataID];
            entity.StaticData = data;
            foreach (var comp in data.GetComponents())
            {
                try
                {
                    var interf = comp.GetType().GetInterfaces().FirstOrDefault(x => x != typeof(IComponentStaticData) && typeof(IComponentStaticData).IsAssignableFrom(x));
                    if (interf == null)
                        continue;
                    var compInterf = staticDataToComponentTable[interf];
                    var component = (IComponent)serviceProvider.Resolve(compInterf, comp);
                    entity.AddComponent(compInterf, component);
                    component.Entity = entity;
                    component.StaticDataBase = comp;
                    var autoInitProperties = component.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(x => x.GetCustomAttribute<InitializeFromStaticDataAttribute>() != null);
                    foreach (var prop in autoInitProperties)
                    {
                        var propertyName = prop.GetCustomAttribute<InitializeFromStaticDataAttribute>().property;
                        var staticProperty = comp.GetType().GetProperty(propertyName);
                        if (staticProperty == null)
                            continue;
                        if (prop.PropertyType != staticProperty.PropertyType)
                        {
                            throw new InvalidCastException($"Could not cast Property Type to static data Property Type. Type {prop.PropertyType} does not match Type {staticProperty.PropertyType}.");
                        }
                        prop.SetValue(component, staticProperty.GetValue(comp));
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Could not initialize component {comp} for entity {staticDataID}.\n{ex.ToString()}");
                }
            }
            PropertyChanged?.Invoke
                (this,
                new EntityEventArgs(
                    "EntityCreated",
                    entity));
            entity.PropertyChanged += OnEntityDestroyed;
            entityPool.Add(entity);
            return entity;
        }

        private void OnEntityDestroyed(INotifyPropertyChanged sender, IPropertyChangedEventArgs args)
        {
            if (args.PropertyName != "IsNull")
                return;
            var entity = (IEntity)sender;
            if (!entity.IsNull())
                return;            
            entity.PropertyChanged = null;
            foreach (var comp in entity.Components.Values)
            {
                serviceProvider.ReleaseComponent(comp);
            }
            serviceProvider.ReleaseComponent(entity);
            entityPool.Remove(entity);
            PropertyChanged?.Invoke
                (this,
                new EntityEventArgs(
                    "EntityDestroyed",
                    entity));
        }

        public void ReleaseAllEntities()
        {
            for (int i = entityPool.Count - 1; i >= 0; i--)
            {
                if (!entityPool[i].StaticData.DontDestroyOnSceneLoad)
                    entityPool[i].Destroy();
            }
        }
    }
}
