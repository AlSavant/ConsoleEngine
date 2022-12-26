using ConsoleEngine.Editor.Services.History;

namespace ConsoleEngine.Editor.Services.Commands.Implementations
{
    internal sealed class UndoActionCommand : LogicCommand, IUndoActionCommand
    {
        private readonly IHistoryActionService historyActionService;

        public UndoActionCommand(IHistoryActionService historyActionService)
        {
            this.historyActionService = historyActionService;
        }

        protected override bool CanExecuteCommand(object? parameter)
        {
            return historyActionService.CanUndo;   
        }

        protected override void ExecuteCommand(object? parameter)
        {
            historyActionService.ApplyPreviousAction();
        }
    }
}
