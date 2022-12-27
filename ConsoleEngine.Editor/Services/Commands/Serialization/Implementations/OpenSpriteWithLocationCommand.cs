using ConsoleEngine.Editor.Services.Commands.Implementations;
using ConsoleEngine.Editor.Services.History;
using ConsoleEngine.Editor.Services.IO;
using ConsoleEngine.Editor.Services.Serialization;
using ConsoleEngine.Editor.Services.SpriteGrid;
using System;
using System.IO;
using System.Windows;

namespace ConsoleEngine.Editor.Services.Commands.Serialization.Implementations
{
    internal sealed class OpenSpriteWithLocationCommand : LogicCommand<string>, IOpenSpriteWithLocationCommand
    {
        private readonly ISpriteGridStateService spriteGridStateService;
        private readonly ISpriteSavePathService spriteSavePathService;
        private readonly ISpriteSerializationService spriteSerializationService;
        private readonly IHistoryActionService historyActionService;

        public OpenSpriteWithLocationCommand(
            ISpriteSerializationService spriteSerializationService,
            ISpriteGridStateService spriteGridStateService,
            ISpriteSavePathService spriteSavePathService,
            IHistoryActionService historyActionService)
        {
            this.spriteSerializationService = spriteSerializationService;
            this.spriteGridStateService = spriteGridStateService;
            this.spriteSavePathService = spriteSavePathService;
            this.historyActionService = historyActionService;
        }

        protected override bool CanExecuteCommand(string? parameter)
        {
            return true;
        }

        protected override void ExecuteCommand(string? path)
        {
            if (path == null)
                return;
            if (!DiscardChanges())
                return;
            if (!File.Exists(path))
            {
                Properties.Settings.Default.RecentFiles.Remove(path);
                spriteSavePathService.RecentFiles.Remove(path);
                Properties.Settings.Default.Save();
                MessageBox.Show("The requested sprite file could not be found at the target location. Removing from list.", "Sprite not found", MessageBoxButton.OK);                
                return;
            }
            var spriteBytes = File.ReadAllBytes(path);
            spriteSerializationService.DeserializeSprite(spriteBytes);            
            spriteGridStateService.SetDirtyStatus(false);            
            spriteSavePathService.SetCurrentSavePath(path);

            int idx = spriteSavePathService.RecentFiles.IndexOf(path);
            if (idx > 0)
            {
                for (int i = 0; i < idx; i++)
                {
                    string movedFile = spriteSavePathService.RecentFiles[i];
                    spriteSavePathService.RecentFiles[i + 1] = movedFile;
                    Properties.Settings.Default.RecentFiles[i + 1] = movedFile;
                }
                spriteSavePathService.RecentFiles[0] = path;
                Properties.Settings.Default.RecentFiles[0] = path;
                Properties.Settings.Default.Save();               
            }
            historyActionService.ClearHistory();
        }

        private bool DiscardChanges()
        {
            if (!spriteGridStateService.IsDirty())
                return true;
            var messageBoxResult = MessageBox.Show("You have pending unsaved changes. Do you wish to discard them?", "Discard Changes", MessageBoxButton.YesNo);
            return messageBoxResult == MessageBoxResult.Yes;
        }
    }
}
