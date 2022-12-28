using ConsoleEngine.Editor.Model;
using ConsoleEngine.Editor.Services.Commands.Implementations;
using ConsoleEngine.Editor.Services.SpriteGrid;
using System.Collections.Generic;
using System.Windows.Input;

namespace ConsoleEngine.Editor.Services.Commands.SpriteCanvas.Implementations
{
    internal sealed class PaintPixelCommand : LogicCommand<KeyValuePair<int, Pixel>>, IPaintPixelCommand
    {
        private readonly ICanvasDrawingService canvasDrawingService;
        private readonly ISpriteGridStateService spriteGridStateService;

        public PaintPixelCommand(ICanvasDrawingService canvasDrawingService, ISpriteGridStateService spriteGridStateService)
        {
            this.canvasDrawingService = canvasDrawingService;
            this.spriteGridStateService = spriteGridStateService;
        }

        protected override bool CanExecuteCommand(KeyValuePair<int, Pixel> param)
        {
            return true;
        }

        protected override void ExecuteCommand(KeyValuePair<int, Pixel> param)
        {
            if(Mouse.LeftButton == MouseButtonState.Pressed)
                canvasDrawingService.SetPixel(param.Key, param.Value.character, param.Value.colorEntry);
            spriteGridStateService.SetHoveredPixel(param.Key);
        }
    }
}
