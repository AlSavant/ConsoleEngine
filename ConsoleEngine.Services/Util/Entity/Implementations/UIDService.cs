using DataModel.Entity;
using System;
using System.Collections.Generic;

namespace ConsoleEngine.Services.Util.Entity.Implementations
{
    internal sealed class UIDService : IUIDService
    {
        private Dictionary<int, IEntity> networkIDToEntityMap;
        private Dictionary<IEntity, int> networkEntityToIDMap;
        private HashSet<int> occupiedIds;
        private Stack<int> releasedIDs;
        private int lastID;

        public UIDService()
        {
            releasedIDs = new Stack<int>();
            networkIDToEntityMap = new Dictionary<int, IEntity>();
            networkEntityToIDMap = new Dictionary<IEntity, int>();
            occupiedIds = new HashSet<int>();
            lastID = -1;
        }

        private bool HasAssociatedEntity(IEntity entity)
        {
            return !entity.IsNull() && networkEntityToIDMap.ContainsKey(entity);
        }

        public int GetUID(IEntity entity = null)
        {
            if (HasAssociatedEntity(entity))
            {
                return networkEntityToIDMap[entity];
            }

            int itemID;
            if (releasedIDs.Count > 0)
            {
                itemID = releasedIDs.Pop();
            }
            else
            {
                int count = lastID + 1;
                while (occupiedIds.Contains(count))
                    count += 1;
                lastID = count;
                itemID = lastID;
            }
            if (!entity.IsNull())
            {
                occupiedIds.Add(itemID);
                networkIDToEntityMap.Add(itemID, entity);
                networkEntityToIDMap.Add(entity, itemID);
            }
            return itemID;
        }

        public IEntity GetRegisteredEntity(int uid)
        {
            if (networkIDToEntityMap.ContainsKey(uid))
                return networkIDToEntityMap[uid];
            return null;
        }       

        public void RegisterUIDForEntity(IEntity entity, int uid)
        {
            if (networkIDToEntityMap.ContainsKey(uid))
            {
                if (networkIDToEntityMap[uid] == entity)
                    return;
                else
                    throw new InvalidOperationException($"Cannot manually assign UID {uid} for entity {entity.StaticData.ID} as it is already occupied by entity {networkIDToEntityMap[uid].StaticData.ID}!");
            }

            occupiedIds.Add(uid);
            networkIDToEntityMap.Add(uid, entity);
            networkEntityToIDMap.Add(entity, uid);
        }

        public void ReleaseUID(int uid)
        {
            if (networkIDToEntityMap.ContainsKey(uid))
            {
                var entity = networkIDToEntityMap[uid];
                networkEntityToIDMap.Remove(entity);
                networkIDToEntityMap.Remove(uid);
            }            
            occupiedIds.Remove(uid);
            releasedIDs.Push(uid);
        }

        public void Reset()
        {
            lastID = -1;
            releasedIDs.Clear();
            networkIDToEntityMap.Clear();
            networkEntityToIDMap.Clear();
            occupiedIds.Clear();
        }

        public void Reset(string scene)
        {
            List<int> toRemove = new List<int>();            
            for (int i = 0; i < toRemove.Count; i++)
            {
                ReleaseUID(toRemove[i]);
            }
        }

        public void SetMaxUID(int maxUID)
        {
            lastID = maxUID;
        }
    }
}
