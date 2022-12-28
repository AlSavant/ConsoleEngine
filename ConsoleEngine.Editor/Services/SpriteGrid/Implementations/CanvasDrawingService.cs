using ConsoleEngine.Editor.Model;
using ConsoleEngine.Editor.Model.ComponentModel.Implementations;
using ConsoleEngine.Editor.Model.History;
using ConsoleEngine.Editor.Services.History;
using ConsoleEngine.Editor.Services.History.Actions;
using ConsoleEngine.Editor.Services.Util;
using ConsoleEngine.Editor.Services.Encoding;
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
        private readonly ICharToOEM437ConverterService charToOEM437ConverterService;
        private readonly ISpriteToolbarStateService spriteToolbarStateService;

        public CanvasDrawingService(
            ISpriteToolbarStateService spriteToolbarStateService,
            ICharToOEM437ConverterService charToOEM437ConverterService,
            ISpriteGridStateService spriteGridStateService, 
            IHistoryActionService historyActionService,
            IMatrixOperationsService matrixOperationsService)
        {
            this.spriteToolbarStateService = spriteToolbarStateService;
            this.charToOEM437ConverterService = charToOEM437ConverterService;
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
                if (pixelEntry.Key >= this.pixels.Length)
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
                spriteGridStateService.SetDirtyStatus(true);
                historyActionService.AddHistoryAction<IPixelsPaintedAction, PixelsPaintedState>(state);
                PropertyChanged?.Invoke(this, new CanvasPixelsChangedEventArgs("Pixels", changedPixels.ToArray()));
            }
        }

        public void SetPixel(int index, char character, ColorEntry colorEntry)
        {            
            if (index >= pixels.Length)
                return;
            var currentPixel = pixels[index];
            if (SetPixelNoBroadcast(index, character, colorEntry))
            {
                var gridSize = spriteGridStateService.GetGridSize();
                var dict = new Dictionary<int, KeyValuePair<Pixel, Pixel>>();
                dict.Add(index, new KeyValuePair<Pixel, Pixel>(currentPixel, pixels[index]));
                spriteGridStateService.SetDirtyStatus(true);
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
            var length = spriteGridStateService.GetGridWidth() * spriteGridStateService.GetGridHeight();
            if (pixels != null && pixels.Length == length)
                return;
            if (pixels == null)
                pixels = new Pixel[length];
            else if (pixels.Length != length)
                Array.Resize(ref pixels, length);            
            PropertyChanged?.Invoke(this, new CanvasPixelsChangedEventArgs("Pixels", Enumerable.Range(0, pixels.Length).ToArray()));
        }

        public void Clear()
        {
            if (pixels == null)
                return;
            var changedPixels = new Dictionary<int, KeyValuePair<Pixel, Pixel>>();
            List<int> dirtyPixels = new List<int>();
            for (int i = 0; i < pixels.Length; i++)
            {
                var previousPixel = pixels[i];
                if (SetPixelNoBroadcast(i, ' ', ColorEntry.FromConsoleColor(ConsoleColor.Black)))
                {
                    dirtyPixels.Add(i);
                    changedPixels.Add(i, new KeyValuePair<Pixel, Pixel>(previousPixel, pixels[i]));
                }
            }
            if (dirtyPixels.Count > 0)
            {
                var gridSize = spriteGridStateService.GetGridSize();
                spriteGridStateService.SetDirtyStatus(true);
                historyActionService.AddHistoryAction<IPixelsPaintedAction, PixelsPaintedState>(new PixelsPaintedState("Clear", changedPixels, new KeyValuePair<Vector2Int, Vector2Int>(gridSize, gridSize)));
                PropertyChanged?.Invoke(this, new CanvasPixelsChangedEventArgs("Pixels", dirtyPixels.ToArray()));
            }
        }

        public void ColorFill(ColorEntry colorEntry)
        {
            if (pixels == null)
                return;
            var changedPixels = new Dictionary<int, KeyValuePair<Pixel, Pixel>>();
            List<int> dirtyPixels = new List<int>();
            for (int i = 0; i < pixels.Length; i++)
            {
                var previousPixel = pixels[i];
                if (SetPixelNoBroadcast(i, previousPixel.character, colorEntry))
                {
                    dirtyPixels.Add(i);
                    changedPixels.Add(i, new KeyValuePair<Pixel, Pixel>(previousPixel, pixels[i]));
                }
            }
            if (dirtyPixels.Count > 0)
            {
                var gridSize = spriteGridStateService.GetGridSize();
                spriteGridStateService.SetDirtyStatus(true);
                historyActionService.AddHistoryAction<IPixelsPaintedAction, PixelsPaintedState>(new PixelsPaintedState("Fill", changedPixels, new KeyValuePair<Vector2Int, Vector2Int>(gridSize, gridSize)));
                PropertyChanged?.Invoke(this, new CanvasPixelsChangedEventArgs("Pixels", dirtyPixels.ToArray()));
            }
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
                spriteGridStateService.SetDirtyStatus(true);
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
            spriteGridStateService.SetDirtyStatus(true);
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
            spriteGridStateService.SetDirtyStatus(true);
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
            spriteGridStateService.SetDirtyStatus(true);
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
            spriteGridStateService.SetDirtyStatus(true);
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
            spriteGridStateService.SetDirtyStatus(true);
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
                spriteGridStateService.SetGridSize(new Vector2Int(newSize.x, newSize.y));
                spriteGridStateService.SetDirtyStatus(true);
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

        public void ImportArt(string param)
        {
            var currentSize = spriteGridStateService.GetGridSize();
            var lines = param.Split('\n');
            var gridHeight = lines.Length;
            var ordered = lines.OrderByDescending(x => x.Length);
            int leftPad = lines.Length <= 1 ? 0 : -1;
            var gridWidth = ordered.First().Length + leftPad;
            var oldPixels = pixels;
            pixels = new Pixel[gridHeight * gridWidth];
            var dict = new Dictionary<int, KeyValuePair<Pixel, Pixel>>();

            for (int y = 0; y < lines.Length; y++)
            {
                var line = lines[y];
                for (int x = 0; x < gridWidth; x++)
                {
                    if (x >= line.Length)
                        continue;
                    var index = y * gridWidth + x;
                    var oldPixel = new Pixel();
                    if(index < oldPixels.Length)
                        oldPixel = oldPixels[index];
                    var pixel = new Pixel();
                    if (charToOEM437ConverterService.IsValidChar(line[x]))
                    {
                        pixel.character = line[x];
                        pixel.colorEntry = spriteToolbarStateService.GetSelectedColor();
                    }
                    else
                    {
                        pixel.character = charToOEM437ConverterService.GetInvalidCharacter();
                        pixel.colorEntry = ColorEntry.FromConsoleColor(ConsoleColor.Black);
                    }
                    if(SetPixelNoBroadcast(index, pixel.character, pixel.colorEntry))
                    {
                        dict.Add(index, new KeyValuePair<Pixel, Pixel>(oldPixel, pixel));
                    }                    
                }
            }


            for (int i = 0; i < pixels.Length; i++)
            {
                if (dict.ContainsKey(i))
                    continue;
                var previousPixel = default(Pixel);
                if (i < oldPixels.Length)
                {
                    previousPixel = oldPixels[i];
                    dict.Add(i, new KeyValuePair<Pixel, Pixel>(previousPixel, pixels[i]));
                    continue;
                }

                if (SetPixelNoBroadcast(i, pixels[i].character, pixels[i].colorEntry))
                {
                    var currentPixel = pixels[i];
                    dict.Add(i, new KeyValuePair<Pixel, Pixel>(previousPixel, currentPixel));
                }
            }
            for (int i = pixels.Length; i < oldPixels.Length; i++)
            {
                if (dict.ContainsKey(i))
                    continue;
                var previousPixel = oldPixels[i];
                var newPixel = default(Pixel);
                dict.Add(i, new KeyValuePair<Pixel, Pixel>(previousPixel, newPixel));
            }            
            
            if (dict.Count > 0)
            {                
                var newSize = new Vector2Int(gridWidth, gridHeight);
                spriteGridStateService.SetGridSize(newSize);
                spriteGridStateService.SetDirtyStatus(true);
                var state = new PixelsPaintedState("Import Art", dict, new KeyValuePair<Vector2Int, Vector2Int>(currentSize, newSize));
                historyActionService.AddHistoryAction<IPixelsPaintedAction, PixelsPaintedState>(state);
                PropertyChanged?.Invoke(this, new CanvasPixelsChangedEventArgs("Pixels", dict.Keys.ToArray()));
            }
            spriteToolbarStateService.SetImportedArt(string.Empty);
        } 
    }
}
