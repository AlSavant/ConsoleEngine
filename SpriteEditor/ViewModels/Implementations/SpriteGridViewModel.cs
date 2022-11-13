using SpriteEditor.Models;
using SpriteEditor.Services.SpriteGrid;
using System.Windows.Media;

namespace SpriteEditor.ViewModels.Implementations
{
    internal sealed class SpriteGridViewModel : ViewModel, ISpriteGridViewModel
    {
        private readonly ISpriteGridStateService spriteGridStateService;

        public SpriteGridViewModel(ISpriteGridStateService spriteGridStateService)
        {
            this.spriteGridStateService = spriteGridStateService;            
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

        public SmartCollection<PixelEntry> Pixels { get; set; }
    }
}
