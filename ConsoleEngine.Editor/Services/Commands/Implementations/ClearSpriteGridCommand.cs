using ConsoleEngine.Editor.Services.SpriteGrid;

namespace ConsoleEngine.Editor.Services.Commands.Implementations
{
    internal sealed class ClearSpriteGridCommand : LogicCommand, IClearSpriteGridCommand
    {
        private readonly ICanvasDrawingService canvasDrawingService;

        public ClearSpriteGridCommand(ICanvasDrawingService canvasDrawingService)
        {
            this.canvasDrawingService = canvasDrawingService;
        }

        protected override bool CanExecuteCommand()
        {
            return true;
        }

        protected override void ExecuteCommand()
        {
            canvasDrawingService.Clear();
        }
    }
}
