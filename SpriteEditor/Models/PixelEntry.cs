using SpriteEditor.Models;

namespace SpriteEditor.ViewModels
{
    internal sealed class PixelEntry : ViewModel
    {
        public static PixelEntry Default
        {
            get
            {
                var entry = new PixelEntry();
                entry.Character = ' ';
                entry.Color = ColorEntry.FromConsoleColor(System.ConsoleColor.Black);
                return entry;
            }
        }

        private char character;
        public char Character
        {
            get
            {
                return character;
            }
            set
            {
                SetProperty(ref character, value, nameof(Character));
            }
        }

        private ColorEntry color;
        public ColorEntry Color
        {
            get
            {
                return color;
            }
            set
            {
                SetProperty(ref color, value, nameof(Color));
            }
        }

        public PixelEntry Clone()
        {
            var clone = new PixelEntry();
            clone.Character = Character;
            clone.Color = Color;
            return clone;
        }
    }
}
