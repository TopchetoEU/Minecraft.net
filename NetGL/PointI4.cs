using NetGL.GraphicsAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetGL
{
    [Vector(4, GraphicsType.Int)]
    public struct PointI4: IVector<int>
    {
        [VectorDimension(0)] public int X { get; set; }
        [VectorDimension(1)] public int Y { get; set; }
        [VectorDimension(2)] public int Z { get; set; }
        [VectorDimension(3)] public int W { get; set; }

        public float Length {
            get => (float)Math.Sqrt(LengthSquared);
            set {
                var x = (int)(X / Length * value);
                var y = (int)(Y / Length * value);
                var z = (int)(Z / Length * value);
                var w = (int)(W / Length * value);

                X = x; Y = y; Z = z; W = w;
            }
        }
        public float LengthSquared {
            get => X * X + Y * Y + Z * Z + W * W;
            set => Length = (float)Math.Sqrt(value);
        }

        public PointI4(int x, int y, int z, int w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
        public PointI4(int val) : this(val, val, val, val) { }

        public float Dot(IVector<int> vector)
        {
            if (!(vector is PointI4)) throw new Exception("Vector isn't instance of PointI4");
            var vec = (PointI4)vector;

            return X * vec.X + Y * vec.Y + Z * vec.Z + W * vec.W;
        }

        public IVector<int> Add(IVector<int> obj)
        {
            if (!(obj is PointI4)) throw new Exception("Vector isn't instance of PointI4");
            var vec = (PointI4)obj;

            return new PointI4(X + vec.X, Y + vec.Y, Z + vec.Z, W + vec.W);
        }
        public IVector<int> Subtract(IVector<int> obj)
        {
            if (!(obj is PointI4)) throw new Exception("Vector isn't instance of PointI4");
            var vec = (PointI4)obj;

            return new PointI4(X - vec.X, Y - vec.Y, Z - vec.Z, W * vec.W);
        }
        public IVector<int> Multiply(float obj)
        {
            return new PointI4((int)(X * obj), (int)(Y * obj), (int)(Z * obj), (int)(W * obj));
        }
        public IVector<int> Divide(float obj)
        {
            return new PointI4((int)(X / obj), (int)(Y / obj), (int)(Z / obj), (int)(W / obj));
        }

        public int[] Flattern() => new[] { X, Y, Z, W };
    }
}
