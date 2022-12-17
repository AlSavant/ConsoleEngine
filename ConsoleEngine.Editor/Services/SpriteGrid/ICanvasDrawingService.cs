using ConsoleEngine.Editor.Model;
using DataModel.ComponentModel;
using DataModel.Math.Structures;
using System.Collections.Generic;

namespace ConsoleEngine.Editor.Services.SpriteGrid
{
    internal interface ICanvasDrawingService : IService, INotifyPropertyChanged
    {
        Pixel Get(int i);
        int PixelCount { get; }
        void SetPixel(int index, char character, ColorEntry colorEntry);
        void SetPixels(Dictionary<int, Pixel> pixels);
        void ApplyGridSize();
        void Fill(char character, ColorEntry colorEntry);
        void FlipHorizontally();
        void FlipVertically();
        void Rotate180();
        void Rotate90CW();
        void Rotate90CCW();
        void Resize(Vector2Int newSize, Vector2Int pivot);
    }
}
