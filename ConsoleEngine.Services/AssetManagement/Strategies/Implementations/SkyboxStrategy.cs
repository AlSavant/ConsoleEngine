using DataModel.Attributes;
using DataModel.Rendering;
using System.IO;
using System.Xml.Serialization;

namespace ConsoleEngine.Services.AssetManagement.Strategies.Implementations
{
    [ResourceExtension(".sky")]
    internal sealed class SkyboxStrategy : ISerializationStrategy
    {
        private readonly IBinaryStrategy binaryStrategy;

        public SkyboxStrategy(IBinaryStrategy binaryStrategy)
        {
            this.binaryStrategy = binaryStrategy;
        }

        public object Deserialize(string path)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(SkyboxMaterial));
                using (Stream reader = new FileStream(path, FileMode.Open))
                {
                    var mat = (SkyboxMaterial)serializer.Deserialize(reader);
                    if (mat != null)
                    {
                        foreach (var layer in mat.layers)
                        {
                            layer.texture = (Sprite)binaryStrategy.Deserialize(layer.texturePath);
                        }                        
                    }
                    return mat;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
