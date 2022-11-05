using ConsoleEngine.Services.Util.Resources;
using DataModel.Attributes;
using DataModel.Rendering;
using System.IO;
using System.Xml.Serialization;

namespace ConsoleEngine.Services.AssetManagement.Strategies.Implementations
{
    [ResourceExtension(".sky")]
    internal sealed class SkyboxStrategy : ISkyboxStrategy
    {
        private readonly IResourcePathParsingService resourcePathParsingService;
        private readonly IBinaryStrategy binaryStrategy;

        public SkyboxStrategy(IBinaryStrategy binaryStrategy, IResourcePathParsingService resourcePathParsingService)
        {
            this.binaryStrategy = binaryStrategy;
            this.resourcePathParsingService = resourcePathParsingService;
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
                            layer.texture = (Sprite)binaryStrategy.Deserialize(resourcePathParsingService.GetFullAssetPath(layer.texturePath));
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
