using ConsoleEngine.Editor.Model;
using ConsoleEngine.Editor.Services.SpriteGrid;
using System.Collections.Generic;

namespace ConsoleEngine.Editor.Services.Commands.Implementations
{
    internal sealed class PaintPixelCommand : LogicCommand<KeyValuePair<int, Pixel>>, IPaintPixelCommand
    {
        private readonly ICanvasDrawingService canvasDrawingService;

        public PaintPixelCommand(ICanvasDrawingService canvasDrawingService)
        {
            this.canvasDrawingService = canvasDrawingService;
        }

        protected override bool CanExecuteCommand(KeyValuePair<int, Pixel> param)
        {
            return true;
        }

        protected override void ExecuteCommand(KeyValuePair<int, Pixel> param)
        {
            canvasDrawingService.SetPixel(param.Key, param.Value.character, param.Value.colorEntry);
        }
    }
}
