using ConsoleEngine.Editor.Model;

namespace ConsoleEngine.Editor.Services.SpriteGrid
{
    internal interface ISpriteToolbarStateService : IService
    {
        void SetCharacterPaintingState(bool enabled);
        bool CanPaintCharacters();
        void SelectColor(ColorEntry colorEntry);
        ColorEntry GetSelectedColor();
        void SelectCharacter(char character);
        char GetSelectedCharacter();
    }
}
