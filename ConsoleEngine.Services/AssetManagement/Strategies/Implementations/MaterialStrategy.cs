using DataModel.Rendering;
using System.IO;
using System.Xml.Serialization;
using DataModel.Attributes;

namespace ConsoleEngine.Services.AssetManagement.Strategies.Implementations
{
    [ResourceExtension(".mat")]
    internal sealed class MaterialStrategy : IMaterialStrategy
    {
        private readonly IBinaryStrategy binaryStrategy;

        public MaterialStrategy(IBinaryStrategy binaryStrategy)
        {
            this.binaryStrategy = binaryStrategy;
        }

        public object Deserialize(string path)
        {
            try
            {
                var serializer = new XmlSerializer(typeof(Material));
                using (Stream reader = new FileStream(path, FileMode.Open))
                {
                    var mat = (Material)serializer.Deserialize(reader);
                    if (mat != null)
                    {
                        mat.texture = (Sprite)binaryStrategy.Deserialize(mat.texturePath);
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
