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

        public int this[int component] {
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
