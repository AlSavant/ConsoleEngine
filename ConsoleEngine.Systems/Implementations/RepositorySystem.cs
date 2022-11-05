using ConsoleEngine.Services.Factories;
using ConsoleEngine.Services.Repositories.Entity;
using ConsoleEngine.Services.Util.Events;
using DataModel.ComponentModel;
using System;
using System.Collections.Generic;

namespace ConsoleEngine.Systems.Implementations
{
    internal sealed class RepositorySystem : System, IRepositorySystem
    {        
        private readonly Dictionary<IEntityRepository, Type> repositoryTypeMap;
        private readonly IEntityRepository[] repositoryCollection;

        public RepositorySystem(
            IEntityFactory entityFactory,
            IEntityRepositoryFactory entityRepositoryFactory,
            IPropertyChangedSubscriberService subscriberService)
        {
            var repositories = entityRepositoryFactory.CreateRepositories();           
            repositoryTypeMap = new Dictionary<IEntityRepository, Type>();
            repositoryCollection = new IEntityRepository[repositories.Count];
            int i = 0;
            foreach (var pair in repositories)
            {
                repositoryCollection[i] = pair.Value;
                repositoryTypeMap.Add(pair.Value, pair.Key);
                i++;
            }
            ClearRepositories();
            subscriberService.Subscribe(entityFactory)
                             .AddActionForProperty("EntityCreated", OnEntityCreated)
                             .AddActionForProperty("EntityDestroyed", OnEntityDestroyed);
        }

        public override bool IsAutoRun()
        {
            return true;
        }

        private void OnEntityDestroyed(INotifyPropertyChanged arg1, IPropertyChangedEventArgs e)
        {
            var entity = ((IEntityEventArgs)e).Entity;
            for (int i = 0; i < repositoryCollection.Length; i++)
            {
                var repository = repositoryCollection[i];
                var componentType = repositoryTypeMap[repository];
                if (entity.Components.ContainsKey(componentType))
                {
                    repository.Unregister(entity);
                }
            }
        }

        private void OnEntityCreated(INotifyPropertyChanged obj, IPropertyChangedEventArgs e)
        {
            var entity = ((IEntityEventArgs)e).Entity;
            for (int i = 0; i < repositoryCollection.Length; i++)
            {
                var repository = repositoryCollection[i];
                var componentType = repositoryTypeMap[repository];
                if (entity.Components.ContainsKey(componentType))
                {
                    repository.Register(entity);
                }
            }
        }

        private void ClearRepositories()
        {
            for (int i = 0; i < repositoryCollection.Length; i++)
            {
                repositoryCollection[i].Clear();
            }
        }        
    }
}
