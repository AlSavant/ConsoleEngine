using System.Collections.Generic;

namespace ConsoleEngine.Editor.Model.History
{
    internal sealed class CanvasState : HistoryState
    {
        public Dictionary<int, KeyValuePair<Pixel, Pixel>> modifiedPixels;

        public CanvasState(string stateName, Dictionary<int, KeyValuePair<Pixel, Pixel>> modifiedPixels)
            : base(stateName)
        {
            this.modifiedPixels = modifiedPixels;
        }
    }
}
