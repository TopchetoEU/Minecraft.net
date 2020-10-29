using NetGL.GraphicsAPI;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace NetGL
{
    [Matrix(3, 3, MultiDimensionType.Double)]
    public struct MatrixD3: IMatrix<double>
    {
        public static MatrixD3 Identity { get; } = new MatrixD3(1, 0, 0, 0, 1, 0, 0, 0, 1);

        public double X1 { get; set; }
        public double X2 { get; set; }
        public double X3 { get; set; }

        public double Y1 { get; set; }
        public double Y2 { get; set; }
        public double Y3 { get; set; }

        public double Z1 { get; set; }
        public double Z2 { get; set; }
        public double Z3 { get; set; }

        public double[] Flattern()
        {
            return new double[] {
                X1, X2, X3,
                Y1, Y2, Y3,
                Z1, Z2, Z3,
            };
        }
        public MatrixD3(double x1, double x2, double x3, double y1, double y2, double y3, double z1, double z2, double z3)
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
        public MatrixD3(MatrixD2 mt) : this(mt.X1, mt.X2, 0, mt.Y1, mt.Y2, 0, 0, 0, 1) { }

        public double Determinant {
            get {
                var a = Y2 * Z3 - Y3 * Z2;
                var b = Y1 * Z3 - Y3 * Z1;
                var c = Y1 * Z2 - Y2 * Z1;

                return a - b + c;
            }
        }

        public IMatrix<double> Add(IMatrix<double> obj)
        {
            if (!(obj is MatrixD3)) throw new Exception("obj is not of type MatrixD3");

            var m = (MatrixD3)obj;

            return new MatrixD3(
                X1 + m.X1, X2 + m.X2, X3 + m.X3,
                Y1 + m.Y1, Y2 + m.Y2, Y3 + m.Y3,
                Z1 + m.Z1, Z2 + m.Z2, Z3 + m.Z3
            );
        }
        public IMatrix<double> Subtract(IMatrix<double> obj)
        {
            if (!(obj is MatrixD3)) throw new Exception("obj is not of type MatrixD3");

            var m = (MatrixD3)obj;

            return new MatrixD3(
                X1 - m.X1, X2 - m.X2, X3 - m.X3,
                Y1 - m.Y1, Y2 - m.Y2, Y3 - m.Y3,
                Z1 - m.Z1, Z2 - m.Z2, Z3 - m.Z3
            );
        }

        public IMatrix<double> Inverse()
        {
            return Divide(Determinant);
        }

        public IMatrix<double> Multiply(IMatrix<double> obj)
        {
            if (!(obj is MatrixD3)) throw new Exception("obj is not of type MatrixD3");

            var m = (MatrixD3)obj;

            var x1 = X1 * m.X1 + X2 * m.Y1 + X3 * m.Z1;
            var x2 = X1 * m.X2 + X2 * m.Y2 + X3 * m.Z2;
            var x3 = X1 * m.X3 + X2 * m.Y3 + X3 * m.Z3;

            var y1 = Y1 * m.X1 + Y2 * m.Y1 + Y3 * m.Z1;
            var y2 = Y1 * m.X2 + Y2 * m.Y2 + Y3 * m.Z2;
            var y3 = Y1 * m.X3 + Y2 * m.Y3 + Y3 * m.Z3;

            var z1 = Z1 * m.X1 + Z2 * m.Y1 + Z3 * m.Z1;
            var z2 = Z1 * m.X2 + Z2 * m.Y2 + Z3 * m.Z2;
            var z3 = Z1 * m.X3 + Z2 * m.Y3 + Z3 * m.Z3;

            return new MatrixD3(x1, x2, x3, y1, y2, y3, z1, z2, z3);
        }
        public IVector<double> Multiply(IVector<double> obj)
        {
            if (!(obj is VectorD3)) throw new Exception("Vector is not of type VectorD3");

            var p = (VectorD3)obj;

            var x = X1 * p.X + X2 * p.X + X3 * p.X;
            var y = Y1 * p.Y + Y2 * p.Y + Y3 * p.Y;
            var z = Z1 * p.Z + Z2 * p.Z + Z3 * p.Z;

            return new VectorD3(x, y, z);
        }
        public IMatrix<double> Multiply(double obj)
        {
            return new MatrixD3(
                X1 * obj, X2 * obj, X3 * obj,
                Y1 * obj, Y2 * obj, Y3 * obj,
                Z1 * obj, Z2 * obj, Z3 * obj);
        }
        public IMatrix<double> Divide(double obj)
        {
            return Multiply(1 / obj);
        }

        public static MatrixD3 operator +(MatrixD3 a, MatrixD3 b) => (MatrixD3)a.Add(b);
        public static MatrixD3 operator -(MatrixD3 a, MatrixD3 b) => (MatrixD3)a.Subtract(b);
        public static MatrixD3 operator *(MatrixD3 a, MatrixD3 b) => (MatrixD3)a.Multiply(b);
        public static MatrixD3 operator *(MatrixD3 b, double a) => (MatrixD3)b.Multiply(a);
        public static Vector3 operator *(MatrixD3 a, VectorD3 b) => (VectorD3)a.Multiply(b);
        public static MatrixD3 operator /(MatrixD3 a, double b) => (MatrixD3)a.Divide(b);

        public MatrixD3 CreateTranslationX(double x)
        {
            return new MatrixD3(0, 0, 1, 0, 1, 0, x, 0, 1);
        }
        public MatrixD3 CreateTranslationY(double y)
        {
            return new MatrixD3(0, 0, 1, 0, 1, 0, 0, y, 1);
        }
        public MatrixD3 CreateTranslation(double x, double y)
        {
            return new MatrixD3(0, 0, 1, 0, 1, 0, x, y, 1);
        }

        public MatrixD3 CreateScaleX(double x)
        {
            return new MatrixD3(new MatrixD2(x, 0, 0, 1));
        }
        public MatrixD3 CreateScaleY(double y)
        {
            return new MatrixD3(new MatrixD2(1, 0, 0, y));
        }
        public MatrixD3 CreateScale(double x, double y)
        {
            return new MatrixD3(new MatrixD2(x, 0, 0, y));
        }

        public MatrixD3 CreateRotation(double deg)
        {
            var cos = Math.Cos(deg * 180 / Math.PI);
            var sin = Math.Cos(deg * 180 / Math.PI);

            return new MatrixD3(new MatrixD2(cos, -sin, sin, cos));
        }
    }
}
