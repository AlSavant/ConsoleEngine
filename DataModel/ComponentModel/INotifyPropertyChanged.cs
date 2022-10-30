using System;

namespace DataModel.ComponentModel
{
    public interface INotifyPropertyChanged
    {
        Action<INotifyPropertyChanged, IPropertyChangedEventArgs> PropertyChanged { get; set; }
    }
}
