using DataModel.ComponentModel;
using DataModel.Entity;
using DataModel.StaticData.Component;

namespace DataModel.Components
{
    public interface IComponent : INotifyPropertyChanged
    {
        IComponentStaticData StaticDataBase { get; set; }
        IEntity Entity { get; set; }
    }

    public interface IComponent<T> : IComponent
        where T : IComponentStaticData
    {
        T StaticData { get; set; }
    }
}
