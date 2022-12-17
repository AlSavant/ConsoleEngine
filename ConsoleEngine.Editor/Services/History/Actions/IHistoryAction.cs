using ConsoleEngine.Editor.Model.History;

namespace ConsoleEngine.Editor.Services.History.Actions
{
    internal interface IHistoryAction : IPooledService
    {
        HistoryState? GetState();
        void Redo();
        void Undo();
    }

    internal interface IHistoryAction<T> : IHistoryAction where T : HistoryState
    {
        void Record(T state);        
    }
}
