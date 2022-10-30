using ConsoleEngine.Services.AssetManagement;
using DataModel.StaticData.Entity;
using DataModel.StaticData.Entity.Implementations;

namespace ConsoleEngine.Services.Factories.Implementations
{
    internal sealed class StaticDataFactory : IStaticDataFactory
    {
        private readonly IAssetManagementService assetManagementService;

        public StaticDataFactory(IAssetManagementService assetManagementService)
        {
            this.assetManagementService = assetManagementService;
        }

        public IEntityStaticData[] GetEntityStaticData()
        {
            var data = assetManagementService.LoadAll<EntityStaticData>("StaticData/Entity");
            var arr = new IEntityStaticData[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                arr[i] = data[i];
            }
            return arr;
        }

        public IEntityStaticData GetStaticData(string path)
        {
            return assetManagementService.Load<EntityStaticData>(path);
        }
    }
}
