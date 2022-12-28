using ConsoleEngine.Editor.Model;
using ConsoleEngine.Editor.Model.ComponentModel.Implementations;
using ConsoleEngine.Editor.Services.Commands;
using ConsoleEngine.Editor.Services.Commands.SpriteCanvas;
using ConsoleEngine.Editor.Services.SpriteGrid;
using DataModel.ComponentModel;
using DataModel.Math.Structures;
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
        public ILogicCommand ImportArtCommand { get; private set; }

        public SpriteGridViewModel(
            IImportArtCommand importArtCommand,
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
            ImportArtCommand = importArtCommand;
            spriteGridStateService.PropertyChanged += OnGridSizeChangedEvent;
            canvasDrawingService.PropertyChanged += OnPixelsChangedEvent;
            spriteGridStateService.PropertyChanged += OnGridVisibilityChanged;
            spriteToolbarStateService.PropertyChanged += OnImportedArtChanged;
            spriteGridStateService.PropertyChanged += OnHoveredPixelChanged;
            canvasDrawingService.ApplyGridSize();
        }

        private KeyValuePair<int, Pixel> PaintPixelParameterResolver(object? binding)
        {
            if (binding == null)
                return new KeyValuePair<int, Pixel>(-1, default);
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

        private void OnHoveredPixelChanged(INotifyPropertyChanged sender, IPropertyChangedEventArgs args)
        {
            if (args.PropertyName != "HoveredPixel")
                return;
            OnPropertyChanged(nameof(HoveredCoordinates));            
        }

        private void OnImportedArtChanged(INotifyPropertyChanged sender, IPropertyChangedEventArgs args)
        {
            if (args.PropertyName != "Imported Art")
                return;
            OnPropertyChanged(nameof(ImportedArt));
            ImportArtCommand.NotifyCanExecuteChanged();
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
            OnPropertyChanged(nameof(HoveredCoordinates));
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
                    OnPropertyChanged(nameof(HoveredCoordinates));
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
                    OnPropertyChanged(nameof(HoveredCoordinates));
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
                    spriteGridStateService.SetDirtyStatus(true);                    
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
        
        public string? ImportedArt
        {
            get
            {
                return spriteToolbarStateService.GetImportedArt();
            }
            set
            {
                if(value != spriteToolbarStateService.GetImportedArt())
                {
                    spriteToolbarStateService.SetImportedArt(value);
                    OnPropertyChanged(nameof(ImportedArt));
                    ImportArtCommand.NotifyCanExecuteChanged();
                }                
            }
        }  
        
        public string HoveredCoordinates
        {
            get
            {
                var pixelCoords = spriteGridStateService.GetHoveredPixel();
                if (pixelCoords == -Vector2Int.one)
                {
                    return string.Empty;
                }
                return $"X : {pixelCoords.x} | Y : {pixelCoords.y}";
            }
        }
    }
}
