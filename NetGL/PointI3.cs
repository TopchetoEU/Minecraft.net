using NetGL.GraphicsAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetGL
{
    [Vector(3, GraphicsType.Int)]
    public struct PointI3: IVector<int>
    {
        [VectorDimension(0)] public int X { get; set; }
        [VectorDimension(1)] public int Y { get; set; }
        [VectorDimension(2)] public int Z { get; set; }

        public float Length {
            get => (float)Math.Sqrt(LengthSquared);
            set {
                var x = (int)(X / Length * value);
                var y = (int)(Y / Length * value);
                var z = (int)(Z / Length * value);

                X = x; Y = y; Z = z;
            }
        }
        public float LengthSquared {
            get => X * X + Y * Y + Z * Z;
            set => Length = (float)Math.Sqrt(value);
        }

        public PointI3(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public PointI3(int val) : this(val, val, val) { }

        public float Dot(IVector<int> vector)
        {
            if (!(vector is PointI3)) throw new Exception("Vector isn't instance of PointI3");
            var vec = (PointI3)vector;

            return X * vec.X + Y * vec.Y + Z * vec.Z;
        }

        public IVector<int> Add(IVector<int> obj)
        {
            if (!(obj is PointI3)) throw new Exception("Vector isn't instance of PointI3");
            var vec = (PointI3)obj;

            return new PointI3(X + vec.X, Y + vec.Y, Z + vec.Z);
        }
        public IVector<int> Subtract(IVector<int> obj)
        {
            if (!(obj is PointI3)) throw new Exception("Vector isn't instance of PointI3");
            var vec = (PointI3)obj;

            return new PointI3(X - vec.X, Y - vec.Y, Z - vec.Z);
        }
        public IVector<int> Multiply(float obj)
        {
            return new PointI3((int)(X * obj), (int)(Y * obj), (int)(Z * obj));
        }
        public IVector<int> Divide(float obj)
        {
            return new PointI3((int)(X / obj), (int)(Y / obj), (int)(Z / obj));
        }

        public int[] Flattern() => new[] { X, Y, Z };
    }
}
