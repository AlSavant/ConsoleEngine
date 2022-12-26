using ConsoleEngine.Editor.Model.History;
using ConsoleEngine.Editor.Services.History.Actions;
using ConsoleEngine.Editor.Services.Factories;
using System.Collections.Generic;
using System;
using DataModel.ComponentModel;

namespace ConsoleEngine.Editor.Services.History.Implementations
{
    internal sealed class HistoryActionService : IHistoryActionService
    {
        public Action<INotifyPropertyChanged, IPropertyChangedEventArgs>? PropertyChanged { get; set; }

        private uint bufferSize;
        private int currentIndex = -1;
        private bool isChangingState = false;

        private List<IHistoryAction> actions;

        private readonly IHistoryActionFactory historyActionFactory;

        public bool CanUndo { get { return currentIndex >= 0; } }
        public bool CanRedo { get { return currentIndex < actions.Count - 1; } }
       
        public HistoryActionService(IHistoryActionFactory historyActionFactory)
        {
            this.historyActionFactory = historyActionFactory;
            actions = new List<IHistoryAction>();        
        }

        public void SetMaxActionBuffer(uint bufferSize)
        {
            this.bufferSize = bufferSize;
        }

        public string UndoActionName
        {
            get
            {
                if (CanUndo)
                {
                    var state = actions[currentIndex].GetState();
                    if(state != null)
                        return state.stateName;
                }
                return string.Empty;
            }
        }

        public string RedoActionName
        {
            get
            {
                if (CanRedo)
                {
                    var state = actions[currentIndex + 1].GetState();
                    if (state != null)
                        return state.stateName;
                }
                return string.Empty;
            }
        }        

        public void AddHistoryAction<T1, T2>(T2 state) where T1 : IHistoryAction<T2> where T2 : HistoryState
        {
            if (isChangingState)
                return;
            T1 action = historyActionFactory.CreateInstance<T1>();
            action.Record(state);
            if(CanRedo)
            {
                var newActions = new List<IHistoryAction>(currentIndex + 1);
                for (int i = 0; i < actions.Count; i++)
                {
                    if(i < currentIndex + 1)
                    {
                        newActions.Add(actions[i]);
                        continue;
                    }
                    historyActionFactory.Dispose(actions[i]);
                    
                }
                actions = newActions;
            }
            actions.Add(action);
            if(actions.Count > bufferSize + 1)
            {
                var toDispose = actions[0];
                actions.RemoveAt(0);
                historyActionFactory.Dispose(toDispose);
            }
            currentIndex = actions.Count - 1;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HistoryChanged"));            
        }

        public void ApplyPreviousAction()
        {
            if (!CanUndo)
                return;
            isChangingState = true;
            actions[currentIndex].Undo();
            currentIndex -= 1;            
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HistoryChanged"));
            isChangingState = false;
        }

        public void ApplyNextAction()
        {
            if (!CanRedo)
                return;
            isChangingState = true;
            currentIndex += 1;
            actions[currentIndex].Redo();                 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HistoryChanged"));
            isChangingState = false;
        }
    }
}
