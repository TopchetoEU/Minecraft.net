using NetGL.GraphicsAPI;
using System;

namespace NetGL
{
    [Vector(2, GraphicsType.Int)]
    public struct PointI2
    {
        public int X { get; set; }
        public int Y { get; set; }

        public static PointI2 operator +(PointI2 a, PointI2 b)
        {
            return new PointI2(a.X + b.X, a.Y + b.Y);
        }
        public static PointI2 operator -(PointI2 a, PointI2 b)
        {
            return new PointI2(a.X - b.X, a.Y - b.Y);
        }

        public static PointI2 operator *(float a, PointI2 b)
        {
            return new PointI2((int)(a * b.X), (int)(a * b.Y));
        }
        public static PointI2 operator *(PointI2 b, float a)
        {
            return a * b;
        }

        public static PointI2 operator /(PointI2 b, float a)
        {
            return (1 / a) * b;
        }

        public float Length() => (float)Math.Sqrt(X * X + Y * Y);
        public int LengthSquared() => X * X + Y * Y;

        public PointI2(int x, int y)
        {
            X = x;
            Y = y;
        }
        public PointI2(PointI2 point) : this(point.X, point.Y) { }
    }
}
