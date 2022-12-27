using ConsoleEngine.Editor.Services.Commands.Implementations;
using ConsoleEngine.Editor.Services.History;
using ConsoleEngine.Editor.Services.IO;
using ConsoleEngine.Editor.Services.Serialization;
using ConsoleEngine.Editor.Services.SpriteGrid;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;

namespace ConsoleEngine.Editor.Services.Commands.Serialization.Implementations
{
    internal sealed class OpenSpriteCommand : LogicCommand, IOpenSpriteCommand
    {
        private readonly ISpriteGridStateService spriteGridStateService;
        private readonly ISpriteSavePathService spriteSavePathService;
        private readonly ISpriteSerializationService spriteSerializationService;
        private readonly IHistoryActionService historyActionService;

        public OpenSpriteCommand(
            IHistoryActionService historyActionService,
            ISpriteSerializationService spriteSerializationService,
            ISpriteGridStateService spriteGridStateService, 
            ISpriteSavePathService spriteSavePathService)
        {
            this.historyActionService = historyActionService;
            this.spriteSerializationService = spriteSerializationService;
            this.spriteGridStateService = spriteGridStateService;
            this.spriteSavePathService = spriteSavePathService;
        }

        protected override bool CanExecuteCommand(object? parameter)
        {
            return true;
        }

        protected override void ExecuteCommand(object? parameter)
        {
            if (!DiscardChanges())
                return;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Console Sprite (*.csp)|*.csp";
            if (string.IsNullOrEmpty(spriteSavePathService.GetCurrentSavePath()))
            {
                openFileDialog.InitialDirectory = Environment.CurrentDirectory;
            }
            else
            {
                openFileDialog.InitialDirectory = Path.GetDirectoryName(spriteSavePathService.GetCurrentSavePath());
            }
            if (openFileDialog.ShowDialog() == true)
            {                
                var filePath = openFileDialog.FileName;
                var bytes = File.ReadAllBytes(filePath);
                spriteSerializationService.DeserializeSprite(bytes);                
                spriteGridStateService.SetDirtyStatus(false);                
                spriteSavePathService.SetCurrentSavePath(filePath);

                if (!spriteSavePathService.RecentFiles.Contains(filePath))
                {
                    spriteSavePathService.RecentFiles.Insert(0, filePath);
                    Properties.Settings.Default.RecentFiles.Insert(0, filePath);
                    if (spriteSavePathService.RecentFiles.Count > 10)
                    {
                        spriteSavePathService.RecentFiles.RemoveAt(10);
                        Properties.Settings.Default.RecentFiles.RemoveAt(10);
                    }
                    Properties.Settings.Default.Save();
                }
                else
                {
                    int idx = spriteSavePathService.RecentFiles.IndexOf(openFileDialog.FileName);
                    if (idx > 0)
                    {
                        for (int i = 0; i < idx; i++)
                        {
                            string movedFile = spriteSavePathService.RecentFiles[i];
                            spriteSavePathService.RecentFiles[i + 1] = movedFile;
                            Properties.Settings.Default.RecentFiles[i + 1] = movedFile;
                        }
                        spriteSavePathService.RecentFiles[0] = openFileDialog.FileName;
                        Properties.Settings.Default.RecentFiles[0] = openFileDialog.FileName;
                        Properties.Settings.Default.Save();
                    }
                }
                historyActionService.ClearHistory();
            }
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
