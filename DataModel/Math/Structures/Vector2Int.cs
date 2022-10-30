using System;
using System.Numerics;

namespace DataModel.Math.Structures
{
    public struct Vector2Int : IEquatable<Vector2Int>
    {
        public int x
        {
            get { return m_X; }
            set { m_X = value; }
        }


        public int y
        {
            get { return m_Y; }
            set { m_Y = value; }
        }

        private int m_X;
        private int m_Y;

        public Vector2Int(int x, int y)
        {
            m_X = x;
            m_Y = y;
        }

        public void Set(int x, int y)
        {
            m_X = x;
            m_Y = y;
        }

        public int this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return x;
                    case 1: return y;
                    default:
                        throw new IndexOutOfRangeException(string.Format("Invalid Vector2Int index addressed: {0}!", index));
                }
            }

            set
            {
                switch (index)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    default:
                        throw new IndexOutOfRangeException(string.Format("Invalid Vector2Int index addressed: {0}!", index));
                }
            }
        }

        public float magnitude { get { return (float)System.Math.Sqrt((float)(x * x + y * y)); } }
        public int sqrMagnitude { get { return x * x + y * y; } }
        public static float Distance(Vector2Int a, Vector2Int b)
        {
            float diff_x = a.x - b.x;
            float diff_y = a.y - b.y;

            return (float)System.Math.Sqrt(diff_x * diff_x + diff_y * diff_y);
        }

        public static Vector2Int Min(Vector2Int lhs, Vector2Int rhs) { return new Vector2Int(System.Math.Min(lhs.x, rhs.x), System.Math.Min(lhs.y, rhs.y)); }
        public static Vector2Int Max(Vector2Int lhs, Vector2Int rhs) { return new Vector2Int(System.Math.Max(lhs.x, rhs.x), System.Math.Max(lhs.y, rhs.y)); }
        public static Vector2Int Scale(Vector2Int a, Vector2Int b) { return new Vector2Int(a.x * b.x, a.y * b.y); }
        public void Scale(Vector2Int scale) { x *= scale.x; y *= scale.y; }
        public void Clamp(Vector2Int min, Vector2Int max)
        {
            x = System.Math.Max(min.x, x);
            x = System.Math.Min(max.x, x);
            y = System.Math.Max(min.y, y);
            y = System.Math.Min(max.y, y);
        }

        public static implicit operator Vector2(Vector2Int v)
        {
            return new Vector2(v.x, v.y);
        }

        public static explicit operator Vector3Int(Vector2Int v)
        {
            return new Vector3Int(v.x, v.y, 0);
        }

        public static Vector2Int FloorToInt(Vector2 v)
        {
            return new Vector2Int(
                (int)System.Math.Floor(v.X),
                (int)System.Math.Floor(v.Y)
            );
        }

        public static Vector2Int CeilToInt(Vector2 v)
        {
            return new Vector2Int(
                (int)System.Math.Ceiling(v.X),
                (int)System.Math.Ceiling(v.Y)
            );
        }

        public static Vector2Int RoundToInt(Vector2 v)
        {
            return new Vector2Int(
                (int)System.Math.Round(v.X),
                (int)System.Math.Round(v.Y)
            );
        }

        public Vector2Int RotateClockwise()
        {
            return new Vector2Int(y, -x);
        }

        public Vector2Int RotateAntiClockwise()
        {
            return new Vector2Int(-y, x);
        }

        public static Vector2Int operator -(Vector2Int v)
        {
            return new Vector2Int(-v.x, -v.y);
        }

        public static Vector2Int operator +(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.x + b.x, a.y + b.y);
        }

        public static Vector2Int operator -(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.x - b.x, a.y - b.y);
        }

        public static Vector2Int operator *(Vector2Int a, Vector2Int b)
        {
            return new Vector2Int(a.x * b.x, a.y * b.y);
        }

        public static Vector2Int operator *(int a, Vector2Int b)
        {
            return new Vector2Int(a * b.x, a * b.y);
        }

        public static Vector2Int operator *(Vector2Int a, int b)
        {
            return new Vector2Int(a.x * b, a.y * b);
        }

        public static Vector2Int operator /(Vector2Int a, int b)
        {
            return new Vector2Int(a.x / b, a.y / b);
        }

        public static bool operator ==(Vector2Int lhs, Vector2Int rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y;
        }

        public static bool operator !=(Vector2Int lhs, Vector2Int rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object other)
        {
            if (!(other is Vector2Int)) return false;

            return Equals((Vector2Int)other);
        }

        public bool Equals(Vector2Int other)
        {
            return x == other.x && y == other.y;
        }

        public override int GetHashCode()
        {
            return x.GetHashCode() ^ (y.GetHashCode() << 2);
        }

        public override string ToString()
        {
            return $"({x}, {y})";
        }

        public static Vector2Int zero { get { return s_Zero; } }
        public static Vector2Int one { get { return s_One; } }
        public static Vector2Int forward { get { return s_Up; } }
        public static Vector2Int back { get { return s_Down; } }
        public static Vector2Int left { get { return s_Left; } }
        public static Vector2Int right { get { return s_Right; } }

        private static readonly Vector2Int s_Zero = new Vector2Int(0, 0);
        private static readonly Vector2Int s_One = new Vector2Int(1, 1);
        private static readonly Vector2Int s_Up = new Vector2Int(0, 1);
        private static readonly Vector2Int s_Down = new Vector2Int(0, -1);
        private static readonly Vector2Int s_Left = new Vector2Int(-1, 0);
        private static readonly Vector2Int s_Right = new Vector2Int(1, 0);
    }
}
