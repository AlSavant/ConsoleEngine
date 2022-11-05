using System;
using System.Collections.Generic;
using System.Text;

namespace DataModel.Rendering
{
    public sealed class Terrain
    {
        public int width;
        public int height;
        public Material[] walls;
        public bool[] collisionMatrix;
        public byte[] floor;
    }
}
