using ConsoleEngine.Editor.Model;
using DataModel.ComponentModel;

namespace ConsoleEngine.Editor.Services.SpriteGrid
{
    internal interface ISpriteToolbarStateService : IService, INotifyPropertyChanged
    {
        string? GetImportedArt();        
        void SetImportedArt(string? importedArt);
        void SetCharacterPaintingState(bool enabled);
        bool CanPaintCharacters();
        void SelectColor(ColorEntry colorEntry);
        ColorEntry GetSelectedColor();
        void SelectCharacter(char character);
        char GetSelectedCharacter();
    }
}
