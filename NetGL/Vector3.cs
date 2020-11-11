using NetGL.GraphicsAPI;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace NetGL
{
    [Vector(3, MultiDimensionType.Float)]
    public struct Vector3: IVector<float>
    {
        public static Vector3 Zero { get; } = new Vector3(0, 0, 0);
        [VectorDimension(0)] public float X { get; set; }
        [VectorDimension(1)] public float Y { get; set; }
        [VectorDimension(2)] public float Z { get; set; }

        public void SetX(float val)
        {
            X = val;
        }
        public void SetY(float val)
        {
            Y = val;
        }
        public void SetZ(float val)
        {
            Z = val;
        }

        internal static int getCharComponent(char @char)
        {
            switch (@char.ToString().ToLower()[0]) {
                case 'r':
                case 'x':
                case 's':
                    return 0;
                case 'g':
                case 'y':
                case 't':
                    return 1;
                case 'b':
                case 'z':
                case 'p':
                    return 2;
                case 'a':
                case 'w':
                case 'q':
                    return 3;
                default:
                    return -1;
            }
        }

        public float this[int component] {
            get {
                switch (component) {
                    case 0:
                        return X;
                    case 1:
                        return Y;
                    case 2:
                        return Z;
                    default:
                        throw new Exception("Invalid component index");
                }
            }
            set {
                switch (component) {
                    case 0:
                        X = value;
                        break;
                    case 1:
                        Y = value;
                        break;
                    case 2:
                        Z = value;
                        break;
                    default:
                        throw new Exception("Invalid component index");
                }
            }
        }

        public Vector3 this[char a, char b, char c] {
            get {
                var x = getCharComponent(a);
                var y = getCharComponent(b);
                var z = getCharComponent(c);

                return new Vector3(this[x], this[y], this[z]);
            }
            set {
                var x = getCharComponent(a);
                var y = getCharComponent(b);
                var z = getCharComponent(c);

                if (x == y || y == z || x == z)
                    throw new Exception("Can't assign vector's diffrent components to the same component");

                this[x] = value.X;
                this[y] = value.Y;
                this[z] = value.Z;
            }
        }
        public Vector2 this[char a, char b] {
            get {
                var x = getCharComponent(a);
                var y = getCharComponent(b);

                return new Vector2(this[x], this[y]);
            }
            set {
                var x = getCharComponent(a);
                var y = getCharComponent(b);

                if (x == y)
                    throw new Exception("Can't assign vector's diffrent components to the same component");

                this[x] = value.X;
                this[y] = value.Y;
            }
        }
        public float this[char component] {
            get {
                var x = getCharComponent(component);

                return this[x];
            }
            set {
                var x = getCharComponent(component);

                this[x] = value;
            }
        }

        public float Length {
            get => (float)Math.Sqrt(LengthSquared);
            set {
                var x = X / Length * value;
                var y = Y / Length * value;
                var z = Z / Length * value;

                X = x;
                Y = y;
                Z = z;
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
            if (!(vector is Vector3))
                throw new Exception("Vector isn't instance of Point3");
            var vec = (Vector3)vector;

            return X * vec.X + Y * vec.Y + Z * vec.Z;
        }

        public IVector<float> Add(IVector<float> obj)
        {
            if (!(obj is Vector3))
                throw new Exception("Vector isn't instance of Point3");
            var vec = (Vector3)obj;

            return new Vector3(X + vec.X, Y + vec.Y, Z + vec.Z);
        }
        public IVector<float> Subtract(IVector<float> obj)
        {
            if (!(obj is Vector3))
                throw new Exception("Vector isn't instance of Point3");
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
        public static Vector3 operator -(Vector3 a) => Zero - a;
    }
}
