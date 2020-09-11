using OpenTK;
using System.Drawing;
using MinecraftNetWindow.Geometry;

namespace MinecraftNetWindow.Units
{
    public class SizeF3D
    {
        public static readonly SizeF3D Empty = new SizeF3D(1, 1, 1);

        public SizeF3D(SizeF3D size)
        {
            Width = size.Width;
            Height = size.Height;
            Depth = size.Depth;
        }
        public SizeF3D(PointF3D pt)
        {
            Width = pt.X;
            Height = pt.Y;
            Depth = pt.Z;
        }

        public SizeF3D(float width, float height, float depth)
        {
            Width = width;
            Height = height;
            Depth = depth;
        }

        public bool IsEmpty { get => this == Empty; }

        public float Width { get; set; }
        public float Height { get; set; }
        public float Depth { get; set; }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(SizeF3D)) return false;
            else
            {
                var size = (SizeF3D)obj;

                return Width == size.Width &&
                       Height == size.Height &&
                       Depth == size.Depth;
            }
        }
        public override int GetHashCode()
        {
            return Width.GetHashCode() +
                   Height.GetHashCode() +
                   Depth.GetHashCode();
        }
        public override string ToString() => $"{{Width: {Width}, Height: {Height}, Depth: {Depth}}}";

        public PointF3D ToPointF3D()
        {
            return new PointF3D(Width, Height, Depth);
        }

        public static bool operator ==(SizeF3D sz1, SizeF3D sz2) => sz1.Equals(sz2);
        public static bool operator !=(SizeF3D sz1, SizeF3D sz2) => !sz1.Equals(sz2);

        public static explicit operator PointF3D(SizeF3D size) => size.ToPointF3D();

        public Matrix4 GetMatrix()
        {
            return Matrix4.CreateScale(Width, Height, Depth);
        }
    }
}