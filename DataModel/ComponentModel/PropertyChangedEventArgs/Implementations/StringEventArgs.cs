namespace DataModel.ComponentModel
{
    public struct StringEventArgs : IStringEventArgs
    {
        public string PropertyName { get; private set; }
        public string Text { get; private set; }

        public StringEventArgs(string propertyName, string text)
        {
            PropertyName = propertyName;
            Text = text;
        }
    }
}
