namespace DataModel.ComponentModel
{
    public struct BoolEventArgs : IBoolEventArgs
    {
        public string PropertyName { get; private set; }
        public bool Value { get; private set; }

        public BoolEventArgs(string propertyName, bool value)
        {
            PropertyName = propertyName;
            Value = value;
        }
    }
}
