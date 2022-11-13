using ConsoleEngine.Editor.Model;
using ConsoleEngine.Editor.Model.ComponentModel.Implementations;
using DataModel.ComponentModel;
using System;
using System.Collections.Generic;

namespace ConsoleEngine.Editor.Services.SpriteGrid.Implementations
{
    internal sealed class CanvasDrawingService : ICanvasDrawingService
    {
        public Action<INotifyPropertyChanged, IPropertyChangedEventArgs>? PropertyChanged { get; set; }

        private Pixel[]? pixels;
        private readonly ISpriteGridStateService spriteGridStateService;

        public CanvasDrawingService(ISpriteGridStateService spriteGridStateService)
        {
            this.spriteGridStateService = spriteGridStateService;
        }

        public Pixel? Get(int i)
        {
            if (pixels == null)
                return default;
            return pixels[i];
        }

        public void SetPixel(int index, char character, ColorEntry colorEntry)
        {
            if (SetPixelNoBroadcast(index, character, colorEntry))
            {
                PropertyChanged?.Invoke(this, new CanvasPixelsChangedEventArgs("Pixels", new int[] { index }));
            }
        }

        public void SetPixels(Pixel[] pixels)
        {

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

        }

        public void Fill(char character, ColorEntry colorEntry)
        {
            if (pixels == null)
                return;
            List<int> dirtyPixels = new List<int>();
            for (int i = 0; i < pixels.Length; i++)
            {
                if (SetPixelNoBroadcast(i, character, colorEntry))
                {
                    dirtyPixels.Add(i);
                }
            }
            if (dirtyPixels.Count > 0)
            {
                PropertyChanged?.Invoke(this, new CanvasPixelsChangedEventArgs("Pixels", dirtyPixels.ToArray()));
            }
        }
    }
}
