namespace ConsoleEngine.Editor.Services.Serialization
{
    internal interface ISpriteSerializationService : IService
    {
        byte[] SerializeSprite();
        void DeserializeSprite(byte[] spriteBytes);
    }
}
