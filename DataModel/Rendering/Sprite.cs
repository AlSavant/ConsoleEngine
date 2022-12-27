using LiteNetLib.Utils;
using System;
using System.Numerics;

namespace DataModel.Rendering
{
    [Serializable]
    public sealed class Sprite : INetSerializable
    {
        public bool isTransparent { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public byte[] characters { get; set; }
        public byte[] colors { get; set; }

        public Sprite() { }
        public Sprite(int width, int height, byte[] characters, byte[] colors, bool isTransparent)
        {
            this.width = width;
            this.height = height;
            this.characters = characters;
            this.colors = colors;
            this.isTransparent = isTransparent;
        }

        public byte[] SamplePixel(Vector2 uv)
        {
            int x = (int)System.Math.Floor(uv.X * width);
            int y = (int)System.Math.Floor(uv.Y * height);
            int index = y * width + x;
            if (index < 0 || index >= characters.Length || index >= colors.Length)
                return null;
            return new byte[] { characters[index], colors[index] };
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(isTransparent);
            writer.Put(width);
            writer.Put(height);
            writer.PutBytesWithLength(characters);
            writer.PutBytesWithLength(colors);
        }

        public void Deserialize(NetDataReader reader)
        {
            isTransparent = reader.GetBool();
            width = reader.GetInt();
            height = reader.GetInt();
            characters = reader.GetBytesWithLength();
            colors = reader.GetBytesWithLength();
        }
    }
}
