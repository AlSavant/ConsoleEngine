using System;
using System.Numerics;
using System.Xml.Serialization;

namespace DataModel.Rendering
{
    [Serializable]
    public class Material
    {
        public string texturePath;
        public Vector2 tiling;
        public Vector2 offset;

        [XmlIgnore]
        public Sprite texture;

        public byte[] SamplePixel(Vector2 uv)
        {
            if (texture == null)
            {
                return new byte[] { 0, 0 };
            }
            Vector2 newUV = new Vector2(uv.X * tiling.X + offset.Y, uv.Y * tiling.Y + offset.Y);
            newUV.X = (float)(newUV.X - System.Math.Truncate(newUV.X));
            newUV.Y = (float)(newUV.Y - System.Math.Truncate(newUV.Y));
            return texture.SamplePixel(newUV);

        }
    }
}
