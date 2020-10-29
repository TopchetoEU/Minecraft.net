using NetGL.GraphicsAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetGL
{
    [Vector(4, MultiDimensionType.Int)]
    public struct VectorI4: IVector<int>
    {
        [VectorDimension(0)] public int X { get; set; }
        [VectorDimension(1)] public int Y { get; set; }
        [VectorDimension(2)] public int Z { get; set; }
        [VectorDimension(2)] public int W { get; set; }

        public float Length {
            get => (float)Math.Sqrt(LengthSquared);
            set {
                var x = X / Length * value;
                var y = Y / Length * value;
                var z = Z / Length * value;
                var w = W / Length * value;

                X = (int)x; Y = (int)y; Z = (int)z; W = (int)w;
            }
        }
        public float LengthSquared {
            get => X * X + Y * Y + Z * Z+ W * W;
            set => Length = (float)Math.Sqrt(value);
        }

        public VectorI4(int x, int y, int z, int w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
        public VectorI4(VectorI2 vec, int z, int w) : this(vec.X, vec.Y, z, w) { }
        public VectorI4(VectorI4 vec, int w) : this(vec.X, vec.Y, vec.Z, w) { }
        public VectorI4(int val) : this(val, val, val, val) { }

        public int Dot(IVector<int> vector)
        {
            if (!(vector is VectorI4)) throw new Exception("Vector isn't instance of VectorI4");
            var vec = (VectorI4)vector;

            return X * vec.X + Y * vec.Y + Z * vec.Z + W * vec.W;
        }

        public IVector<int> Add(IVector<int> obj)
        {
            if (!(obj is VectorI4)) throw new Exception("Vector isn't instance of VectorI4");
            var vec = (VectorI4)obj;

            return new VectorI4(X + vec.X, Y + vec.Y, Z + vec.Z, W + vec.W);
        }
        public IVector<int> Subtract(IVector<int> obj)
        {
            if (!(obj is VectorI4)) throw new Exception("Vector isn't instance of VectorI4");
            var vec = (VectorI4)obj;

            return new VectorI4(X - vec.X, Y - vec.Y, Z - vec.Z, W - vec.W);
        }
        public IVector<int> Multiply(float obj)
        {
            return new VectorI4((int)(X * obj), (int)(Y * obj), (int)(Z * obj), (int)(W * obj));
        }
        public IVector<int> Divide(float obj)
        {
            return new VectorI4((int)(X / obj), (int)(Y / obj), (int)(Z / obj), (int)(W * obj));
        }

        public int[] Flattern() => new[] { X, Y, Z, W };

        public static implicit operator Vector4(VectorI4 pt)
        {
            return new Vector4(pt.X, pt.Y, pt.Z, pt.W);
        }

        public static VectorI4 operator +(VectorI4 a, VectorI4 b) => (VectorI4)a.Add(b);
        public static VectorI4 operator -(VectorI4 a, VectorI4 b) => (VectorI4)a.Subtract(b);
        public static VectorI4 operator /(VectorI4 a, float b) => (VectorI4)a.Divide(b);
        public static VectorI4 operator *(VectorI4 a, float b) => (VectorI4)a.Multiply(b);
    }
}
