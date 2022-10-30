using DataModel.StaticData.Entity;

namespace ConsoleEngine.Services.Factories
{
    public interface IStaticDataFactory : IService
    {        
        IEntityStaticData[] GetEntityStaticData();
        IEntityStaticData GetStaticData(string path);
    }
}
