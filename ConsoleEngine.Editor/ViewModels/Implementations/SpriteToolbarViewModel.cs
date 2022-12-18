using ConsoleEngine.Editor.Model;
using ConsoleEngine.Editor.Services.Encoding;
using ConsoleEngine.Editor.Services.SpriteGrid;
using System;
using System.Collections.ObjectModel;

namespace ConsoleEngine.Editor.ViewModels.Implementations
{
    internal sealed class SpriteToolbarViewModel : ViewModel, ISpriteToolbarViewModel
    {
        public ObservableCollection<char> CharacterList { get; set; }
        public ObservableCollection<ColorEntry> ColorList { get; set; }

        private readonly ISpriteToolbarStateService spriteToolbarStateService;

        public SpriteToolbarViewModel(ICharToOEM437ConverterService charToOEM437ConverterService, ISpriteToolbarStateService spriteToolbarStateService)
        {            
            this.spriteToolbarStateService = spriteToolbarStateService;
            CharacterList = new ObservableCollection<char>();
            var registeredCharacters = charToOEM437ConverterService.GetRegisteredCharacters();
            for(int i = 0; i < registeredCharacters.Length; i++) 
            {
                CharacterList.Add(registeredCharacters[i]);
            }            
            ColorList = new ObservableCollection<ColorEntry>();
            var consoleColors = (ConsoleColor[])Enum.GetValues(typeof(ConsoleColor));
            for (int i = 0; i < consoleColors.Length; i++)
            {
                var entry = ColorEntry.FromConsoleColor(consoleColors[i]);
                ColorList.Add(entry);
            }
            var last = ColorList[ColorList.Count - 1];
            ColorList[ColorList.Count - 1] = ColorList[0];
            ColorList[0] = last;
            spriteToolbarStateService.SelectCharacter(CharacterList[0]);
            spriteToolbarStateService.SelectColor(ColorList[0]);            
        }

        private int selectedCharacterIndex = 0;
        public int SelectedCharacterIndex
        {
            get
            {
                return selectedCharacterIndex;
            }
            set
            {
                if (SetProperty(ref selectedCharacterIndex, value, nameof(SelectedCharacterIndex)))
                {
                    spriteToolbarStateService.SelectCharacter(CharacterList[value]);                    
                }
            }
        }        

        private int selectedColorIndex = 0;
        public int SelectedColorIndex
        {
            get
            {
                return selectedColorIndex;
            }
            set
            {
                if (SetProperty(ref selectedColorIndex, value, nameof(SelectedColorIndex)))
                {
                    spriteToolbarStateService.SelectColor(ColorList[value]);                    

                }
            }
        }               
        
        public bool CanPaintCharacters
        {
            get
            {
                return spriteToolbarStateService.CanPaintCharacters();
            }
            set
            {
                if(value != spriteToolbarStateService.CanPaintCharacters())
                {
                    spriteToolbarStateService.SetCharacterPaintingState(value);
                    OnPropertyChanged(nameof(CanPaintCharacters));
                }                
            }
        }
    }
}
