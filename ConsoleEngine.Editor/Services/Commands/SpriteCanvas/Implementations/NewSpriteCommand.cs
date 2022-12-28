using ConsoleEngine.Editor.Services.Commands.Implementations;
using ConsoleEngine.Editor.Services.History;
using ConsoleEngine.Editor.Services.IO;
using ConsoleEngine.Editor.Services.SpriteGrid;
using DataModel.Math.Structures;
using System.Windows;

namespace ConsoleEngine.Editor.Services.Commands.SpriteCanvas.Implementations
{
    internal sealed class NewSpriteCommand : LogicCommand, INewSpriteCommand
    {
        private readonly ISpriteGridStateService spriteGridStateService;
        private readonly ICanvasDrawingService canvasDrawingService;
        private readonly IHistoryActionService historyActionService;
        private readonly ISpriteSavePathService spriteSavePathService;

        public NewSpriteCommand(
            ISpriteSavePathService spriteSavePathService,
            ISpriteGridStateService spriteGridStateService, 
            ICanvasDrawingService canvasDrawingService, 
            IHistoryActionService historyActionService)
        {
            this.spriteSavePathService = spriteSavePathService;
            this.spriteGridStateService = spriteGridStateService;
            this.canvasDrawingService = canvasDrawingService;
            this.historyActionService = historyActionService;
        }

        protected override bool CanExecuteCommand(object? parameter)
        {
            return true;
        }

        protected override void ExecuteCommand(object? parameter)
        {
            if (!DiscardChanges())
                return;
            spriteGridStateService.SetGridSize(new Vector2Int(20, 20));
            canvasDrawingService.Clear();
            spriteGridStateService.SetGridVisibility(false);
            spriteGridStateService.SetDirtyStatus(false);
            historyActionService.ClearHistory();
            spriteSavePathService.SetCurrentSavePath(string.Empty);
        }

        private bool DiscardChanges()
        {
            if (!spriteGridStateService.IsDirty())
                return true;
            var messageBoxResult = MessageBox.Show("You have pending unsaved changes. Do you wish to discard them?", "Discard Changes", MessageBoxButton.YesNo);
            return messageBoxResult == MessageBoxResult.Yes;
        }
    }
}
