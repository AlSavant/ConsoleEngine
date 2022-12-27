using System.Collections.ObjectModel;

namespace ConsoleEngine.Editor.Services.IO
{
    internal interface ISpriteSavePathService : IService
    {
        ObservableCollection<string> RecentFiles { get; }
        string? GetCurrentSavePath();
        void SetCurrentSavePath(string? savePath);
    }
}
