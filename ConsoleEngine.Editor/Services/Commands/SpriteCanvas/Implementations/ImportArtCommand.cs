using ConsoleEngine.Editor.Services.Commands.Implementations;
using ConsoleEngine.Editor.Services.SpriteGrid;

namespace ConsoleEngine.Editor.Services.Commands.SpriteCanvas.Implementations
{
    internal sealed class ImportArtCommand : LogicCommand<string>, IImportArtCommand
    {
        private readonly ISpriteToolbarStateService spriteToolbarStateService;
        private readonly ICanvasDrawingService canvasDrawingService;

        public ImportArtCommand(ICanvasDrawingService canvasDrawingService, ISpriteToolbarStateService spriteToolbarStateService)
        {
            this.canvasDrawingService = canvasDrawingService;
            this.spriteToolbarStateService = spriteToolbarStateService;
        }

        protected override bool CanExecuteCommand(string? param)
        {
            return !string.IsNullOrEmpty(spriteToolbarStateService.GetImportedArt());
        }

        protected override void ExecuteCommand(string? param)
        {
            if (string.IsNullOrEmpty(param))
                return;
            canvasDrawingService.ImportArt(param);            
        }
    }
}
