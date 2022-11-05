using DataModel.Entity;
using System.Collections.Generic;

namespace ConsoleEngine.Services.Repositories.Entity.Implementations
{
    internal sealed class CameraRepositoryService : ICameraRepositoryService
    {
        private readonly List<IEntity> entities;

        public CameraRepositoryService()
        {
            entities = new List<IEntity>();
        }

        public int Count
        {
            get
            {
                return entities.Count;
            }
        }

        public void Clear()
        {
            entities.Clear();
        }

        public bool Contains(IEntity entity)
        {
            return entities.Contains(entity);
        }

        public IEntity Get(int index)
        {
            return entities[index];
        }

        public IEnumerable<IEntity> GetCollectionClone()
        {
            return new List<IEntity>(entities);
        }

        public void Register(IEntity entity)
        {
            entities.Add(entity);
        }

        public void Unregister(IEntity entity)
        {
            entities.Remove(entity);
        }
    }
}
