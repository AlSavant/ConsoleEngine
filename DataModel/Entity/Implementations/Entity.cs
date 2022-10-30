using DataModel.ComponentModel;
using DataModel.Components;
using DataModel.StaticData.Entity;
using System;
using System.Collections.Generic;

namespace DataModel.Entity.Implementations
{
    public sealed class Entity : IEntity
    {
        public Action<INotifyPropertyChanged, IPropertyChangedEventArgs> PropertyChanged { get; set; }

        public IEntityStaticData StaticData { get; set; }
        public Dictionary<Type, IComponent> Components { get; set; }

        public Entity()
        {
            Components = new Dictionary<Type, IComponent>();
        }

        public void AddComponent(Type type, IComponent component)
        {
            Components.Add(type, component);
        }

        public T GetComponent<T>() where T : IComponent
        {
            return (T)Components[typeof(T)];
        }

        public bool HasComponent<T>() where T : IComponent
        {
            return Components.ContainsKey(typeof(T));
        }

        public object GetFirstComponentOfType(Type type)
        {
            foreach (var component in Components)
            {
                var componentType = component.Value.GetType();
                if (componentType.IsAssignableFrom(type) ||
                    type.IsAssignableFrom(componentType))
                    return component.Value;
            }
            return null;
        }

        private bool isNull;
        private bool IsNull
        {
            get
            {
                return isNull;
            }
            set
            {
                if (isNull != value)
                {
                    isNull = value;
                    this.Broadcast(new PropertyChangedEventArgs("IsNull"));
                }
            }
        }
        public void Destroy()
        {
            if (IsNull)
                return;
            var components = new List<IComponent>(Components.Values);
            foreach (var component in components)
            {
                component.PropertyChanged = null;
            }
            IsNull = true;
            PropertyChanged = null;
        }

        #region Disposable
        public static bool operator ==(Entity x, Entity y)
        {
            if (ReferenceEquals(x, y))
                return true;
            if (ReferenceEquals(x, null))
            {
                if (ReferenceEquals(y, null))
                {
                    return true;
                }
                else
                {
                    return y.IsNull;
                }
            }
            if (ReferenceEquals(y, null))
            {
                if (ReferenceEquals(x, null))
                {
                    return true;
                }
                else
                {
                    return x.IsNull;
                }
            }
            return x.Equals(y);
        }

        public static bool operator !=(Entity x, Entity y)
        {
            return !(x == y);
        }

        public override bool Equals(object obj)
        {
            if (obj == null && IsNull)
                return true;
            if (obj == null && !IsNull)
                return false;
            return ReferenceEquals(obj, this);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }
}
