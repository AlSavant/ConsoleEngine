namespace DataModel.ComponentModel
{
    public interface IBoolEventArgs : IPropertyChangedEventArgs
    {
        bool Value { get; }
    }
}
