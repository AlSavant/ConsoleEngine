using ConsoleEngine.Editor.Model;
using ConsoleEngine.Editor.Services.Encoding;
using ConsoleEngine.Editor.Services.SpriteGrid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ConsoleEngine.Editor.ViewModels.Implementations
{
    internal sealed class SpriteToolbarViewModel : ViewModel, ISpriteToolbarViewModel
    {
        public ObservableCollection<char> CharacterList { get; set; }
        public ObservableCollection<ColorEntry> ColorList { get; set; }
        public ObservableCollection<SpriteToolPresetViewModel> SpriteTools { get; set; }

        public int SelectedToolbarPreset 
        {
            get
            {
                return (int)spriteToolbarStateService.GetSelectedToolPreset();
            }
            set
            {
                var currentPreset = (int)spriteToolbarStateService.GetSelectedToolPreset();
                if(currentPreset != value)
                {
                    spriteToolbarStateService.SelectToolPreset((ESpriteToolPreset)value);
                    OnPropertyChanged(nameof(SelectedToolbarPreset));
                }
            }
        }

        private readonly Dictionary<ESpriteToolPreset, SpriteToolPresetViewModel> spriteToolMap;
        private readonly ISpriteToolbarStateService spriteToolbarStateService;

        public SpriteToolbarViewModel(ICharToOEM437ConverterService charToOEM437ConverterService, ISpriteToolbarStateService spriteToolbarStateService)
        {
            this.spriteToolbarStateService = spriteToolbarStateService;
            CharacterList = new ObservableCollection<char>();
            var registeredCharacters = charToOEM437ConverterService.GetRegisteredCharacters();
            for (int i = 0; i < registeredCharacters.Length; i++)
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

            spriteToolMap = new Dictionary<ESpriteToolPreset, SpriteToolPresetViewModel>() {
                { ESpriteToolPreset.Paint, new SpriteToolPresetViewModel() { Preset = ESpriteToolPreset.Paint, ListIcon = "/Resources/brush.png", CursorIcon = "/Resources/Cursors/brush.cur" } },
                { ESpriteToolPreset.Line, new SpriteToolPresetViewModel() { Preset = ESpriteToolPreset.Line, ListIcon = "/Resources/line.png", CursorIcon = "/Resources/Cursors/brush.cur" } },
                { ESpriteToolPreset.Selection, new SpriteToolPresetViewModel() { Preset = ESpriteToolPreset.Selection, ListIcon = "/Resources/selection.png", CursorIcon = "Cross" } },
                { ESpriteToolPreset.Bucket, new SpriteToolPresetViewModel() { Preset = ESpriteToolPreset.Bucket, ListIcon = "/Resources/bucket.png", CursorIcon = "/Resources/Cursors/bucket.cur"} } };
            
            SpriteTools = new ObservableCollection<SpriteToolPresetViewModel>(spriteToolMap.Values);

            spriteToolbarStateService.SelectCharacter(CharacterList[0]);
            spriteToolbarStateService.SelectColor(ColorList[0]); 
            spriteToolbarStateService.SelectToolPreset(ESpriteToolPreset.Paint);
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
