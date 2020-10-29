﻿using NetGL.GraphicsAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetGL
{
    [Vector(4, MultiDimensionType.Double)]
    public struct VectorD4: IVector<double>
    {
        [VectorDimension(0)] public double X { get; set; }
        [VectorDimension(1)] public double Y { get; set; }
        [VectorDimension(2)] public double Z { get; set; }
        [VectorDimension(3)] public double W { get; set; }

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
            get => (float)Dot(this);
            set => Length = (float)Math.Sqrt(value);
        }

        public VectorD4(double x, double y, double z, double w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
        public VectorD4(VectorD2 pt, double z, double w) : this(pt.X, pt.Y, z, w) { }
        public VectorD4(VectorD3 pt, double w) : this(pt.X, pt.Y, pt.Z, w) { }
        public VectorD4(double val) : this(val, val, val, val) { }

        public double Dot(IVector<double> vector)
        {
            if (!(vector is VectorD4)) throw new Exception("Vector isn't instance of VectorD4");
            var vec = (VectorD4)vector;

            return X * vec.X + Y * vec.Y + Z * vec.Z;
        }
        public double Dot(IVector<float> vector)
        {
            if (!(vector is Vector4)) throw new Exception("Vector isn't instance of Vector4");
            var vec = (Vector4)vector;

            return X * vec.X + Y * vec.Y + Z * vec.Z;
        }
        public double Dot(IVector<int> vector)
        {
            if (!(vector is VectorI4)) throw new Exception("Vector isn't instance of VectorI4");
            var vec = (VectorI4)vector;

            return X * vec.X + Y * vec.Y + Z * vec.Z;
        }

        public IVector<double> Add(IVector<double> obj)
        {
            if (!(obj is VectorD4)) throw new Exception("Vector isn't instance of VectorD4");
            var vec = (VectorD4)obj;

            return new VectorD4(X + vec.X, Y + vec.Y, Z + vec.Z, W + vec.W);
        }
        public IVector<double> Subtract(IVector<double> obj)
        {
            if (!(obj is VectorD4)) throw new Exception("Vector isn't instance of VectorD4");
            var vec = (VectorD4)obj;

            return new VectorD4(X - vec.X, Y - vec.Y, Z - vec.Z, W - vec.W);
        }
        public IVector<double> Multiply(float obj)
        {
            return new VectorD4(X * obj, Y * obj, Z * obj, W * obj);
        }
        public IVector<double> Divide(float obj)
        {
            return new VectorD4(X / obj, Y / obj, Z / obj, W / obj);
        }

        public double[] Flattern() => new[] { X, Y, Z, W };

        public static implicit operator VectorI4(VectorD4 pt)
        {
            return new VectorI4((int)pt.X, (int)pt.Y, (int)pt.Z, (int)pt.W);
        }
        public static implicit operator Vector4(VectorD4 pt)
        {
            return new Vector4((float)pt.X, (float)pt.Y, (float)pt.Z, (float)pt.W);
        }

        public static VectorD4 operator +(VectorD4 a, VectorD4 b) => (VectorD4)a.Add(b);
        public static VectorD4 operator -(VectorD4 a, VectorD4 b) => (VectorD4)a.Subtract(b);
        public static VectorD4 operator /(VectorD4 a, float b) => (VectorD4)a.Divide(b);
        public static VectorD4 operator *(VectorD4 a, float b) => (VectorD4)a.Multiply(b);
    }
}
