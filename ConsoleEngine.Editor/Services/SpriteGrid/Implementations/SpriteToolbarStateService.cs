using ConsoleEngine.Editor.Model;

namespace ConsoleEngine.Editor.Services.SpriteGrid.Implementations
{
    internal sealed class SpriteToolbarStateService : ISpriteToolbarStateService
    {
        private bool canPaintCharacters = true;
        private ColorEntry selectedColor;
        private char selectedCharacter;

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
