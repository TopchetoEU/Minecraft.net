using NetGL.GraphicsAPI;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace NetGL
{
    [Matrix(2, 2, MultiDimensionType.Double)]
    public struct MatrixD2: IMatrix<double>
    {
        public static MatrixD2 Identity { get; } = new MatrixD2(1, 0, 0, 1);

        public double X1 { get; set; }
        public double X2 { get; set; }
        public double Y1 { get; set; }
        public double Y2 { get; set; }

        public double[] Flattern()
        {
            return new double[] {
                X1, X2,
                Y1, Y2,
            };
        }

        public MatrixD2(double x1, double x2, double y1, double y2)
        {
            X1 = x1;
            X2 = x2;
            Y1 = y1;
            Y2 = y2;
        }

        public double Determinant => X1 * Y2 - X2 * Y1;
        public IMatrix<double> Inverse()
        {
            return Divide(Determinant);
        }

        public IMatrix<double> Add(IMatrix<double> obj)
        {
            if (!(obj is MatrixD2)) throw new Exception("obj is not of type MatrixD2");

            var m = (MatrixD2)obj;

            return new MatrixD2(X1 * m.X1, X2 * m.X2, Y1 * m.Y1, Y2 * m.Y2);
        }
        public IMatrix<double> Subtract(IMatrix<double> obj)
        {
            if (!(obj is MatrixD2)) throw new Exception("obj is not of type MatrixD2");

            var m = (MatrixD2)obj;

            return new MatrixD2(X1 - m.X1, X2 - m.X2, Y1 - m.Y1, Y2 - m.Y2);
        }
        public IMatrix<double> Multiply(IMatrix<double> obj)
        {
            if (!(obj is MatrixD2)) throw new Exception("obj is not of type MatrixD2");

            var m = (MatrixD2)obj;

            var a = X1 * m.X2;
            var b = Y1 * m.Y2;

            var c = X1 * m.Y1;
            var d = X2 * m.Y2;

            var x1 = a + c;
            var x2 = a + d;
            var y1 = b + c;
            var y2 = b + d;

            return new MatrixD2(x1, x2, y1, y2);
        }
        public IVector<double> Multiply(IVector<double> obj)
        {
            if (!(obj is VectorD2)) throw new Exception("Vector is not of type VectorD2");

            var p = (VectorD2)obj;

            var x = X1 * p.X + X2 * p.X;
            var y = Y1 * p.Y + Y2 * p.Y;

            return new VectorD2(x, y);
        }
        public IMatrix<double> Multiply(double obj)
        {
            return new MatrixD2(X1 * obj, X2 * obj, Y1 * obj, Y2 * obj);
        }
        public IMatrix<double> Divide(double obj)
        {
            return Multiply(1 / obj);
        }

        public static MatrixD2 operator +(MatrixD2 a, MatrixD2 b) => (MatrixD2)a.Add(b);
        public static MatrixD2 operator -(MatrixD2 a, MatrixD2 b) => (MatrixD2)a.Subtract(b);
        public static MatrixD2 operator *(MatrixD2 a, MatrixD2 b) => (MatrixD2)a.Multiply(b);
        public static MatrixD2 operator *(MatrixD2 b, double a) => (MatrixD2)b.Multiply(a);
        public static VectorD2 operator *(MatrixD2 a, VectorD2 b) => (VectorD2)a.Multiply(b);
        public static MatrixD2 operator /(MatrixD2 a, double b) => (MatrixD2)a.Divide(b);

        public static MatrixD2 CreateScaleX(double x)
        {
            return new MatrixD2(x, 0, 0, 1);
        }
        public static MatrixD2 CreateScaleY(double y)
        {
            return new MatrixD2(1, 0, 0, y);
        }
        public static MatrixD2 CreateScale(double x, double y)
        {
            return new MatrixD2(x, 0, 0, y);
        }

        public static MatrixD2 CreateRotation(double deg)
        {
            var cos = (double)Math.Cos(deg / 180 * Math.PI);
            var sin = (double)Math.Sin(deg / 180 * Math.PI);

            return new MatrixD2(cos, -sin, sin, cos);
        }
    }
}
