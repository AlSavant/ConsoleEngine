using ConsoleEngine.Editor.Services.History.Actions;

namespace ConsoleEngine.Editor.Services.Factories
{
    internal interface IHistoryActionFactory : IService
    {
        T CreateInstance<T>() where T : IHistoryAction;
        void Dispose(IHistoryAction historyAction);
    }
}
