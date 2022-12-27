using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;

namespace ConsoleEngine.Editor.Services.IO.Implementations
{
    internal sealed class SpriteSavePathService : ISpriteSavePathService
    {
        public ObservableCollection<string> RecentFiles { get; private set; }
        private string? currentSavePath;

        public string? GetCurrentSavePath()
        {            
            return currentSavePath;
        }

        public void SetCurrentSavePath(string? savePath)
        {
            currentSavePath = savePath;
        }

        public SpriteSavePathService()
        {
            RecentFiles = new ObservableCollection<string>();
            if (Properties.Settings.Default.RecentFiles == null)
            {
                Properties.Settings.Default.RecentFiles = new StringCollection();
                Properties.Settings.Default.Save();
            }
            for (int i = Properties.Settings.Default.RecentFiles.Count - 1; i >= 0; i--)
            {
                var file = Properties.Settings.Default.RecentFiles[i];
                if (!File.Exists(file))
                {
                    Properties.Settings.Default.RecentFiles.RemoveAt(i);
                    Properties.Settings.Default.Save();
                    continue;
                }
                RecentFiles.Add(file);
            }
        }
    }
}
