namespace ConsoleEngine.Services.AssetManagement.Strategies
{
    internal interface ISerializationStrategy : IService
    {
        object Deserialize(string path);
    }
}
