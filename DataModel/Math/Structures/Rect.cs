using System.Numerics;

namespace DataModel.Math.Structures
{
    public struct Rect
    {
        public Vector2 position;
        public Vector2 size;
        public float x
        {
            get
            {
                return position.X;
            }
            set
            {
                position.X = value;
            }
        }

        public float y
        {
            get
            {
                return position.Y;
            }
            set
            {
                position.Y = value;
            }
        }

        public float width
        {
            get
            {
                return size.X;
            }
            set
            {
                size.X = value;
            }
        }

        public float height
        {
            get
            {
                return size.Y;
            }
            set
            {
                size.Y = value;
            }
        }

        public Rect(float x, float y, float width, float height)
        {
            position = new Vector2(x, y);
            size = new Vector2(width, height);
        }

        public Rect(Vector2 position, Vector2 size)
        {
            this.position = position;
            this.size = size;
        }

        public Rect(float x, float y, Vector2 size)
        {
            position = new Vector2(x, y);
            this.size = size;
        }

        public Rect(Vector2 position, float width, float height)
        {
            this.position = position;
            size = new Vector2(width, height);
        }
    }
}
