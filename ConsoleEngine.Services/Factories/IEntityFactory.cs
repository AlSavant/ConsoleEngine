using DataModel.ComponentModel;
using DataModel.Entity;
using DataModel.StaticData.Entity;
using System.Collections.Generic;
using System.Numerics;

namespace ConsoleEngine.Services.Factories
{
    public interface IEntityFactory : IService, INotifyPropertyChanged
    {
        IEntity CreateInstance(string staticDataID);
        IEntity CreateInstance(string staticDataID, Vector2 position, float rotation, string scene, int id = -1);
        bool ContainsEntityID(string id);
        void AddStaticData(string id, IEntityStaticData data);
        IEntityStaticData GetData(string id);
        IEnumerable<string> GetStaticDataIDs();        
        void ReleaseAllEntities();        
    }
}
