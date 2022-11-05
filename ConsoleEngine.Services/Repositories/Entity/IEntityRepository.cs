using DataModel.Components;
using DataModel.Entity;
using System.Collections.Generic;

namespace ConsoleEngine.Services.Repositories.Entity
{
    public interface IEntityRepository
    {
        int Count { get; }
        void Clear();
        bool Contains(IEntity entity);
        IEntity Get(int index);
        IEnumerable<IEntity> GetCollectionClone();
        void Register(IEntity entity);
        void Unregister(IEntity entity);
    }

    public interface IEntityRepository<T> : IEntityRepository where T : IComponent { }
}
