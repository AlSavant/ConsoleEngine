using ConsoleEngine.Editor.Model;
using DataModel.ComponentModel;
using System;

namespace ConsoleEngine.Editor.Services.SpriteGrid.Implementations
{
    internal sealed class SpriteToolbarStateService : ISpriteToolbarStateService
    {
        public Action<INotifyPropertyChanged, IPropertyChangedEventArgs>? PropertyChanged { get; set; }

        private ESpriteToolPreset selectedToolPreset;
        private bool canPaintCharacters = true;
        private ColorEntry selectedColor;
        private char selectedCharacter;
        private string? importedArt;        

        public ESpriteToolPreset GetSelectedToolPreset()
        {
            return selectedToolPreset;
        }

        public void SelectToolPreset(ESpriteToolPreset preset)
        {
            if(selectedToolPreset != preset)
            {
                selectedToolPreset = preset;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SelectedToolPreset"));
            }
            
        }

        public string? GetImportedArt()
        {
            return importedArt;
        }

        public void SetImportedArt(string? importedArt)
        {
            if (this.importedArt == importedArt)
                return;
            this.importedArt = importedArt;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Imported Art"));
        }

        public void SetCharacterPaintingState(bool enabled)
        {
            canPaintCharacters = enabled;
        }

        public bool CanPaintCharacters()
        {
            return canPaintCharacters;
        }

        public void SelectColor(ColorEntry colorEntry)
        {
            selectedColor = colorEntry;
        }

        public ColorEntry GetSelectedColor()
        {
            return selectedColor;
        }

        public void SelectCharacter(char character)
        {
            selectedCharacter = character;
        }

        public char GetSelectedCharacter()
        {
            return selectedCharacter;
        }
    }
}
