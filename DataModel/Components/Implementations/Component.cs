using DataModel.ComponentModel;
using DataModel.Entity;
using DataModel.StaticData.Component;
using System;

namespace DataModel.Components.Implementations
{
    public abstract class Component : IComponent
    {
        public Action<INotifyPropertyChanged, IPropertyChangedEventArgs> PropertyChanged { get; set; }
        public IEntity Entity { get; set; }
        public abstract IComponentStaticData StaticDataBase { get; set; }
    }

    public abstract class Component<T> : Component, IComponent<T>
        where T : IComponentStaticData
    {
        public override IComponentStaticData StaticDataBase
        {
            get
            {
                return StaticData;
            }
            set
            {
                if (value is T)
                {
                    StaticData = (T)value;
                }

            }
        }

        public T StaticData { get; set; }
    }
}
