using ConsoleEngine.Editor.Model;
using ConsoleEngine.Editor.Model.ComponentModel.Implementations;
using ConsoleEngine.Editor.Model.History;
using ConsoleEngine.Editor.Services.History;
using ConsoleEngine.Editor.Services.History.Actions;
using ConsoleEngine.Editor.Services.Util;
using DataModel.ComponentModel;
using DataModel.Math.Structures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleEngine.Editor.Services.SpriteGrid.Implementations
{
    internal sealed class CanvasDrawingService : ICanvasDrawingService
    {
        public Action<INotifyPropertyChanged, IPropertyChangedEventArgs>? PropertyChanged { get; set; }

        private Pixel[] pixels;
        private readonly ISpriteGridStateService spriteGridStateService;
        private readonly IHistoryActionService historyActionService;
        private readonly IMatrixOperationsService matrixOperationsService;

        public CanvasDrawingService(
            ISpriteGridStateService spriteGridStateService, 
            IHistoryActionService historyActionService,
            IMatrixOperationsService matrixOperationsService)
        {
            this.spriteGridStateService = spriteGridStateService;
            this.historyActionService = historyActionService;
            this.matrixOperationsService = matrixOperationsService;
            pixels = Array.Empty<Pixel>();            
        }

        public Pixel Get(int i)
        {
            if (pixels == null)
                throw new IndexOutOfRangeException();            
            return pixels[i];
        }

        public int PixelCount
        {
            get
            {
                return pixels.Length;
            }
        }

        public void SetPixels(Dictionary<int, Pixel> pixels)
        {
            List<int> changedPixels = new List<int>();
            var dict = new Dictionary<int, KeyValuePair<Pixel, Pixel>>();
            foreach (var pixelEntry in pixels)
            {
                if (this.pixels.Length >= pixelEntry.Key)
                    continue;
                var previousPixel = this.pixels[pixelEntry.Key];
                if(SetPixelNoBroadcast(pixelEntry.Key, pixelEntry.Value.character, pixelEntry.Value.colorEntry))
                {
                    var currentPixel = this.pixels[pixelEntry.Key];
                    dict.Add(pixelEntry.Key, new KeyValuePair<Pixel, Pixel>(previousPixel, currentPixel));
                    changedPixels.Add(pixelEntry.Key);
                }
            }
            if(changedPixels.Count > 0)
            {
                var gridSize = spriteGridStateService.GetGridSize();
                var state = new PixelsPaintedState("Paint Pixels", dict, new KeyValuePair<Vector2Int, Vector2Int>(gridSize, gridSize));
                historyActionService.AddHistoryAction<IPixelsPaintedAction, PixelsPaintedState>(state);
                PropertyChanged?.Invoke(this, new CanvasPixelsChangedEventArgs("Pixels", changedPixels.ToArray()));
            }
        }

        public void SetPixel(int index, char character, ColorEntry colorEntry)
        {            
            if (pixels.Length >= index)
                return;
            var currentPixel = pixels[index];
            if (SetPixelNoBroadcast(index, character, colorEntry))
            {
                var gridSize = spriteGridStateService.GetGridSize();
                var dict = new Dictionary<int, KeyValuePair<Pixel, Pixel>>();
                dict.Add(index, new KeyValuePair<Pixel, Pixel>(currentPixel, pixels[index]));
                historyActionService.AddHistoryAction<IPixelsPaintedAction, PixelsPaintedState>(new PixelsPaintedState("Paint Pixel", dict, new KeyValuePair<Vector2Int, Vector2Int>(gridSize,gridSize)));
                PropertyChanged?.Invoke(this, new CanvasPixelsChangedEventArgs("Pixels", new int[] { index }));
            }
        }        

        private bool SetPixelNoBroadcast(int index, char character, ColorEntry colorEntry)
        {
            if (pixels == null)
                return false;
            if (pixels[index] == default(Pixel))
            {
                pixels[index] = new Pixel();
            }
            bool isDirty = pixels[index].colorEntry != colorEntry || pixels[index].character != character;
            pixels[index].character = character;
            pixels[index].colorEntry = colorEntry;
            return isDirty;
        }

        public void ApplyGridSize()
        {
            pixels = new Pixel[spriteGridStateService.GetGridWidth() * spriteGridStateService.GetGridHeight()];
            PropertyChanged?.Invoke(this, new CanvasPixelsChangedEventArgs("Pixels", Enumerable.Range(0, pixels.Length).ToArray()));
        }

        public void Fill(char character, ColorEntry colorEntry)
        {
            if (pixels == null)
                return;
            var changedPixels = new Dictionary<int, KeyValuePair<Pixel, Pixel>>();
            List<int> dirtyPixels = new List<int>();
            for (int i = 0; i < pixels.Length; i++)
            {
                var previousPixel = pixels[i];
                if (SetPixelNoBroadcast(i, character, colorEntry))
                {
                    dirtyPixels.Add(i);
                    changedPixels.Add(i, new KeyValuePair<Pixel, Pixel>(previousPixel, pixels[i]));
                }
            }
            if (dirtyPixels.Count > 0)
            {
                var gridSize = spriteGridStateService.GetGridSize();
                historyActionService.AddHistoryAction<IPixelsPaintedAction, PixelsPaintedState>(new PixelsPaintedState("Fill", changedPixels, new KeyValuePair<Vector2Int, Vector2Int>(gridSize, gridSize)));
                PropertyChanged?.Invoke(this, new CanvasPixelsChangedEventArgs("Pixels", dirtyPixels.ToArray()));
            }
        }

        public void FlipHorizontally()
        {
            var dict = TransformSprites(matrixOperationsService.FlipMatrixHorizontally);
            if (dict == null || dict.Count <= 0)
                return;
            var gridSize = spriteGridStateService.GetGridSize();
            var state = new PixelsPaintedState("Flip Grid Horizontally", dict, new KeyValuePair<Vector2Int, Vector2Int>(gridSize, gridSize));
            historyActionService.AddHistoryAction<IPixelsPaintedAction, PixelsPaintedState>(state);
            PropertyChanged?.Invoke(this, new CanvasPixelsChangedEventArgs("Pixels", dict.Keys.ToArray()));
        }

        public void FlipVertically()
        {
            var dict = TransformSprites(matrixOperationsService.FlipMatrixVertically);
            if (dict == null || dict.Count <= 0)
                return;
            var gridSize = spriteGridStateService.GetGridSize();
            var state = new PixelsPaintedState("Flip Grid Vertically", dict, new KeyValuePair<Vector2Int, Vector2Int>(gridSize, gridSize));
            historyActionService.AddHistoryAction<IPixelsPaintedAction, PixelsPaintedState>(state);
            PropertyChanged?.Invoke(this, new CanvasPixelsChangedEventArgs("Pixels", dict.Keys.ToArray()));            
        }

        public void Rotate180()
        {
            var dict = TransformSprites(matrixOperationsService.RotateMatrix180);
            if (dict == null || dict.Count <= 0)
                return;
            var gridSize = spriteGridStateService.GetGridSize();
            var state = new PixelsPaintedState("Rotate Grid 180°", dict, new KeyValuePair<Vector2Int, Vector2Int>(gridSize, gridSize));
            historyActionService.AddHistoryAction<IPixelsPaintedAction, PixelsPaintedState>(state);
            PropertyChanged?.Invoke(this, new CanvasPixelsChangedEventArgs("Pixels", dict.Keys.ToArray()));
        }

        public void Rotate90CW()
        {
            var dict = TransformSprites(matrixOperationsService.RotateMatrix90CW);
            if (dict == null || dict.Count <= 0)
                return;
            var size = spriteGridStateService.GetGridSize();
            spriteGridStateService.SetGridSize(new Vector2Int(size.y, size.x));
            var state = new PixelsPaintedState("Rotate Grid 90° CW", dict, new KeyValuePair<Vector2Int, Vector2Int>(size, spriteGridStateService.GetGridSize()));
            historyActionService.AddHistoryAction<IPixelsPaintedAction, PixelsPaintedState>(state);
            PropertyChanged?.Invoke(this, new CanvasPixelsChangedEventArgs("Pixels", dict.Keys.ToArray()));
        }

        public void Rotate90CCW()
        {
            var dict = TransformSprites(matrixOperationsService.RotateMatrix90CCW);
            if (dict == null || dict.Count <= 0)
                return;
            var size = spriteGridStateService.GetGridSize();
            spriteGridStateService.SetGridSize(new Vector2Int(size.y, size.x));
            var state = new PixelsPaintedState("Rotate Grid 90° CCW", dict, new KeyValuePair<Vector2Int, Vector2Int>(size, spriteGridStateService.GetGridSize()));
            historyActionService.AddHistoryAction<IPixelsPaintedAction, PixelsPaintedState>(state);
            PropertyChanged?.Invoke(this, new CanvasPixelsChangedEventArgs("Pixels", dict.Keys.ToArray()));
        }

        public void Resize(Vector2Int newSize, Vector2Int pivot)
        {
            var oldSize = spriteGridStateService.GetGridSize();
            if (newSize == oldSize)
                return;
            var newPixels = matrixOperationsService.ResizeMatrix(pixels, oldSize.x, oldSize.y, newSize.x, newSize.y, pivot);

            var dict = new Dictionary<int, KeyValuePair<Pixel, Pixel>>();
            for (int i = 0; i < newPixels.Length; i++)
            {
                var previousPixel = default(Pixel);
                if(i < pixels.Length)
                {
                    previousPixel = pixels[i];
                    dict.Add(i, new KeyValuePair<Pixel, Pixel>(previousPixel, newPixels[i]));
                    continue;
                }
                    
                if (SetPixelNoBroadcast(i, newPixels[i].character, newPixels[i].colorEntry))
                {
                    var currentPixel = newPixels[i];
                    dict.Add(i, new KeyValuePair<Pixel, Pixel>(previousPixel, currentPixel));
                }
            }
            for(int i = newPixels.Length; i < pixels.Length; i++)
            {
                var previousPixel = pixels[i];
                var newPixel = default(Pixel);
                dict.Add(i, new KeyValuePair<Pixel, Pixel>(previousPixel, newPixel));
            }
            if(dict.Count > 0)
            {
                pixels = newPixels;
                spriteGridStateService.SetGridSize(new Vector2Int(newSize.y, newSize.x));
                var state = new PixelsPaintedState("Resize Grid", dict, new KeyValuePair<Vector2Int, Vector2Int>(oldSize, newSize));
                historyActionService.AddHistoryAction<IPixelsPaintedAction, PixelsPaintedState>(state);
                PropertyChanged?.Invoke(this, new CanvasPixelsChangedEventArgs("Pixels", dict.Keys.ToArray()));
            }
        }

        private Dictionary<int, KeyValuePair<Pixel, Pixel>>? TransformSprites(Func<Pixel[], int, int, Pixel[]> operation)
        {
            if (pixels == null || pixels.Length == 0)
                return null;
            var size = spriteGridStateService.GetGridSize();
            var flippedPixels = operation.Invoke(pixels, size.x, size.y);           
            var dict = new Dictionary<int, KeyValuePair<Pixel, Pixel>>();
            for (int i = 0; i < pixels.Length; i++)
            {
                var previousPixel = pixels[i];
                if (SetPixelNoBroadcast(i, flippedPixels[i].character, flippedPixels[i].colorEntry))
                {
                    var currentPixel = flippedPixels[i];
                    dict.Add(i, new KeyValuePair<Pixel, Pixel>(previousPixel, currentPixel));                   
                }
            }
            return dict;            
        }
    }
}
