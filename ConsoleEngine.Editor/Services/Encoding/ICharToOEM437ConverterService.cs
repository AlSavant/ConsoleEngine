namespace ConsoleEngine.Editor.Services.Encoding
{
    internal interface ICharToOEM437ConverterService : IService
    {
        char[] GetRegisteredCharacters();
        byte CharToByte(char character);
        char ByteToChar(byte value);
    }
}
