using ConsoleEngine.Editor.Model.History;
using ConsoleEngine.Editor.Services.History.Actions;
using DataModel.ComponentModel;

namespace ConsoleEngine.Editor.Services.History
{
    internal interface IHistoryActionService : IService, INotifyPropertyChanged
    {
        bool CanUndo { get; }
        bool CanRedo { get ; }
        string UndoActionName { get; }
        string RedoActionName { get; }
        void SetMaxActionBuffer(uint bufferSize);
        void AddHistoryAction<T1, T2>(T2 state) where T1 : IHistoryAction<T2> where T2 : HistoryState;
        void ApplyPreviousAction();
        void ApplyNextAction();
        void ClearHistory();
    }
}
