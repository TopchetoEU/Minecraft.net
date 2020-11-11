using NetGL.GraphicsAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetGL
{
    [Vector(2, MultiDimensionType.Float)]
    public struct VectorD2: IVector<double>
    {
        [VectorDimension(0)] public double X { get; set; }
        [VectorDimension(1)] public double Y { get; set; }

        public double this[int component] {
            get {
                switch (component) {
                    case 0:
                        return X;
                    case 1:
                        return Y;
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

        public VectorD2 this[char a, char b] {
            get {
                var x = getCharComponent(a);
                var y = getCharComponent(b);

                return new VectorD2(this[x], this[y]);
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
        public double this[char component] {
            get {
                var x = getCharComponent(component);

                return this[x];
            }
            set {
                var x = getCharComponent(component);

                this[x] = value;
            }
        }

        public VectorD2 XX => new VectorD2(X, X);
        public VectorD2 YY => new VectorD2(Y, Y);
        public VectorD2 YX => new VectorD2(Y, X);

        public float Length {
            get => (float)Math.Sqrt(LengthSquared);
            set {
                var x = X / Length * value;
                var y = Y / Length * value;

                X = x; Y = y;
            }
        }
        public float LengthSquared {
            get => (float)(X * X + Y * Y);
            set => Length = (float)Math.Sqrt(value);
        }

        public VectorD2(double x, double y)
        {
            X = x;
            Y = y;
        }
        public VectorD2(double val) : this(val, val) { }

        public double Dot(IVector<double> vector)
        {
            if (!(vector is VectorD2)) throw new Exception("Vector isn't instance of VectorD2");
            var vec = (VectorD2)vector;

            return X * vec.X + Y * vec.Y;
        }
        public double Dot(IVector<float> vector)
        {
            if (!(vector is Vector2)) throw new Exception("Vector isn't instance of Vector2");
            var vec = (Vector2)vector;

            return X * vec.X + Y * vec.Y;
        }
        public double Dot(IVector<int> vector)
        {
            if (!(vector is VectorI2)) throw new Exception("Vector isn't instance of VectorI2");
            var vec = (VectorI2)vector;

            return X * vec.X + Y * vec.Y;
        }

        public IVector<double> Add(IVector<double> obj)
        {
            if (!(obj is VectorD2)) throw new Exception("Vector isn't instance of VectorD2");
            var vec = (VectorD2)obj;

            return new VectorD2(X + vec.X, Y + vec.Y);
        }
        public IVector<double> Subtract(IVector<double> obj)
        {
            if (!(obj is VectorD2)) throw new Exception("Vector isn't instance of VectorD2");
            var vec = (VectorD2)obj;

            return new VectorD2(X - vec.X, Y - vec.Y);
        }
        public IVector<double> Multiply(float obj)
        {
            return new VectorD2(X * obj, Y * obj);
        }
        public IVector<double> Divide(float obj)
        {
            return new VectorD2(X / obj, Y / obj);
        }

        public double[] Flattern() => new[] { X, Y };

        public static implicit operator VectorI2(VectorD2 pt)
        {
            return new VectorI2((int)pt.X, (int)pt.Y);
        }
        public static implicit operator Vector2(VectorD2 pt)
        {
            return new Vector2((float)pt.X, (float)pt.Y);
        }

        public static VectorD2 operator +(VectorD2 a, VectorD2 b) => (VectorD2)a.Add(b);
        public static VectorD2 operator -(VectorD2 a, VectorD2 b) => (VectorD2)a.Subtract(b);
        public static VectorD2 operator /(VectorD2 a, float b) => (VectorD2)a.Divide(b);
        public static VectorD2 operator *(VectorD2 a, float b) => (VectorD2)a.Multiply(b);
    }
}
