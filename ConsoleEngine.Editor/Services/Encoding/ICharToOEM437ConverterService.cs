namespace ConsoleEngine.Editor.Services.Encoding
{
    internal interface ICharToOEM437ConverterService : IService
    {
        bool IsValidChar(char character);
        char[] GetRegisteredCharacters();
        byte CharToByte(char character);
        char ByteToChar(byte value);
        char GetInvalidCharacter();
    }
}
