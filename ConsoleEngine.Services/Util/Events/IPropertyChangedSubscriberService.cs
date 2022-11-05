using DataModel.ComponentModel;
using System;

namespace ConsoleEngine.Services.Util.Events
{
    public interface IPropertyChangedSubscriberService : ITransientService
    {
        IPropertyChangedSubscriberService Subscribe(INotifyPropertyChanged broadcaster);
        void UnSubscribe();
        void UnSubscribe(INotifyPropertyChanged broadcaster);
        IPropertyChangedSubscriberService AddActionForProperty(string propertyName, Action<INotifyPropertyChanged, IPropertyChangedEventArgs> action);
        void ExecuteAllPropertyActions();
        void Reset();
    }
}
