using ConsoleEngine.Editor.Services.Commands.Implementations;
using ConsoleEngine.Editor.Services.IO;
using ConsoleEngine.Editor.Services.Serialization;
using ConsoleEngine.Editor.Services.SpriteGrid;
using System.IO;

namespace ConsoleEngine.Editor.Services.Commands.Serialization.Implementations
{
    internal sealed class SaveSpriteCommand : LogicCommand, ISaveSpriteCommand
    {
        private readonly ISpriteGridStateService spriteGridStateService;
        private readonly ISpriteSavePathService spriteSavePathService;
        private readonly ISpriteSerializationService spriteSerializationService;

        public SaveSpriteCommand(
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
            return spriteGridStateService.IsDirty() && !string.IsNullOrEmpty(spriteSavePathService.GetCurrentSavePath());
        }

        protected override void ExecuteCommand(object? parameter)
        {            
            File.WriteAllBytes(spriteSavePathService.GetCurrentSavePath()!, spriteSerializationService.SerializeSprite());
            spriteGridStateService.SetDirtyStatus(false);
        }
    }
}
