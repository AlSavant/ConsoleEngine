using DataModel.Entity;

namespace DataModel.ComponentModel
{
    public struct EntityEventArgs : IEntityEventArgs
    {
        public IEntity Entity { get; private set; }
        public string PropertyName { get; private set; }

        public EntityEventArgs(string propertyName, IEntity entity)
        {
            Entity = entity;
            PropertyName = propertyName;
        }
    }
}
