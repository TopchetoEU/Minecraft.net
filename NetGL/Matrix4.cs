using NetGL.GraphicsAPI;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace NetGL
{
    [Matrix(4, 4, MultiDimensionType.Float)]
    public struct Matrix4: IMatrix<float>
    {
        public static Matrix4 Identity { get; } = new Matrix4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);

        public float X1 { get; set; }
        public float X2 { get; set; }
        public float X3 { get; set; }
        public float X4 { get; set; }

        public float Y1 { get; set; }
        public float Y2 { get; set; }
        public float Y3 { get; set; }
        public float Y4 { get; set; }

        public float Z1 { get; set; }
        public float Z2 { get; set; }
        public float Z3 { get; set; }
        public float Z4 { get; set; }

        public float W1 { get; set; }
        public float W2 { get; set; }
        public float W3 { get; set; }
        public float W4 { get; set; }

        public float[] Flattern()
        {
            return new float[] {
                X1, X2, X3, X4,
                Y1, Y2, Y3, Y4,
                Z1, Z2, Z3, Z4,
                W1, W2, W3, W4
            };
        }

        public Matrix4(
            float x1, float x2, float x3, float x4,
            float y1, float y2, float y3, float y4,
            float z1, float z2, float z3, float z4,
            float w1, float w2, float w3, float w4)
        {
            X1 = x1;
            X2 = x2;
            X3 = x3;
            X4 = x4;

            Y1 = y1;
            Y2 = y2;
            Y3 = y3;
            Y4 = y4;

            Z1 = z1;
            Z2 = z2;
            Z3 = z3;
            Z4 = z4;

            W1 = w1;
            W2 = w2;
            W3 = w3;
            W4 = w4;
        }
        public Matrix4(Matrix2 mt) : this(
            mt.X1, mt.X2, 0, 0,
            mt.Y1, mt.Y2, 0, 0,
            0, 0, 1, 0,
            0, 0, 0, 1)
        { }
        public Matrix4(Matrix3 mt) : this(
            mt.X1, mt.X2, mt.X3, 0,
            mt.Y1, mt.Y2, mt.Y3, 0,
            mt.Z1, mt.Z2, mt.Z3, 0,
            0, 0, 0, 1)
        { }

        public float Determinant {
            get {
                var a = new Matrix3(Y2, Z2, W2, Y3, Z3, W3, Y4, Z4, W4).Determinant;
                var b = new Matrix3(Y1, Z1, W1, Y3, Z3, W3, Y4, Z4, W4).Determinant;
                var c = new Matrix3(Y1, Z1, W1, Y2, Z2, W2, Y4, Z4, W4).Determinant;
                var d = new Matrix3(Y1, Z1, W1, Y2, Z2, W2, Y3, Z3, W3).Determinant;

                return a - b + c - d;
            }
        }

        public IMatrix<float> Add(IMatrix<float> obj)
        {
            if (!(obj is Matrix4)) throw new Exception("obj is not of type Matrix4");

            var m = (Matrix4)obj;

            return new Matrix4(
                X1 + m.X1, X2 + m.X2, X3 + m.X3, X4 + m.X4,
                Y1 + m.Y1, Y2 + m.Y2, Y3 + m.Y3, Y4 + m.Y4,
                Z1 + m.Z1, Z2 + m.Z2, Z3 + m.Z3, Z4 + m.Z4,
                W1 + m.W1, W2 + m.W2, W3 + m.W3, W4 + m.W4
            );
        }
        public IMatrix<float> Subtract(IMatrix<float> obj)
        {
            if (!(obj is Matrix4)) throw new Exception("obj is not of type Matrix4");

            var m = (Matrix4)obj;

            return new Matrix4(
                X1 - m.X1, X2 - m.X2, X3 - m.X3, X4 - m.X4,
                Y1 - m.Y1, Y2 - m.Y2, Y3 - m.Y3, Y4 - m.Y4,
                Z1 - m.Z1, Z2 - m.Z2, Z3 - m.Z3, Z4 - m.Z4,
                W1 - m.W1, W2 - m.W2, W3 - m.W3, W4 - m.W4
            );
        }

        public IMatrix<float> Inverse()
        {
            return Divide(Determinant);
        }

        public IMatrix<float> Multiply(IMatrix<float> obj)
        {
            if (!(obj is Matrix4)) throw new Exception("obj is not of type Matrix4");

            var m = (Matrix4)obj;

            var x1 = X1 * m.X1 + X2 * m.Y1 + X3 * m.Z1 + X4 * m.W1;
            var x2 = X1 * m.X2 + X2 * m.Y2 + X3 * m.Z2 + X4 * m.W2;
            var x3 = X1 * m.X3 + X2 * m.Y3 + X3 * m.Z3 + X4 * m.W3;
            var x4 = X1 * m.X4 + X2 * m.Y4 + X3 * m.Z4 + X4 * m.W4;

            var y1 = Y1 * m.X1 + Y2 * m.Y1 + Y3 * m.Z1 + Y4 * m.W1;
            var y2 = Y1 * m.X2 + Y2 * m.Y2 + Y3 * m.Z2 + Y4 * m.W2;
            var y3 = Y1 * m.X3 + Y2 * m.Y3 + Y3 * m.Z3 + Y4 * m.W3;
            var y4 = Y1 * m.X4 + Y2 * m.Y4 + Y3 * m.Z4 + Y4 * m.W4;

            var z1 = Z1 * m.X1 + Z2 * m.Y1 + Z3 * m.Z1 + Z4 * m.W1;
            var z2 = Z1 * m.X2 + Z2 * m.Y2 + Z3 * m.Z2 + Z4 * m.W2;
            var z3 = Z1 * m.X3 + Z2 * m.Y3 + Z3 * m.Z3 + Z4 * m.W3;
            var z4 = Z1 * m.X4 + Z2 * m.Y4 + Z3 * m.Z4 + Z4 * m.W4;

            var w1 = W1 * m.X1 + W2 * m.Y1 + W3 * m.Z1 + W4 * m.W1;
            var w2 = W1 * m.X2 + W2 * m.Y2 + W3 * m.Z2 + W4 * m.W2;
            var w3 = W1 * m.X3 + W2 * m.Y3 + W3 * m.Z3 + W4 * m.W3;
            var w4 = W1 * m.X4 + W2 * m.Y4 + W3 * m.Z4 + W4 * m.W4;

            return new Matrix4(
                x1, x2, y3, x4,
                y1, y2, y3, y4,
                z1, z2, z3, z4,
                w1, w2, w3, w4
            );
        }
        public IVector<float> Multiply(IVector<float> obj)
        {
            if (!(obj is Vector4)) throw new Exception("Vector is not of type Vector4");

            var p = (Vector4)obj;

            var x = X1 * p.X + X2 * p.X + X3 * p.X + X4 * p.X;
            var y = Y1 * p.Y + Y2 * p.Y + Y3 * p.Y + Y4 * p.Y;
            var z = Z1 * p.Z + Z2 * p.Z + Z3 * p.Z + Z4 * p.Z;
            var w = W1 * p.W + W2 * p.W + W3 * p.W + W4 * p.W;

            return new Vector4(x, y, z, w);
        }
        public IMatrix<float> Multiply(float obj)
        {
            return new Matrix4(
                X1 * obj, X2 * obj, X3 * obj, X4 * obj,
                Y1 * obj, Y2 * obj, Y3 * obj, Y4 * obj,
                Z1 * obj, Z2 * obj, Z3 * obj, Z4 * obj,
                W1 * obj, W2 * obj, W3 * obj, W4 * obj
            );
        }
        public IMatrix<float> Divide(float obj)
        {
            return Multiply(1 / obj);
        }

        public static Matrix4 operator +(Matrix4 a, Matrix4 b) => (Matrix4)a.Add(b);
        public static Matrix4 operator -(Matrix4 a, Matrix4 b) => (Matrix4)a.Subtract(b);
        public static Matrix4 operator *(Matrix4 a, Matrix4 b) => (Matrix4)a.Multiply(b);
        public static Matrix4 operator *(Matrix4 b, float a) => (Matrix4)b.Multiply(a);
        public static Vector4 operator *(Matrix4 a, Vector4 b) => (Vector4)a.Multiply(b);
        public static Matrix4 operator /(Matrix4 a, float b) => (Matrix4)a.Divide(b);

        public Matrix4 CreateTranslationX(float x)
        {
            return new Matrix4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, x, 0, 0, 1);
        }
        public Matrix4 CreateTranslationY(float y)
        {
            return new Matrix4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, y, 0, 1);
        }
        public Matrix4 CreateTranslationZ(float z)
        {
            return new Matrix4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, z, 1);
        }
        public Matrix4 CreateTranslation(float x, float y, float z)
        {
            return new Matrix4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, x, y, z, 1);
        }

        public Matrix4 CreateScaleX(float x)
        {
            return new Matrix4(new Matrix3(x, 0, 0, 0, 1, 0, 0, 0, 1));
        }
        public Matrix4 CreateScaleY(float y)
        {
            return new Matrix4(new Matrix3(1, 0, 0, 0, y, 0, 0, 0, 1));
        }
        public Matrix4 CreateScaleZ(float z)
        {
            return new Matrix4(new Matrix3(1, 0, 0, 0, 1, 0, 0, 0, z));
        }
        public Matrix4 CreateScale(float x, float y, float z)
        {
            return new Matrix4(new Matrix3(x, 0, 0, 0, y, 0, 0, 0, z));
        }

        public Matrix4 CreateRotationX(float deg)
        {
            var cos = (float)Math.Cos(deg * 180 / Math.PI);
            var sin = (float)Math.Cos(deg * 180 / Math.PI);

            return new Matrix4(new Matrix3(1, 0, 0, 0, cos, -sin, 0, sin, cos));
        }
        public Matrix4 CreateRotationY(float deg)
        {
            var cos = (float)Math.Cos(deg * 180 / Math.PI);
            var sin = (float)Math.Cos(deg * 180 / Math.PI);

            return new Matrix4(new Matrix3(cos, 0, sin, 0, 1, 0, -sin, 0, cos));
        }
        public Matrix4 CreateRotationZ(float deg)
        {
            var cos = (float)Math.Cos(deg * 180 / Math.PI);
            var sin = (float)Math.Cos(deg * 180 / Math.PI);

            return new Matrix4(new Matrix2(cos, -sin, sin, cos));
        }
    }
}
