using ConsoleEngine.Editor.Model;
using ConsoleEngine.Editor.Model.ComponentModel.Implementations;
using ConsoleEngine.Editor.Services.SpriteGrid;
using DataModel.ComponentModel;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;

namespace ConsoleEngine.Editor.ViewModels.Implementations
{
    internal sealed class SpriteGridViewModel : ViewModel, ISpriteGridViewModel
    {
        private readonly ISpriteGridStateService spriteGridStateService;
        private readonly ICanvasDrawingService canvasDrawingService;

        public SpriteGridViewModel(ISpriteGridStateService spriteGridStateService, ICanvasDrawingService canvasDrawingService)
        {
            this.spriteGridStateService = spriteGridStateService;
            this.canvasDrawingService = canvasDrawingService;
            spriteGridStateService.PropertyChanged += OnGridSizeChangedEvent;
            canvasDrawingService.PropertyChanged += OnPixelsChangedEvent;
            canvasDrawingService.ApplyGridSize();
        }

        private void OnPixelsChangedEvent(INotifyPropertyChanged sender, IPropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Pixels")
                return;
            if(Pixels == null)
            {
                Pixels = new SmartCollection<PixelEntry>();
                for(int i = 0; i < GridWidth * GridHeight; i++)
                {
                    Pixels.Add(PixelEntry.Default);
                }
            }
            var pixels = (CanvasPixelsChangedEventArgs)args;
            foreach (var index in pixels.ChangedIndices)
            {
                Pixels[index].Character = canvasDrawingService.Get(index).character;
                Pixels[index].Color = canvasDrawingService.Get(index).colorEntry;
            }

        }

        private void OnGridSizeChangedEvent(INotifyPropertyChanged sender, IPropertyChangedEventArgs args)
        {
            if (args.PropertyName != "GridSize")
                return;
            var newSize = spriteGridStateService.GetGridSize();
            var newCount = newSize.x * newSize.y;
            if(Pixels == null)
            {
                Pixels = new SmartCollection<PixelEntry>();
            }
            if (Pixels.Count > newCount)
            {
                Pixels.TrimEnd(Pixels.Count - newCount);
            }
            else if (Pixels.Count < newCount)
            {
                List<PixelEntry> newEntries = new List<PixelEntry>(newCount - Pixels.Count);
                for (int i = 0; i < newCount - Pixels.Count; i++)
                {
                    newEntries.Add(PixelEntry.Default);
                }
                Pixels.AddRange(newEntries);
            }
            OnPropertyChanged(nameof(GridWidth));
            OnPropertyChanged(nameof(GridHeight));
            OnPropertyChanged(nameof(PixelWidth));
        }

        public int GridWidth
        {
            get
            {
                return spriteGridStateService.GetGridWidth();
            }
            set
            {
                if (value != spriteGridStateService.GetGridWidth())
                {
                    spriteGridStateService.SetGridWidth(value);
                    OnPropertyChanged(nameof(GridWidth));
                    OnPropertyChanged(nameof(PixelWidth));
                }
            }
        }

        public int GridHeight
        {
            get
            {
                return spriteGridStateService.GetGridHeight();
            }
            set
            {
                if (value != spriteGridStateService.GetGridHeight())
                {
                    spriteGridStateService.SetGridHeight(value);
                    OnPropertyChanged(nameof(GridHeight));
                }
            }
        }

        public int PixelWidth
        {
            get
            {
                return GridWidth * 20;
            }
        }

        public SolidColorBrush GridColor
        {
            get
            {
                return spriteGridStateService.CanShowGrid() ?
                    new SolidColorBrush(Colors.White) :
                    new SolidColorBrush(Colors.Black);
            }
        }

        public SmartCollection<PixelEntry>? Pixels { get; set; }       
        
        public ICommand SelectPixelCommand { get; set; }
    }
}
