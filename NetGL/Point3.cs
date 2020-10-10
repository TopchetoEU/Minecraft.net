using NetGL.GraphicsAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetGL
{
    [Vector(3, GraphicsType.Float)]
    public struct Point3: IVector<float>
    {
        [VectorDimension(0)] public float X { get; set; }
        [VectorDimension(1)] public float Y { get; set; }
        [VectorDimension(2)] public float Z { get; set; }

        public float Length {
            get => (float)Math.Sqrt(LengthSquared);
            set {
                var x = X / Length * value;
                var y = Y / Length * value;
                var z = Z / Length * value;

                X = x; Y = y; Z = z;
            }
        }
        public float LengthSquared {
            get => X * X + Y * Y + Z * Z;
            set => Length = (float)Math.Sqrt(value);
        }

        public Point3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public Point3(float val) : this(val, val, val) { }

        public float Dot(IVector<float> vector)
        {
            if (!(vector is Point3)) throw new Exception("Vector isn't instance of Point3");
            var vec = (Point3)vector;

            return X * vec.X + Y * vec.Y + Z * vec.Z;
        }

        public IVector<float> Add(IVector<float> obj)
        {
            if (!(obj is Point3)) throw new Exception("Vector isn't instance of Point3");
            var vec = (Point3)obj;

            return new Point3(X + vec.X, Y + vec.Y, Z + vec.Z);
        }
        public IVector<float> Subtract(IVector<float> obj)
        {
            if (!(obj is Point3)) throw new Exception("Vector isn't instance of Point3");
            var vec = (Point3)obj;

            return new Point3(X - vec.X, Y - vec.Y, Z - vec.Z);
        }
        public IVector<float> Multiply(float obj)
        {
            return new Point3(X * obj, Y * obj, Z * obj);
        }
        public IVector<float> Divide(float obj)
        {
            return new Point3(X / obj, Y / obj, Z / obj);
        }

        public float[] Flattern() => new[] { X, Y, Z };
    }
}
