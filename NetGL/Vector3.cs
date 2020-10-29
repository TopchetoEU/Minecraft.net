using NetGL.GraphicsAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetGL
{
    [Vector(3, MultiDimensionType.Float)]
    public struct Vector3: IVector<float>
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

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public Vector3(Vector2 point, float z)
        {
            X = point.X;
            Y = point.Y;
            Z = z;
        }
        public Vector3(float val) : this(val, val, val) { }

        public float Dot(IVector<float> vector)
        {
            if (!(vector is Vector3)) throw new Exception("Vector isn't instance of Point3");
            var vec = (Vector3)vector;

            return X * vec.X + Y * vec.Y + Z * vec.Z;
        }

        public IVector<float> Add(IVector<float> obj)
        {
            if (!(obj is Vector3)) throw new Exception("Vector isn't instance of Point3");
            var vec = (Vector3)obj;

            return new Vector3(X + vec.X, Y + vec.Y, Z + vec.Z);
        }
        public IVector<float> Subtract(IVector<float> obj)
        {
            if (!(obj is Vector3)) throw new Exception("Vector isn't instance of Point3");
            var vec = (Vector3)obj;

            return new Vector3(X - vec.X, Y - vec.Y, Z - vec.Z);
        }
        public IVector<float> Multiply(float obj)
        {
            return new Vector3(X * obj, Y * obj, Z * obj);
        }
        public IVector<float> Divide(float obj)
        {
            return new Vector3(X / obj, Y / obj, Z / obj);
        }

        public float[] Flattern() => new[] { X, Y, Z };


        public static implicit operator VectorI3(Vector3 pt)
        {
            return new VectorI3((int)pt.X, (int)pt.Y, (int)pt.Z);
        }

        public static Vector3 operator +(Vector3 a, Vector3 b) => (Vector3)a.Add(b);
        public static Vector3 operator -(Vector3 a, Vector3 b) => (Vector3)a.Subtract(b);
        public static Vector3 operator /(Vector3 a, float b) => (Vector3)a.Divide(b);
        public static Vector3 operator *(Vector3 a, float b) => (Vector3)a.Multiply(b);
    }
}
