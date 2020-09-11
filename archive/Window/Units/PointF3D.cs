using OpenTK;
using System;
using MinecraftNetWindow.Geometry;
using System.Drawing;

namespace MinecraftNetWindow.Units
{
    public class PointF3D
    {
        public static readonly PointF3D Empty = new PointF3D(0, 0, 0);

        public PointF3D(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public bool IsEmpty { get => this == Empty; }

        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(PointF3D)) return false;
            else
            {
                var point = (PointF3D)obj;
                return X == point.X &&
                       Y == point.Y &&
                       Z == point.Z;
            }
        }
        public override int GetHashCode()
        {
            return X.GetHashCode() +
                   Y.GetHashCode() +
                   Z.GetHashCode();
        }
        public override string ToString()
        {
            return $"{{X: {X}, Y: {Y}, Z: {Z}}}";
        }

        public static bool operator ==(PointF3D left, PointF3D right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(PointF3D left, PointF3D right)
        {
            return !left.Equals(right);
        }

        public Matrix4 GetMatrix()
        {
            return Matrix4.CreateTranslation(X, Y, Z);
        }

        public static implicit operator Vector2(PointF3D point)
        {
            return new Vector2(point.X, point.Y);
        }
        public static implicit operator Vector3(PointF3D point)
        {
            return new Vector3(point.X, point.Y, point.Z);
        }
        public static implicit operator Vector4(PointF3D point)
        {
            return new Vector4(point.X, point.Y, point.Z, 1);
        }
        public static implicit operator PointF3D(Vector2 vector)
        {
            return new PointF3D(vector.X, vector.Y, 0);
        }
        public static implicit operator PointF3D(Vector3 vector)
        {
            return new PointF3D(vector.X, vector.Y, vector.Z);
        }
        public static implicit operator PointF3D(Vector4 vector)
        {
            return new PointF3D(vector.X, vector.Y, vector.Z);
        }

        public static PointF3D operator +(PointF3D a, PointF3D b)
        {
            return new PointF3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }
        public static PointF3D operator -(PointF3D a, PointF3D b)
        {
            return new PointF3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static PointF3D operator *(PointF3D a, SizeF3D b)
        {
            return new PointF3D(a.X * b.Width, a.Y * b.Height, a.Z * b.Depth);
        }
        public static PointF3D operator /(PointF3D a, SizeF3D b)
        {
            return new PointF3D(a.X / b.Width, a.Y / b.Height, a.Z / b.Depth);
        }

        public static PointF3D operator *(PointF3D a, float b)
        {
            return new PointF3D(a.X * b, a.Y * b, a.Z * b);
        }
        public static PointF3D operator /(PointF3D a, float b)
        {
            return new PointF3D(a.X / b, a.Y / b, a.Z / b);
        }
    }
}
