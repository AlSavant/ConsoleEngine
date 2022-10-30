using ConsoleEngine.Services.AssetManagement.Strategies;
using ConsoleEngine.Services.Factories;
using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleEngine.Services.AssetManagement.Implementations
{
    internal sealed class AssetManagementService : IAssetManagementService
    {
        private readonly Dictionary<string, ISerializationStrategy> serializers;
        private readonly Dictionary<string, object> pool;
        private readonly string root;
        private readonly bool pathExists;

        public AssetManagementService(ISerializationStrategyFactory serializationStrategyFactory)
        {            
            root = AppDomain.CurrentDomain.BaseDirectory;
            root += "/Resources/";
            if (!Directory.Exists(root))
            {
                pathExists = false;
                return;
            }
            pathExists = true;

            pool = new Dictionary<string, object>();
            serializers = serializationStrategyFactory.CreateInstances();
        }

        public T Load<T>(string path)
        {
            if (!pathExists)
                return default;
            if (pool.ContainsKey(path))
            {
                return (T)pool[path];
            }
            string directory = Path.GetDirectoryName(root + path);
            string fileName = Path.GetFileName(root + path);

            DirectoryInfo dir = new DirectoryInfo(directory);
            FileInfo[] files = dir.GetFiles(fileName + ".*");
            if (files.Length < 0)
                return default;

            FileInfo file = files[0];
            var ext = file.Extension;
            if (!serializers.ContainsKey(ext))
                return default;
            var val = (T)serializers[ext].Deserialize(file.FullName);
            if (val == null)
                return default;
            pool.Add(path, val);
            return val;
        }

        public void Unload(object asset)
        {
            var keys = pool.Keys;
            string assetKey = string.Empty;
            foreach (var key in keys)
            {
                if (pool[key] == asset)
                {
                    assetKey = key;
                    break;
                }
            }
            pool.Remove(assetKey);
        }

        public void UnloadAll()
        {
            pool.Clear();
        }
    }
}
