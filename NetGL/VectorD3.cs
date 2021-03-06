﻿using NetGL.GraphicsAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetGL
{
    [Vector(3, MultiDimensionType.Double)]
    public struct VectorD3: IVector<double>
    {
        [VectorDimension(0)] public double X { get; set; }
        [VectorDimension(1)] public double Y { get; set; }
        [VectorDimension(2)] public double Z { get; set; }

        public double this[int component] {
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

        public VectorD3 this[char a, char b, char c] {
            get {
                var x = getCharComponent(a);
                var y = getCharComponent(b);
                var z = getCharComponent(c);

                return new VectorD3(this[x], this[y], this[z]);
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
            get => (float)Dot(this);
            set => Length = (float)Math.Sqrt(value);
        }

        public VectorD3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public VectorD3(VectorD2 point, double z)
        {
            X = point.X;
            Y = point.Y;
            Z = z;
        }
        public VectorD3(double val) : this(val, val, val) { }

        public double Dot(IVector<double> vector)
        {
            if (!(vector is VectorD3)) throw new Exception("Vector isn't instance of VectorD3");
            var vec = (VectorD3)vector;

            return X * vec.X + Y * vec.Y + Z * vec.Z;
        }
        public double Dot(IVector<float> vector)
        {
            if (!(vector is Vector3)) throw new Exception("Vector isn't instance of Vector3");
            var vec = (Vector3)vector;

            return X * vec.X + Y * vec.Y + Z * vec.Z;
        }
        public double Dot(IVector<int> vector)
        {
            if (!(vector is VectorI3)) throw new Exception("Vector isn't instance of VectorI3");
            var vec = (VectorI3)vector;

            return X * vec.X + Y * vec.Y + Z * vec.Z;
        }

        public IVector<double> Add(IVector<double> obj)
        {
            if (!(obj is VectorD3)) throw new Exception("Vector isn't instance of VectorD3");
            var vec = (VectorD3)obj;

            return new VectorD3(X + vec.X, Y + vec.Y, Z + vec.Z);
        }
        public IVector<double> Subtract(IVector<double> obj)
        {
            if (!(obj is VectorD3)) throw new Exception("Vector isn't instance of VectorD3");
            var vec = (VectorD3)obj;

            return new VectorD3(X - vec.X, Y - vec.Y, Z - vec.Z);
        }
        public IVector<double> Multiply(float obj)
        {
            return new VectorD3(X * obj, Y * obj, Z * obj);
        }
        public IVector<double> Divide(float obj)
        {
            return new VectorD3(X / obj, Y / obj, Z / obj);
        }

        public double[] Flattern() => new[] { X, Y, Z };


        public static implicit operator VectorI3(VectorD3 pt)
        {
            return new VectorI3((int)pt.X, (int)pt.Y, (int)pt.Z);
        }
        public static implicit operator Vector3(VectorD3 pt)
        {
            return new Vector3((float)pt.X, (float)pt.Y, (float)pt.Z);
        }

        public static VectorD3 operator +(VectorD3 a, VectorD3 b) => (VectorD3)a.Add(b);
        public static VectorD3 operator -(VectorD3 a, VectorD3 b) => (VectorD3)a.Subtract(b);
        public static VectorD3 operator /(VectorD3 a, float b) => (VectorD3)a.Divide(b);
        public static VectorD3 operator *(VectorD3 a, float b) => (VectorD3)a.Multiply(b);
    }
}
