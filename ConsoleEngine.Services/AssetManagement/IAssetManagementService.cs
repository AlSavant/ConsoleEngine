namespace ConsoleEngine.Services.AssetManagement
{
    public interface IAssetManagementService : IService
    {
        T[] LoadAll<T>(string path);
        T Load<T>(string path);
        void Unload(object asset);
        void UnloadAll();
    }
}
