using ConsoleEngine.Services.AssetManagement.Strategies;
using ConsoleEngine.Services.Factories;
using ConsoleEngine.Services.Util.Resources;
using DataModel.StaticData.Component.Implementations;
using DataModel.StaticData.Entity.Implementations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace ConsoleEngine.Services.AssetManagement.Implementations
{
    internal sealed class AssetManagementService : IAssetManagementService
    {
        private readonly IResourcePathParsingService resourcePathParsingService;

        private readonly XmlAttributeOverrides xmlAttributeOverrides;
        private readonly Dictionary<string, ISerializationStrategy> serializers;
        private readonly Dictionary<string, object> pool;

        public AssetManagementService(ISerializationStrategyFactory serializationStrategyFactory, IResourcePathParsingService resourcePathParsingService)
        {
            xmlAttributeOverrides = new XmlAttributeOverrides();
            var dataTypes = typeof(ComponentStaticData).Assembly.GetTypes().Where(x => !x.IsInterface && !x.IsAbstract && typeof(ComponentStaticData).IsAssignableFrom(x));
            var attributes = new XmlAttributes();
            foreach (var type in dataTypes)
            {
                attributes.XmlArrayItems.Add(new XmlArrayItemAttribute()
                {
                    ElementName = type.Name,
                    Type = type
                });
            }
            xmlAttributeOverrides.Add(typeof(EntityStaticData), "components", attributes);
            pool = new Dictionary<string, object>();
            serializers = serializationStrategyFactory.CreateInstances();
            this.resourcePathParsingService = resourcePathParsingService;
        }

        public T[] LoadAll<T>(string path)
        {
            var list = new List<T>();
            var fullPath = resourcePathParsingService.GetFullAssetPath(path);
            if (!Directory.Exists(fullPath))
            {
                return Array.Empty<T>();
            }
            var files = Directory.GetFiles(fullPath, "*.xml", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var formattedPath = resourcePathParsingService.GetFormattedPath(file);                                
                var resource = GetResource<T>(file);
                if (resource == null)
                {
                    continue;
                }
                list.Add(resource);
                if (!pool.ContainsKey(formattedPath))
                {
                    pool.Add(formattedPath, resource);                    
                }                
            }
            return list.ToArray();
        }

        private T GetResource<T>(string fullPath)
        {            
            try
            {
                var serializer = new XmlSerializer(typeof(T), xmlAttributeOverrides);
                using (Stream reader = new FileStream(fullPath, FileMode.Open))
                {
                    return (T)serializer.Deserialize(reader);                    
                }               
            }
            catch
            {
                return default;
            }            
        }

        public T Load<T>(string path)
        {
            if (!resourcePathParsingService.PathExists)
                return default;
            if (pool.ContainsKey(path))
            {
                return (T)pool[path];
            }
            string directory = resourcePathParsingService.GetDirectoryName(path);
            string fileName = resourcePathParsingService.GetFileName(path);

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
