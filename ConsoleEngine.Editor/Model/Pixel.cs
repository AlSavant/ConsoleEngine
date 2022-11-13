using System;

namespace ConsoleEngine.Editor.Model
{
    internal struct Pixel : IEquatable<Pixel>
    {
        public char character;
        public ColorEntry colorEntry;

        public Pixel()
        {
            character = ' ';
            colorEntry = ColorEntry.FromConsoleColor(ConsoleColor.Black);
        }

        public Pixel(char character, ColorEntry colorEntry)
        {
            this.character = character;
            this.colorEntry = colorEntry;
        }

        public bool Equals(Pixel other)
        {
            return Equals(other, this);
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var objectToCompareWith = (Pixel)obj;
            return objectToCompareWith.character == character && objectToCompareWith.colorEntry == colorEntry;
        }

        public override int GetHashCode()
        {            
            return character.GetHashCode() ^ colorEntry.GetHashCode();
        }

        public static bool operator ==(Pixel c1, Pixel c2)
        {
            return c1.Equals(c2);
        }

        public static bool operator !=(Pixel c1, Pixel c2)
        {
            return !c1.Equals(c2);
        }
    }
}
