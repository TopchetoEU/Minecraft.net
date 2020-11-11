using NetGL.GraphicsAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetGL
{
    [Vector(4, MultiDimensionType.Float)]
    public struct Vector4: IVector<float>
    {
        [VectorDimension(0)] public float X { get; set; }
        [VectorDimension(1)] public float Y { get; set; }
        [VectorDimension(2)] public float Z { get; set; }
        [VectorDimension(3)] public float W { get; set; }

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

        public Vector4 this[char a, char b, char c, char d] {
            get {
                var x = getCharComponent(a);
                var y = getCharComponent(b);
                var z = getCharComponent(c);
                var w = getCharComponent(c);

                return new Vector4(this[x], this[y], this[z], this[w]);
            }
            set {
                var x = getCharComponent(a);
                var y = getCharComponent(b);
                var z = getCharComponent(c);
                var w = getCharComponent(c);

                if (x == y || x == z || x == w || y == z || y == w || z == w)
                    throw new Exception("Can't assign vector's diffrent components to the same component");

                this[x] = value.X;
                this[y] = value.Y;
                this[z] = value.Z;
                this[w] = value.W;
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
                var w = W / Length * value;

                X = x; Y = y; Z = z; W = w;
            }
        }
        public float LengthSquared {
            get => X * X + Y * Y + Z * Z + W * W;
            set => Length = (float)Math.Sqrt(value);
        }

        public Vector4(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
        public Vector4(Vector2 pt, float z, float w) : this(pt.X, pt.Y, z, w) { }
        public Vector4(Vector3 pt, float w) : this(pt.X, pt.Y, pt.Z, w) { }
        public Vector4(float val) : this(val, val, val, val) { }

        public float Dot(IVector<float> vector)
        {
            if (!(vector is Vector4)) throw new Exception("Vector isn't instance of Point4");
            var vec = (Vector4)vector;

            return X * vec.X + Y * vec.Y + Z * vec.Z;
        }

        public IVector<float> Add(IVector<float> obj)
        {
            if (!(obj is Vector4)) throw new Exception("Vector isn't instance of Point4");
            var vec = (Vector4)obj;

            return new Vector4(X + vec.X, Y + vec.Y, Z + vec.Z, W + vec.W);
        }
        public IVector<float> Subtract(IVector<float> obj)
        {
            if (!(obj is Vector4)) throw new Exception("Vector isn't instance of Point4");
            var vec = (Vector4)obj;

            return new Vector4(X - vec.X, Y - vec.Y, Z - vec.Z, W - vec.W);
        }
        public IVector<float> Multiply(float obj)
        {
            return new Vector4(X * obj, Y * obj, Z * obj, W * obj);
        }
        public IVector<float> Divide(float obj)
        {
            return new Vector4(X / obj, Y / obj, Z / obj, W / obj);
        }

        public float[] Flattern() => new[] { X, Y, Z, W };

        public static implicit operator VectorI4(Vector4 pt)
        {
            return new VectorI4((int)pt.X, (int)pt.Y, (int)pt.Z, (int)pt.W);
        }

        public static Vector4 operator +(Vector4 a, Vector4 b) => (Vector4)a.Add(b);
        public static Vector4 operator -(Vector4 a, Vector4 b) => (Vector4)a.Subtract(b);
        public static Vector4 operator /(Vector4 a, float b) => (Vector4)a.Divide(b);
        public static Vector4 operator *(Vector4 a, float b) => (Vector4)a.Multiply(b);
    }
}
