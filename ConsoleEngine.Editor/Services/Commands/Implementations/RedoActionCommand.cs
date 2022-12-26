using ConsoleEngine.Editor.Services.History;

namespace ConsoleEngine.Editor.Services.Commands.Implementations
{
    internal sealed class RedoActionCommand : LogicCommand, IRedoActionCommand
    {
        private readonly IHistoryActionService historyActionService;

        public RedoActionCommand(IHistoryActionService historyActionService)
        {
            this.historyActionService = historyActionService;
        }

        protected override bool CanExecuteCommand(object? parameter)
        {
            return historyActionService.CanRedo;
        }

        protected override void ExecuteCommand(object? parameter)
        {
            historyActionService.ApplyNextAction();
        }
    }
}
