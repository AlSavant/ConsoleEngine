using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleEngine.Editor.Services.Encoding.Implementations
{
    internal sealed class CharToOEM437ConverterService : ICharToOEM437ConverterService
    {
        private readonly Dictionary<char, byte> characterMap;
        private readonly char[] registeredCharacters;
        private readonly System.Text.Encoding encoding;

        public CharToOEM437ConverterService()
        {
            characterMap = new Dictionary<char, byte>();            
            encoding = CodePagesEncodingProvider.Instance.GetEncoding(437)!;
            if(encoding == null)
            {
                throw new InvalidOperationException("Could not resolve OEM 437 Encoding.");
            }
            List<char> characters = new List<char>();
            if (encoding != null)
            {
                var space = encoding.GetString(new byte[] { 32 });
                characterMap.Add(space[0], 32);
                characters.Add(space[0]);
                for (byte i = 0; i < 255; i++)
                {
                    char c = (char)i;
                    if (char.IsControl(c))
                        continue;
                    if (char.IsWhiteSpace(c))
                        continue;
                    var s = encoding.GetString(new byte[] { i });
                    characterMap.Add(s[0], i);
                    characters.Add(s[0]);
                }
            }
            registeredCharacters = characters.ToArray();
        }

        public char[] GetRegisteredCharacters()
        {
            return registeredCharacters;
        }

        public byte CharToByte(char character)
        {
            if(!characterMap.ContainsKey(character))
                return 0;
            return characterMap[character];
        }

        public char ByteToChar(byte value)
        {            
            var s = encoding.GetString(new byte[] { value });
            return s[0];
        }
    }
}
