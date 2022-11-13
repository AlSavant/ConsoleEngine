namespace ConsoleEngine.Editor.Model.ComponentModel.Implementations
{
    internal struct CanvasPixelsChangedEventArgs : ICanvasPixelsChangedEventArgs
    {
        public int[] ChangedIndices { get; private set; }
        public string PropertyName { get; private set; }

        public CanvasPixelsChangedEventArgs(string propertyName, int[] changedIndices)
        {
            PropertyName = propertyName;
            ChangedIndices = changedIndices;
        }
    }
}
