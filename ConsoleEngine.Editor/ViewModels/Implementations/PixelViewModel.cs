using ConsoleEngine.Editor.Model;

namespace ConsoleEngine.Editor.ViewModels.Implementations
{
    internal sealed class PixelViewModel : ViewModel
    {
        public static PixelViewModel Default
        {
            get
            {
                var entry = new PixelViewModel();
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

        public PixelViewModel Clone()
        {
            var clone = new PixelViewModel();
            clone.Character = Character;
            clone.Color = Color;
            return clone;
        }
    }
}
