using ConsoleEngine.Editor.Services.SpriteGrid;

namespace ConsoleEngine.Editor.Services.Commands.Implementations
{
    internal sealed class FillSpriteGridCommand : LogicCommand, IFillSpriteGridCommand
    {
        private readonly ICanvasDrawingService canvasDrawingService;
        private readonly ISpriteToolbarStateService spriteToolbarStateService;

        public FillSpriteGridCommand(ICanvasDrawingService canvasDrawingService, ISpriteToolbarStateService spriteToolbarStateService)
        {
            this.canvasDrawingService = canvasDrawingService;
            this.spriteToolbarStateService = spriteToolbarStateService;
        }

        protected override bool CanExecuteCommand(object? parameter)
        {
            return true;
        }

        protected override void ExecuteCommand(object? parameter)
        {
            var color = spriteToolbarStateService.GetSelectedColor();
            if(spriteToolbarStateService.CanPaintCharacters())
            {
                var character = spriteToolbarStateService.GetSelectedCharacter();
                canvasDrawingService.Fill(character, color);
            }   
            else
            {
                canvasDrawingService.ColorFill(color);
            }
        }
    }
}
