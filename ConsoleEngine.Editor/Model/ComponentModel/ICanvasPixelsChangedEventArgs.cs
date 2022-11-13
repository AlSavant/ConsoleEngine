using DataModel.ComponentModel;

namespace ConsoleEngine.Editor.Model.ComponentModel
{
    internal interface ICanvasPixelsChangedEventArgs : IPropertyChangedEventArgs
    {
        int[] ChangedIndices { get; }
    }
}
