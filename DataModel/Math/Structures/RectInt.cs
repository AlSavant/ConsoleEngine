﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace DataModel.Math.Structures
{
    public struct RectInt : IEquatable<RectInt>
    {
        private int m_XMin, m_YMin, m_Width, m_Height;
        public int x { get { return m_XMin; } set { m_XMin = value; } }
        public int y { get { return m_YMin; } set { m_YMin = value; } }
        public Vector2 center { get { return new Vector2(x + m_Width / 2f, y + m_Height / 2f); } }
        public Vector2Int min { get { return new Vector2Int(xMin, yMin); } set { xMin = value.x; yMin = value.y; } }
        public Vector2Int max { get { return new Vector2Int(xMax, yMax); } set { xMax = value.x; yMax = value.y; } }
        public int width { get { return m_Width; } set { m_Width = value; } }
        public int height { get { return m_Height; } set { m_Height = value; } }

        public int xMin { get { return System.Math.Min(m_XMin, m_XMin + m_Width); } set { int oldxmax = xMax; m_XMin = value; m_Width = oldxmax - m_XMin; } }
        public int yMin { get { return System.Math.Min(m_YMin, m_YMin + m_Height); } set { int oldymax = yMax; m_YMin = value; m_Height = oldymax - m_YMin; } }
        public int xMax { get { return System.Math.Max(m_XMin, m_XMin + m_Width); } set { m_Width = value - m_XMin; } }
        public int yMax { get { return System.Math.Max(m_YMin, m_YMin + m_Height); } set { m_Height = value - m_YMin; } }

        public Vector2Int position { get { return new Vector2Int(m_XMin, m_YMin); } set { m_XMin = value.x; m_YMin = value.y; } }
        public Vector2Int size { get { return new Vector2Int(m_Width, m_Height); } set { m_Width = value.x; m_Height = value.y; } }

        public void SetMinMax(Vector2Int minPosition, Vector2Int maxPosition)
        {
            min = minPosition;
            max = maxPosition;
        }

        public RectInt(int xMin, int yMin, int width, int height)
        {
            m_XMin = xMin;
            m_YMin = yMin;
            m_Width = width;
            m_Height = height;
        }

        public RectInt(Vector2Int position, Vector2Int size)
        {
            m_XMin = position.x;
            m_YMin = position.y;
            m_Width = size.x;
            m_Height = size.y;
        }

        public void ClampToBounds(RectInt bounds)
        {
            position = new Vector2Int(
                System.Math.Max(System.Math.Min(bounds.xMax, position.x), bounds.xMin),
                System.Math.Max(System.Math.Min(bounds.yMax, position.y), bounds.yMin)
            );
            size = new Vector2Int(
                System.Math.Min(bounds.xMax - position.x, size.x),
                System.Math.Min(bounds.yMax - position.y, size.y)
            );
        }

        public bool Contains(Vector2Int position)
        {
            return position.x >= xMin
                && position.y >= yMin
                && position.x < xMax
                && position.y < yMax;
        }

        public bool Overlaps(RectInt other)
        {
            return other.xMin < xMax
                && other.xMax > xMin
                && other.yMin < yMax
                && other.yMax > yMin;
        }



        public bool Equals(RectInt other)
        {
            return m_XMin == other.m_XMin &&
                m_YMin == other.m_YMin &&
                m_Width == other.m_Width &&
                m_Height == other.m_Height;
        }

        public PositionEnumerator allPositionsWithin
        {
            get { return new PositionEnumerator(min, max); }
        }

        public struct PositionEnumerator : IEnumerator<Vector2Int>
        {
            private readonly Vector2Int _min, _max;
            private Vector2Int _current;

            public PositionEnumerator(Vector2Int min, Vector2Int max)
            {
                _min = _current = min;
                _max = max;
                Reset();
            }

            public PositionEnumerator GetEnumerator()
            {
                return this;
            }

            public bool MoveNext()
            {
                if (_current.y >= _max.y)
                    return false;

                _current.x++;
                if (_current.x >= _max.x)
                {
                    _current.x = _min.x;
                    if (_current.x >= _max.x)
                        return false;

                    _current.y++;
                    if (_current.y >= _max.y)
                    {
                        return false;
                    }
                }

                return true;
            }

            public void Reset()
            {
                _current = _min;
                _current.x--;
            }

            public Vector2Int Current { get { return _current; } }

            object IEnumerator.Current { get { return Current; } }

            void IDisposable.Dispose() { }
        }
    }
}
