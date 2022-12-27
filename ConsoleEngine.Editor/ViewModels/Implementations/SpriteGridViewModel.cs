using ConsoleEngine.Editor.Model;
using ConsoleEngine.Editor.Model.ComponentModel.Implementations;
using ConsoleEngine.Editor.Services.Commands.SpriteCanvas;
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
        private readonly ISpriteToolbarStateService spriteToolbarStateService;

        public ICommand PaintPixelCommand { get; private set; }
        public ICommand ClearCommand { get; private set; }
        public ICommand FillCommand { get; private set; }

        public SpriteGridViewModel(
            ISpriteToolbarStateService spriteToolbarStateService,
            ISpriteGridStateService spriteGridStateService, 
            ICanvasDrawingService canvasDrawingService,
            IPaintPixelCommand paintPixelCommand,
            IClearSpriteGridCommand clearSpriteGridCommand, 
            IFillSpriteGridCommand fillSpriteGridCommand)
        {
            this.spriteToolbarStateService = spriteToolbarStateService;
            this.spriteGridStateService = spriteGridStateService;
            this.canvasDrawingService = canvasDrawingService;
            paintPixelCommand.SetParameterResolver(PaintPixelParameterResolver);
            PaintPixelCommand = paintPixelCommand;
            ClearCommand = clearSpriteGridCommand;
            FillCommand = fillSpriteGridCommand;
            spriteGridStateService.PropertyChanged += OnGridSizeChangedEvent;
            canvasDrawingService.PropertyChanged += OnPixelsChangedEvent;
            spriteGridStateService.PropertyChanged += OnGridVisibilityChanged;
            canvasDrawingService.ApplyGridSize();
        }

        private KeyValuePair<int, Pixel> PaintPixelParameterResolver(object? binding)
        {
            if (binding == null)
                return default;
            var pixelEntry = (PixelViewModel)binding;
            if (pixelEntry == null)
                return default;
            if (Pixels == null)
                return default;
            var index = Pixels.IndexOf(pixelEntry);
            char character = pixelEntry.Character;
            if(spriteToolbarStateService.CanPaintCharacters())
            {
                character = spriteToolbarStateService.GetSelectedCharacter();
            }
            return new KeyValuePair<int, Pixel>(index, new Pixel(
                    character, 
                    spriteToolbarStateService.GetSelectedColor()));
        }

        private void OnGridVisibilityChanged(INotifyPropertyChanged sender, IPropertyChangedEventArgs args)
        {
            if (args.PropertyName != "GridVisibility")
                return;
            OnPropertyChanged(nameof(ShowGrid));
        }

        private void OnPixelsChangedEvent(INotifyPropertyChanged sender, IPropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Pixels")
                return;
            if(Pixels == null)
            {
                Pixels = new SmartCollection<PixelViewModel>();
                for(int i = 0; i < GridWidth * GridHeight; i++)
                {
                    Pixels.Add(PixelViewModel.Default);
                }
            }
            var pixels = (CanvasPixelsChangedEventArgs)args;
            foreach (var index in pixels.ChangedIndices)
            {
                if (Pixels.Count <= index)
                    continue;
                if (index < 0)
                    continue;
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
                Pixels = new SmartCollection<PixelViewModel>();
            }
            if (Pixels.Count > newCount)
            {
                Pixels.TrimEnd(Pixels.Count - newCount);
            }
            else if (Pixels.Count < newCount)
            {
                List<PixelViewModel> newEntries = new List<PixelViewModel>(newCount - Pixels.Count);
                for (int i = 0; i < newCount - Pixels.Count; i++)
                {
                    newEntries.Add(PixelViewModel.Default);
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

        public SmartCollection<PixelViewModel>? Pixels { get; set; }

        public bool SupportsTransparency
        {
            get
            {
                return spriteGridStateService.SupportsTransparency();
            }
            set
            {
                if (value != spriteGridStateService.SupportsTransparency())
                {
                    spriteGridStateService.SetTransparencyMode(value);
                    OnPropertyChanged(nameof(SupportsTransparency));
                    //IsDirty = true;
                }
            }
        }

        public bool ShowGrid
        {
            get
            {
                return spriteGridStateService.CanShowGrid();
            }
            set
            {
                if (value != spriteGridStateService.CanShowGrid())
                {
                    spriteGridStateService.SetGridVisibility(value);
                    OnPropertyChanged(nameof(ShowGrid));
                    OnPropertyChanged(nameof(GridColor));
                }
            }
        }        

        private string importedArt;
        public string ImportedArt
        {
            get
            {
                return importedArt;
            }
            set
            {
                SetProperty(ref importedArt, value, nameof(ImportedArt));
            }
        }
    }
}
