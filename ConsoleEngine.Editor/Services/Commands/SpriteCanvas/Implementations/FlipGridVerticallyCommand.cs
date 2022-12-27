using ConsoleEngine.Editor.Services.Commands.Implementations;
using ConsoleEngine.Editor.Services.SpriteGrid;

namespace ConsoleEngine.Editor.Services.Commands.SpriteCanvas.Implementations
{
    internal sealed class FlipGridVerticallyCommand : LogicCommand, IFlipGridVerticallyCommand
    {
        private readonly ICanvasDrawingService canvasDrawingService;

        public FlipGridVerticallyCommand(ICanvasDrawingService canvasDrawingService)
        {
            this.canvasDrawingService = canvasDrawingService;
        }

        protected override bool CanExecuteCommand(object? parameter)
        {
            return true;
        }

        protected override void ExecuteCommand(object? parameter)
        {
            canvasDrawingService.FlipVertically();
        }
    }
}
