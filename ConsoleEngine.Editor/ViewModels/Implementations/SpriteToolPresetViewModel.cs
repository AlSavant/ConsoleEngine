using ConsoleEngine.Editor.Model;

namespace ConsoleEngine.Editor.ViewModels.Implementations
{
    internal sealed class SpriteToolPresetViewModel : ViewModel
    {
        private ESpriteToolPreset preset;
        public ESpriteToolPreset Preset 
        { 
            get 
            { 
                return preset;
            } 
            set
            {
                SetProperty(ref preset, value, nameof(Preset));
            }
        }

        private string? listIcon;
        public string? ListIcon
        {
            get
            {
                return listIcon;
            }
            set
            {
                SetProperty(ref listIcon, value, nameof(ListIcon));
            }
        }

        private string? cursorIcon;
        public string? CursorIcon
        {
            get
            {
                return cursorIcon;
            }
            set
            {
                SetProperty(ref cursorIcon, value, nameof(CursorIcon));
            }
        }
    };
}
