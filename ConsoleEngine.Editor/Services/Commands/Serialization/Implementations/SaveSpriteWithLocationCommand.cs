using ConsoleEngine.Editor.Services.Commands.Implementations;
using ConsoleEngine.Editor.Services.IO;
using ConsoleEngine.Editor.Services.Serialization;
using ConsoleEngine.Editor.Services.SpriteGrid;
using Microsoft.Win32;
using System;
using System.IO;

namespace ConsoleEngine.Editor.Services.Commands.Serialization.Implementations
{
    internal sealed class SaveSpriteWithLocationCommand : LogicCommand, ISaveSpriteWithLocationCommand
    {
        private readonly ISpriteGridStateService spriteGridStateService;
        private readonly ISpriteSavePathService spriteSavePathService;

        private readonly ISpriteSerializationService spriteSerializationService;

        public SaveSpriteWithLocationCommand(
            ISpriteSerializationService spriteSerializationService,            
            ISpriteGridStateService spriteGridStateService,
            ISpriteSavePathService spriteSavePathService)
        {
            this.spriteSerializationService = spriteSerializationService;            
            this.spriteSavePathService = spriteSavePathService;
            this.spriteGridStateService = spriteGridStateService;            
        }

        protected override bool CanExecuteCommand(object? parameter)
        {
            return spriteGridStateService.IsDirty();
        }

        protected override void ExecuteCommand(object? parameter)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Console Sprite (*.csp)|*.csp";
            if (string.IsNullOrEmpty(spriteSavePathService.GetCurrentSavePath()))
            {
                saveFileDialog.InitialDirectory = Environment.CurrentDirectory;
            }
            else
            {
                saveFileDialog.InitialDirectory = Path.GetDirectoryName(spriteSavePathService.GetCurrentSavePath());
            }
            if (saveFileDialog.ShowDialog() == true)
            {
                spriteSavePathService.SetCurrentSavePath(saveFileDialog.FileName);
                File.WriteAllBytes(spriteSavePathService.GetCurrentSavePath()!, spriteSerializationService.SerializeSprite());
                spriteGridStateService.SetDirtyStatus(false);
                if (!spriteSavePathService.RecentFiles.Contains(saveFileDialog.FileName))
                {
                    spriteSavePathService.RecentFiles.Insert(0, saveFileDialog.FileName);
                    Properties.Settings.Default.RecentFiles.Insert(0, saveFileDialog.FileName);
                    Properties.Settings.Default.Save();
                }
                else
                {
                    int idx = spriteSavePathService.RecentFiles.IndexOf(saveFileDialog.FileName);
                    if (idx > 0)
                    {
                        for (int i = 0; i < idx; i++)
                        {
                            string movedFile = spriteSavePathService.RecentFiles[i];
                            spriteSavePathService.RecentFiles[i + 1] = movedFile;
                            Properties.Settings.Default.RecentFiles[i + 1] = movedFile;
                        }
                        spriteSavePathService.RecentFiles[0] = saveFileDialog.FileName;
                        Properties.Settings.Default.RecentFiles[0] = saveFileDialog.FileName;
                        Properties.Settings.Default.Save();
                    }
                }
            }                        
        }
    }
}
