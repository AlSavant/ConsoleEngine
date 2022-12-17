using ConsoleEngine.Editor.Model.History;
using ConsoleEngine.Editor.Services.SpriteGrid;

namespace ConsoleEngine.Editor.Services.History.Actions.Implementations
{
    internal sealed class PixelsPaintedAction : IPixelsPaintedAction
    {
        private PixelsPaintedState? state;        

        private readonly ICanvasDrawingService canvasDrawingService;
        private readonly ISpriteGridStateService spriteGridStateService;

        public PixelsPaintedAction(ICanvasDrawingService canvasDrawingService, ISpriteGridStateService spriteGridStateService)
        {
            this.canvasDrawingService = canvasDrawingService;
            this.spriteGridStateService = spriteGridStateService;
        }

        public void Record(PixelsPaintedState state)
        {
            this.state = state;
        }

        public HistoryState? GetState()
        {
            return state;
        }

        public void Redo()
        {
            if (state == null)
                return;
            spriteGridStateService.SetGridSize(state.NextGridSize);
            canvasDrawingService.SetPixels(state.NextPixels);                     
        }        

        public void Undo()
        {
            if (state == null)
                return;
            spriteGridStateService.SetGridSize(state.PreviousGridSize);
            canvasDrawingService.SetPixels(state.PreviousPixels);            
        }
    }
}
