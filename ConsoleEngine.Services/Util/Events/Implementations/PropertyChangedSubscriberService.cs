using DataModel.ComponentModel;
using System;
using System.Collections.Generic;

namespace ConsoleEngine.Services.Util.Events.Implementations
{
    internal sealed class PropertyChangedSubscriberService : IPropertyChangedSubscriberService
    {
        private Dictionary<string, HashSet<Action<INotifyPropertyChanged, IPropertyChangedEventArgs>>> propertyActions =
            new Dictionary<string, HashSet<Action<INotifyPropertyChanged, IPropertyChangedEventArgs>>>();

        private HashSet<INotifyPropertyChanged> currentElements = new HashSet<INotifyPropertyChanged>();

        private void OnPropertyChanged(INotifyPropertyChanged sender, IPropertyChangedEventArgs args)
        {
            if (propertyActions.ContainsKey(args.PropertyName))
            {
                ExecuteActions(propertyActions[args.PropertyName], sender, args);
            }
        }


        public IPropertyChangedSubscriberService Subscribe(INotifyPropertyChanged broadcaster)
        {
            if (!currentElements.Contains(broadcaster))
                currentElements.Add(broadcaster);

            broadcaster.PropertyChanged -= OnPropertyChanged;
            broadcaster.PropertyChanged += OnPropertyChanged;

            return this;
        }

        public void UnSubscribe()
        {
            foreach (var element in currentElements)
            {
                element.PropertyChanged -= OnPropertyChanged;
            }


            currentElements.Clear();
            propertyActions.Clear();
        }

        public void UnSubscribe(INotifyPropertyChanged broadcaster)
        {
            if (!currentElements.Contains(broadcaster))
                return;

            broadcaster.PropertyChanged -= OnPropertyChanged;
            currentElements.Remove(broadcaster);
        }


        public IPropertyChangedSubscriberService AddActionForProperty(string propertyName, Action<INotifyPropertyChanged, IPropertyChangedEventArgs> action)
        {
            if (!propertyActions.ContainsKey(propertyName))
            {
                propertyActions.Add(propertyName, new HashSet<Action<INotifyPropertyChanged, IPropertyChangedEventArgs>>());
            }

            if (!propertyActions[propertyName].Contains(action))
                propertyActions[propertyName].Add(action);

            return this;
        }

        public void ExecuteAllPropertyActions()
        {
            foreach (var propertyAction in propertyActions)
            {
                foreach (var action in propertyAction.Value)
                    action?.Invoke(null, null);
            }
        }

        private void ExecuteActions(HashSet<Action<INotifyPropertyChanged, IPropertyChangedEventArgs>> actions,
            INotifyPropertyChanged sender,
            IPropertyChangedEventArgs args)
        {
            foreach (var action in actions)
                action?.Invoke(sender, args);
        }

        public void Reset()
        {
            UnSubscribe();
        }
    }
}
