using DataModel.Math.Structures;
using System.Collections.Generic;

namespace ConsoleEngine.Editor.Model.History
{
    internal sealed class PixelsPaintedState : HistoryState
    {
        public Vector2Int PreviousGridSize { get; private set; }
        public Vector2Int NextGridSize { get; private set; }
        public Dictionary<int, Pixel> PreviousPixels { get; private set; }
        public Dictionary<int, Pixel> NextPixels { get; private set; }

        public PixelsPaintedState(string stateName, Dictionary<int, KeyValuePair<Pixel, Pixel>> modifiedPixels, KeyValuePair<Vector2Int, Vector2Int> gridSize)
            : base(stateName)
        {
            PreviousGridSize = gridSize.Key;
            NextGridSize = gridSize.Value;
            var prev = new Dictionary<int, Pixel>();
            var next = new Dictionary<int, Pixel>();
            foreach (var pair in modifiedPixels)
            {
                prev.Add(pair.Key, pair.Value.Key);
                next.Add(pair.Key, pair.Value.Value);
            }
            PreviousPixels = prev;
            NextPixels = next;
        }
    }
}
