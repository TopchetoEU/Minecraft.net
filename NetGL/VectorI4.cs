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

        public int this[int component] {
            get {
                switch (component) {
                    case 0:
                        return X;
                    case 1:
                        return Y;
                    case 2:
                        return Z;
                    case 3:
                        return W;
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
                    case 3:
                        W = value;
                        break;
                    default:
                        throw new Exception("Invalid component index");
                }
            }
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

        public VectorI4 this[char a, char b, char c, char d] {
            get {
                var x = getCharComponent(a);
                var y = getCharComponent(b);
                var z = getCharComponent(c);
                var w = getCharComponent(c);

                return new VectorI4(this[x], this[y], this[z], this[w]);
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
        public VectorI3 this[char a, char b, char c] {
            get {
                var x = getCharComponent(a);
                var y = getCharComponent(b);
                var z = getCharComponent(c);

                return new VectorI3(this[x], this[y], this[z]);
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
        public VectorI2 this[char a, char b] {
            get {
                var x = getCharComponent(a);
                var y = getCharComponent(b);

                return new VectorI2(this[x], this[y]);
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
        public int this[char component] {
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
