using NetGL.GraphicsAPI;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace NetGL
{
    [Matrix(2, 2, MultiDimensionType.Float)]
    public struct Matrix2: IMatrix<float>
    {
        public static Matrix2 Identity { get; } = new Matrix2(1, 0, 0, 1);

        public float X1 { get; set; }
        public float X2 { get; set; }
        public float Y1 { get; set; }
        public float Y2 { get; set; }

        public float[] Flattern()
        {
            return new float[] {
                X1, X2,
                Y1, Y2,
            };
        }

        public Matrix2(float x1, float x2, float y1, float y2)
        {
            X1 = x1;
            X2 = x2;
            Y1 = y1;
            Y2 = y2;
        }

        public float Determinant => X1 * Y2 - X2 * Y1;
        public IMatrix<float> Inverse()
        {
            return Divide(Determinant);
        }

        public IMatrix<float> Add(IMatrix<float> obj)
        {
            if (!(obj is Matrix2)) throw new Exception("obj is not of type Matrix2");

            var m = (Matrix2)obj;

            return new Matrix2(X1 * m.X1, X2 * m.X2, Y1 * m.Y1, Y2 * m.Y2);
        }
        public IMatrix<float> Subtract(IMatrix<float> obj)
        {
            if (!(obj is Matrix2)) throw new Exception("obj is not of type Matrix2");

            var m = (Matrix2)obj;

            return new Matrix2(X1 - m.X1, X2 - m.X2, Y1 - m.Y1, Y2 - m.Y2);
        }
        public IMatrix<float> Multiply(IMatrix<float> obj)
        {
            if (!(obj is Matrix2)) throw new Exception("obj is not of type Matrix2");

            var m = (Matrix2)obj;

            var a = X1 * m.X2;
            var b = Y1 * m.Y2;

            var c = X1 * m.Y1;
            var d = X2 * m.Y2;

            var x1 = a + c;
            var x2 = a + d;
            var y1 = b + c;
            var y2 = b + d;

            return new Matrix2(x1, x2, y1, y2);
        }
        public IVector<float> Multiply(IVector<float> obj)
        {
            if (!(obj is Vector2)) throw new Exception("Vector is not of type Vector2");

            var p = (Vector2)obj;

            var x = X1 * p.X + X2 * p.X;
            var y = Y1 * p.Y + Y2 * p.Y;

            return new Vector2(x, y);
        }
        public IMatrix<float> Multiply(float obj)
        {
            return new Matrix2(X1 * obj, X2 * obj, Y1 * obj, Y2 * obj);
        }
        public IMatrix<float> Divide(float obj)
        {
            return Multiply(1 / obj);
        }

        /* Not implemented yet
        public static implicit operator VectorI3(Vector3 pt)
        {
            return new VectorI3((int)pt.X, (int)pt.Y, (int)pt.Z);
        }
        */

        public static Matrix2 operator +(Matrix2 a, Matrix2 b) => (Matrix2)a.Add(b);
        public static Matrix2 operator -(Matrix2 a, Matrix2 b) => (Matrix2)a.Subtract(b);
        public static Matrix2 operator *(Matrix2 a, Matrix2 b) => (Matrix2)a.Multiply(b);
        public static Matrix2 operator *(Matrix2 b, float a) => (Matrix2)b.Multiply(a);
        public static Vector2 operator *(Matrix2 a, Vector2 b) => (Vector2)a.Multiply(b);
        public static Matrix2 operator /(Matrix2 a, float b) => (Matrix2)a.Divide(b);

        public static Matrix2 CreateScaleX(float x)
        {
            return new Matrix2(x, 0, 0, 1);
        }
        public static Matrix2 CreateScaleY(float y)
        {
            return new Matrix2(1, 0, 0, y);
        }
        public static Matrix2 CreateScale(float x, float y)
        {
            return new Matrix2(x, 0, 0, y);
        }

        public static Matrix2 CreateRotation(float deg)
        {
            var cos = (float)Math.Cos(deg / 180 * Math.PI);
            var sin = (float)Math.Sin(deg / 180 * Math.PI);

            return new Matrix2(cos, -sin, sin, cos);
        }
    }
}
