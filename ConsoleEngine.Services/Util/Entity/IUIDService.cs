using DataModel.Entity;
using System.Collections.Generic;

namespace ConsoleEngine.Services.Util.Entity
{
    public interface IUIDService : IService
    {
        int GetUID(IEntity entity = null);
        void RegisterUIDForEntity(IEntity entity, int uid);
        IEntity GetRegisteredEntity(int uid);
        void ReleaseUID(int uid);
        void Reset();
        void SetMaxUID(int maxUID);
        void Reset(string scene);       
    }
}
