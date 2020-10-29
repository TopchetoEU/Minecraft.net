using NetGL.GraphicsAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetGL
{
    [Vector(3, MultiDimensionType.Int)]
    public struct VectorI3: IVector<int>
    {
        [VectorDimension(0)] public int X { get; set; }
        [VectorDimension(1)] public int Y { get; set; }
        [VectorDimension(2)] public int Z { get; set; }

        public float Length {
            get => (float)Math.Sqrt(LengthSquared);
            set {
                var x = X / Length * value;
                var y = Y / Length * value;
                var z = Z / Length * value;

                X = (int)x; Y = (int)y; Z = (int)z;
            }
        }
        public float LengthSquared {
            get => X * X + Y * Y + Z * Z;
            set => Length = (float)Math.Sqrt(value);
        }

        public VectorI3(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public VectorI3(VectorI2 vec, int z) : this(vec.X, vec.Y, z) { }
        public VectorI3(int val) : this(val, val, val) { }

        public int Dot(IVector<int> vector)
        {
            if (!(vector is VectorI3)) throw new Exception("Vector isn't instance of Point2");
            var vec = (VectorI3)vector;

            return X * vec.X + Y * vec.Y + Z * vec.Z;
        }

        public IVector<int> Add(IVector<int> obj)
        {
            if (!(obj is VectorI3)) throw new Exception("Vector isn't instance of Point2");
            var vec = (VectorI3)obj;

            return new VectorI3(X + vec.X, Y + vec.Y, Z + vec.Z);
        }
        public IVector<int> Subtract(IVector<int> obj)
        {
            if (!(obj is VectorI3)) throw new Exception("Vector isn't instance of Point2");
            var vec = (VectorI3)obj;

            return new VectorI3(X - vec.X, Y - vec.Y, Z - vec.Z);
        }
        public IVector<int> Multiply(float obj)
        {
            return new VectorI3((int)(X * obj), (int)(Y * obj), (int)(Z * obj));
        }
        public IVector<int> Divide(float obj)
        {
            return new VectorI3((int)(X / obj), (int)(Y / obj), (int)(Z / obj));
        }

        public int[] Flattern() => new[] { X, Y, Z };

        public static implicit operator Vector3(VectorI3 pt)
        {
            return new Vector3(pt.X, pt.Y, pt.Z);
        }

        public static VectorI3 operator +(VectorI3 a, VectorI3 b) => (VectorI3)a.Add(b);
        public static VectorI3 operator -(VectorI3 a, VectorI3 b) => (VectorI3)a.Subtract(b);
        public static VectorI3 operator /(VectorI3 a, float b) => (VectorI3)a.Divide(b);
        public static VectorI3 operator *(VectorI3 a, float b) => (VectorI3)a.Multiply(b);
    }
}
