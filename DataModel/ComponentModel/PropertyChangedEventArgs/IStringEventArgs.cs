namespace DataModel.ComponentModel
{
    public interface IStringEventArgs : IPropertyChangedEventArgs
    {
        string Text { get; }
    }
}
