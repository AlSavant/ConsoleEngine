namespace ConsoleEngine.Editor.Model.History
{
    internal abstract class HistoryState
    {
        public string stateName;

        public HistoryState(string stateName)
        {
            this.stateName = stateName;
        }
    }
}
