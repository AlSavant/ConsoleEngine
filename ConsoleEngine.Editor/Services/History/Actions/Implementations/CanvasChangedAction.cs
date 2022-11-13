using ConsoleEngine.Editor.Model.History;
using ConsoleEngine.Editor.Services.SpriteGrid;

namespace ConsoleEngine.Editor.Services.History.Actions.Implementations
{
    internal sealed class CanvasChangedAction : ICanvasChangedAction
    {
        private CanvasState? state;

        private readonly ISpriteGridStateService spriteGridStateService;

        public CanvasChangedAction(ISpriteGridStateService spriteGridStateService)
        {
            this.spriteGridStateService = spriteGridStateService;
        }

        public void Record(CanvasState state)
        {
            this.state = state;
        }

        public void Reset()
        {

        }
    }
}
