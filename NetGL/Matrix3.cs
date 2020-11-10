using NetGL.GraphicsAPI;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace NetGL
{
    [Matrix(3, 3, MultiDimensionType.Float)]
    public struct Matrix3: IMatrix<float>
    {
        public static Matrix3 Identity { get; } = new Matrix3(1, 0, 0, 0, 1, 0, 0, 0, 1);

        public float X1 { get; set; }
        public float X2 { get; set; }
        public float X3 { get; set; }

        public float Y1 { get; set; }
        public float Y2 { get; set; }
        public float Y3 { get; set; }

        public float Z1 { get; set; }
        public float Z2 { get; set; }
        public float Z3 { get; set; }

        public float[] Flattern()
        {
            return new float[] {
                X1, X2, X3,
                Y1, Y2, Y3,
                Z1, Z2, Z3,
            };
        }
        public Matrix3(float x1, float x2, float x3, float y1, float y2, float y3, float z1, float z2, float z3)
        {
            X1 = x1;
            X2 = x2;
            X3 = x3;
            Y1 = y1;
            Y2 = y2;
            Y3 = y3;
            Z1 = z1;
            Z2 = z2;
            Z3 = z3;
        }
        public Matrix3(Matrix2 mt) : this(mt.X1, mt.X2, 0, mt.Y1, mt.Y2, 0, 0, 0, 1) { }

        public float Determinant {
            get {
                var a = Y2 * Z3 - Y3 * Z2;
                var b = Y1 * Z3 - Y3 * Z1;
                var c = Y1 * Z2 - Y2 * Z1;

                return a - b + c;
            }
        }

        public IMatrix<float> Add(IMatrix<float> obj)
        {
            if (!(obj is Matrix3)) throw new Exception("obj is not of type Matrix3");

            var m = (Matrix3)obj;

            return new Matrix3(
                X1 + m.X1, X2 + m.X2, X3 + m.X3,
                Y1 + m.Y1, Y2 + m.Y2, Y3 + m.Y3,
                Z1 + m.Z1, Z2 + m.Z2, Z3 + m.Z3
            );
        }
        public IMatrix<float> Subtract(IMatrix<float> obj)
        {
            if (!(obj is Matrix3)) throw new Exception("obj is not of type Matrix3");

            var m = (Matrix3)obj;

            return new Matrix3(
                X1 - m.X1, X2 - m.X2, X3 - m.X3,
                Y1 - m.Y1, Y2 - m.Y2, Y3 - m.Y3,
                Z1 - m.Z1, Z2 - m.Z2, Z3 - m.Z3
            );
        }

        public IMatrix<float> Inverse()
        {
            return Divide(Determinant);
        }

        public IMatrix<float> Multiply(IMatrix<float> obj)
        {
            if (!(obj is Matrix3)) throw new Exception("obj is not of type Matrix3");

            var m = (Matrix3)obj;

            var a = m;
            var b = this;


            var x1 = a.X1 * b.X1 + a.X2 * b.Y1 + a.X3 * b.Z1;
            var x2 = a.X1 * b.X2 + a.X2 * b.Y2 + a.X3 * b.Z2;
            var x3 = a.X1 * b.X3 + a.X2 * b.Y3 + a.X3 * b.Z3;

            var y1 = a.Y1 * b.X1 + a.Y2 * b.Y1 + a.Y3 * b.Z1;
            var y2 = a.Y1 * b.X2 + a.Y2 * b.Y2 + a.Y3 * b.Z2;
            var y3 = a.Y1 * b.X3 + a.Y2 * b.Y3 + a.Y3 * b.Z3;

            var z1 = a.Z1 * b.X1 + a.Z2 * b.Y1 + a.Z3 * b.Z1;
            var z2 = a.Z1 * b.X2 + a.Z2 * b.Y2 + a.Z3 * b.Z2;
            var z3 = a.Z1 * b.X3 + a.Z2 * b.Y3 + a.Z3 * b.Z3;

            return new Matrix3(x1, x2, x3, y1, y2, y3, z1, z2, z3);
        }
        public IVector<float> Multiply(IVector<float> obj)
        {
            if (!(obj is Vector3)) throw new Exception("Vector is not of type Vectpr3");

            var p = (Vector3)obj;

            var x = X1 * p.X + X2 * p.X + X3 * p.X;
            var y = Y1 * p.Y + Y2 * p.Y + Y3 * p.Y;
            var z = Z1 * p.Z + Z2 * p.Z + Z3 * p.Z;

            return new Vector3(x, y, z);
        }
        public IMatrix<float> Multiply(float obj)
        {
            return new Matrix3(
                X1 * obj, X2 * obj, X3 * obj,
                Y1 * obj, Y2 * obj, Y3 * obj,
                Z1 * obj, Z2 * obj, Z3 * obj);
        }
        public IMatrix<float> Divide(float obj)
        {
            return Multiply(1 / obj);
        }

        public static Matrix3 operator +(Matrix3 a, Matrix3 b) => (Matrix3)a.Add(b);
        public static Matrix3 operator -(Matrix3 a, Matrix3 b) => (Matrix3)a.Subtract(b);
        public static Matrix3 operator *(Matrix3 a, Matrix3 b) => (Matrix3)a.Multiply(b);
        public static Matrix3 operator *(Matrix3 b, float a) => (Matrix3)b.Multiply(a);
        public static Vector3 operator *(Matrix3 a, Vector3 b) => (Vector3)a.Multiply(b);
        public static Matrix3 operator /(Matrix3 a, float b) => (Matrix3)a.Divide(b);

        public static Matrix3 CreateTranslationX(float x)
        {
            return new Matrix3(1, 0, x, 0, 1, 0, 0, 0, 1);
        }
        public static Matrix3 CreateTranslationY(float y)
        {
            return new Matrix3(1, 0, 0, 0, 1, y, 0, 0, 1);
        }
        public static Matrix3 CreateTranslation(float x, float y)
        {
            return new Matrix3(1, 0, x, 0, 1, y, 0, 0, 1);
        }

        public static Matrix3 CreateScaleX(float x)
        {
            return new Matrix3(new Matrix2(x, 0, 0, 1));
        }
        public static Matrix3 CreateScaleY(float y)
        {
            return new Matrix3(new Matrix2(1, 0, 0, y));
        }
        public static Matrix3 CreateScale(float x, float y)
        {
            return new Matrix3(x, 0, 0, 0, y, 0, 0, 0, 1);
        }
        public static Matrix3 CreateScale(float scale)
        {
            return CreateScale(scale, scale);
        }
    
        public static Matrix3 CreateRotation(float deg)
        {
            var cos = (float)Math.Cos(deg / 180 * Math.PI);
            var sin = (float)Math.Sin(deg / 180 * Math.PI);

            return new Matrix3(new Matrix2(cos, -sin, sin, cos));
        }
    }
}
