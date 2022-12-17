using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace ConsoleEngine.Editor.Model
{
    internal sealed class SmartCollection<T> : ObservableCollection<T>
    {
        public SmartCollection()
            : base()
        {
        }

        public SmartCollection(IEnumerable<T> collection)
            : base(collection)
        {
        }

        public SmartCollection(List<T> list)
            : base(list)
        {
        }

        public void AddRange(IEnumerable<T> range)
        {
            foreach (var item in range)
            {
                Items.Add(item);
            }

            this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
            this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public void TrimEnd(int count)
        {
            for(int i  = 0; i < count; i++)
            {
                if (Items.Count == 0)
                    return;
                Items.RemoveAt(Items.Count - 1);
            }
            this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
            this.OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
        }

        public void Reset(IEnumerable<T> range)
        {
            this.Items.Clear();

            AddRange(range);
        }
    }
}
