namespace DataModel.ComponentModel
{
    public interface IByteEventArgs : IPropertyChangedEventArgs
    {
        byte Value { get; }
    }
}
