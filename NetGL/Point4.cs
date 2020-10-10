using NetGL.GraphicsAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetGL
{
    [Vector(4, GraphicsType.Float)]
    public struct Point4: IVector<float>
    {
        [VectorDimension(0)] public float X { get; set; }
        [VectorDimension(1)] public float Y { get; set; }
        [VectorDimension(2)] public float Z { get; set; }
        [VectorDimension(3)] public float W { get; set; }

        public float Length {
            get => (float)Math.Sqrt(LengthSquared);
            set {
                var x = X / Length * value;
                var y = Y / Length * value;
                var z = Z / Length * value;
                var w = W / Length * value;

                X = x; Y = y; Z = z; W = w;
            }
        }
        public float LengthSquared {
            get => X * X + Y * Y + Z * Z + W * W;
            set => Length = (float)Math.Sqrt(value);
        }

        public Point4(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
        public Point4(float val) : this(val, val, val, val) { }

        public float Dot(IVector<float> vector)
        {
            if (!(vector is Point4)) throw new Exception("Vector isn't instance of Point4");
            var vec = (Point4)vector;

            return X * vec.X + Y * vec.Y + Z * vec.Z;
        }

        public IVector<float> Add(IVector<float> obj)
        {
            if (!(obj is Point4)) throw new Exception("Vector isn't instance of Point4");
            var vec = (Point4)obj;

            return new Point4(X + vec.X, Y + vec.Y, Z + vec.Z, W + vec.W);
        }
        public IVector<float> Subtract(IVector<float> obj)
        {
            if (!(obj is Point4)) throw new Exception("Vector isn't instance of Point4");
            var vec = (Point4)obj;

            return new Point4(X - vec.X, Y - vec.Y, Z - vec.Z, W - vec.W);
        }
        public IVector<float> Multiply(float obj)
        {
            return new Point4(X * obj, Y * obj, Z * obj, W * obj);
        }
        public IVector<float> Divide(float obj)
        {
            return new Point4(X / obj, Y / obj, Z / obj, W / obj);
        }

        public float[] Flattern() => new[] { X, Y, Z, W };
    }
}
