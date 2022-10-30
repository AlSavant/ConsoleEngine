using System;
using System.Numerics;

namespace DataModel.Math.Structures
{
    public struct Vector3Int : IEquatable<Vector3Int>
    {
        public int x { get { return m_X; } set { m_X = value; } }
        public int y { get { return m_Y; } set { m_Y = value; } }
        public int z { get { return m_Z; } set { m_Z = value; } }

        private int m_X;
        private int m_Y;
        private int m_Z;

        public Vector3Int(int x, int y, int z)
        {
            m_X = x;
            m_Y = y;
            m_Z = z;
        }

        public void Set(int x, int y, int z)
        {
            m_X = x;
            m_Y = y;
            m_Z = z;
        }

        public int this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return x;
                    case 1: return y;
                    case 2: return z;
                    default:
                        throw new IndexOutOfRangeException(string.Format("Invalid Vector3Int index addressed: {0}!", index));
                }
            }

            set
            {
                switch (index)
                {
                    case 0: x = value; break;
                    case 1: y = value; break;
                    case 2: z = value; break;
                    default:
                        throw new IndexOutOfRangeException(string.Format("Invalid Vector3Int index addressed: {0}!", index));
                }
            }
        }

        public float magnitude { get { return (float)System.Math.Sqrt((float)(x * x + y * y + z * z)); } }

        public int sqrMagnitude { get { return x * x + y * y + z * z; } }

        public static float Distance(Vector3Int a, Vector3Int b) { return (a - b).magnitude; }

        public static Vector3Int Min(Vector3Int lhs, Vector3Int rhs) { return new Vector3Int(System.Math.Min(lhs.x, rhs.x), System.Math.Min(lhs.y, rhs.y), System.Math.Min(lhs.z, rhs.z)); }

        public static Vector3Int Max(Vector3Int lhs, Vector3Int rhs) { return new Vector3Int(System.Math.Max(lhs.x, rhs.x), System.Math.Max(lhs.y, rhs.y), System.Math.Max(lhs.z, rhs.z)); }

        public static Vector3Int Scale(Vector3Int a, Vector3Int b) { return new Vector3Int(a.x * b.x, a.y * b.y, a.z * b.z); }

        public void Scale(Vector3Int scale) { x *= scale.x; y *= scale.y; z *= scale.z; }

        public void Clamp(Vector3Int min, Vector3Int max)
        {
            x = System.Math.Max(min.x, x);
            x = System.Math.Min(max.x, x);
            y = System.Math.Max(min.y, y);
            y = System.Math.Min(max.y, y);
            z = System.Math.Max(min.z, z);
            z = System.Math.Min(max.z, z);
        }

        public static implicit operator Vector3(Vector3Int v)
        {
            return new Vector3(v.x, v.y, v.z);
        }

        public static explicit operator Vector2Int(Vector3Int v)
        {
            return new Vector2Int(v.x, v.y);
        }

        public static Vector3Int FloorToInt(Vector3 v)
        {
            return new Vector3Int(
                (int)System.Math.Floor(v.X),
                (int)System.Math.Floor(v.Y),
                (int)System.Math.Floor(v.Z)
            );
        }

        public static Vector3Int CeilToInt(Vector3 v)
        {
            return new Vector3Int(
                (int)System.Math.Ceiling(v.X),
                (int)System.Math.Ceiling(v.Y),
                (int)System.Math.Ceiling(v.Z)
            );
        }

        public static Vector3Int RoundToInt(Vector3 v)
        {
            return new Vector3Int(
                (int)System.Math.Round(v.X),
                (int)System.Math.Round(v.Y),
                (int)System.Math.Round(v.Z)
            );
        }

        public static Vector3Int operator +(Vector3Int a, Vector3Int b)
        {
            return new Vector3Int(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vector3Int operator -(Vector3Int a, Vector3Int b)
        {
            return new Vector3Int(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static Vector3Int operator *(Vector3Int a, Vector3Int b)
        {
            return new Vector3Int(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        public static Vector3Int operator -(Vector3Int a)
        {
            return new Vector3Int(-a.x, -a.y, -a.z);
        }

        public static Vector3Int operator *(Vector3Int a, int b)
        {
            return new Vector3Int(a.x * b, a.y * b, a.z * b);
        }

        public static Vector3Int operator *(int a, Vector3Int b)
        {
            return new Vector3Int(a * b.x, a * b.y, a * b.z);
        }

        public static Vector3Int operator /(Vector3Int a, int b)
        {
            return new Vector3Int(a.x / b, a.y / b, a.z / b);
        }

        public static bool operator ==(Vector3Int lhs, Vector3Int rhs)
        {
            return lhs.x == rhs.x && lhs.y == rhs.y && lhs.z == rhs.z;
        }

        public static bool operator !=(Vector3Int lhs, Vector3Int rhs)
        {
            return !(lhs == rhs);
        }

        public override bool Equals(object other)
        {
            if (!(other is Vector3Int)) return false;

            return Equals((Vector3Int)other);
        }

        public bool Equals(Vector3Int other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            var yHash = y.GetHashCode();
            var zHash = z.GetHashCode();
            return x.GetHashCode() ^ (yHash << 4) ^ (yHash >> 28) ^ (zHash >> 4) ^ (zHash << 28);
        }

        public static Vector3Int zero { get { return s_Zero; } }
        public static Vector3Int one { get { return s_One; } }
        public static Vector3Int up { get { return s_Up; } }
        public static Vector3Int down { get { return s_Down; } }
        public static Vector3Int left { get { return s_Left; } }
        public static Vector3Int right { get { return s_Right; } }

        private static readonly Vector3Int s_Zero = new Vector3Int(0, 0, 0);
        private static readonly Vector3Int s_One = new Vector3Int(1, 1, 1);
        private static readonly Vector3Int s_Up = new Vector3Int(0, 1, 0);
        private static readonly Vector3Int s_Down = new Vector3Int(0, -1, 0);
        private static readonly Vector3Int s_Left = new Vector3Int(-1, 0, 0);
        private static readonly Vector3Int s_Right = new Vector3Int(1, 0, 0);
    }
}
