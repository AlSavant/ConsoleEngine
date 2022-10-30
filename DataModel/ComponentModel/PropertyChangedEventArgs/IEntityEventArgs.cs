using DataModel.Entity;

namespace DataModel.ComponentModel
{
    public interface IEntityEventArgs : IPropertyChangedEventArgs
    {
        IEntity Entity { get; }
    }
}
