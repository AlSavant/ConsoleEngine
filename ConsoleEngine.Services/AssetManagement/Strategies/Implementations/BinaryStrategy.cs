using DataModel.Attributes;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ConsoleEngine.Services.AssetManagement.Strategies.Implementations
{
    [ResourceExtension(".csp")]
    internal sealed class BinaryStrategy : IBinaryStrategy
    {
        public object Deserialize(string path)
        {
            try
            {
                var formatter = new BinaryFormatter();
                var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
                var val = formatter.Deserialize(stream);
                stream.Close();
                return val;
            }
            catch
            {
                return null;
            }
        }
    }
}
