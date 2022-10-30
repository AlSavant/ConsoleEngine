namespace DataModel.ComponentModel
{
    public struct PropertyChangedEventArgs : IPropertyChangedEventArgs
    {
        public string PropertyName { get; private set; }

        public PropertyChangedEventArgs(string propertyName)
        {
            PropertyName = propertyName;
        }
    }
}
