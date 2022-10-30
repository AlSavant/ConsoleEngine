using System;
using System.Xml.Serialization;

namespace DataModel.Rendering
{
    [Serializable]
    public class SkyboxLayer
    {
        public float rotation;
        public string texturePath;

        [XmlIgnore]
        public Sprite texture;
    }
}
