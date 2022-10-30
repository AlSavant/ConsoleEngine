namespace DataModel.ComponentModel
{
    public struct ByteEventArgs : IByteEventArgs
    {
        public string PropertyName { get; private set; }
        public byte Value { get; private set; }

        public ByteEventArgs(string propertyName, byte value)
        {
            PropertyName = propertyName;
            Value = value;
        }
    }
}
