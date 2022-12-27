using ConsoleEngine.Editor.Services.Commands.Implementations;
using ConsoleEngine.Editor.Services.SpriteGrid;

namespace ConsoleEngine.Editor.Services.Commands.SpriteCanvas.Implementations
{
    internal sealed class RotateGrid90CWCommand : LogicCommand, IRotateGrid90CWCommand
    {
        private readonly ICanvasDrawingService canvasDrawingService;

        public RotateGrid90CWCommand(ICanvasDrawingService canvasDrawingService)
        {
            this.canvasDrawingService = canvasDrawingService;
        }

        protected override bool CanExecuteCommand(object? parameter)
        {
            return true;
        }

        protected override void ExecuteCommand(object? parameter)
        {
            canvasDrawingService.Rotate90CW();
        }
    }
}
