using ConsoleEngine.Editor.Model.History;

namespace ConsoleEngine.Editor.Services.History.Actions
{
    internal interface IHistoryAction : IPooledService
    {
    }

    internal interface IHistoryAction<T> : IHistoryAction where T : HistoryState
    {
        void Record(T state);
        void Reset();
    }
}
