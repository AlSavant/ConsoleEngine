using System.Collections.Generic;

namespace ConsoleEngine.Editor.Model
{
    internal sealed class HistoryService
    {
        private int MaxCount { get; set; }
        private int Index { get; set; }
        private List<State> States { get; set; }

        public bool CanUndo { get { return Index > 0; } }
        public bool CanRedo { get { return Index < States.Count - 1; } }

        public string UndoAction
        {
            get
            {
                if (CanUndo)
                {
                    return States[Index].StateName;
                }
                return string.Empty;
            }
        }

        public string RedoAction
        {
            get
            {
                if (CanRedo)
                {
                    return States[Index + 1].StateName;
                }
                return string.Empty;
            }
        }

        public State GetPreviousState()
        {
            if (!CanUndo)
                return default;
            Index--;
            return States[Index];

        }

        public State GetNextState()
        {
            if (!CanRedo)
                return default;
            Index++;
            return States[Index];
        }

        public void AddState(State state)
        {
            if (CanRedo)
            {
                var newStates = new List<State>(Index + 1);
                for (int i = 0; i < Index + 1; i++)
                {
                    newStates.Add(States[i]);
                }
                States = newStates;
            }
            States.Add(state);
            if (States.Count > MaxCount + 1)
                States.RemoveAt(0);
            Index = States.Count - 1;
        }

        public HistoryService(int maxCount)
        {
            MaxCount = maxCount;
            States = new List<State>();
            Index = 0;
        }
    }
}
