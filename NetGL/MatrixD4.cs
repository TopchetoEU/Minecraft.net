using NetGL.GraphicsAPI;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace NetGL
{
    [Matrix(4, 4, MultiDimensionType.Double)]
    public struct MatrixD4: IMatrix<double>
    {
        public static MatrixD4 Identity { get; } = new MatrixD4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);

        public double X1 { get; set; }
        public double X2 { get; set; }
        public double X3 { get; set; }
        public double X4 { get; set; }

        public double Y1 { get; set; }
        public double Y2 { get; set; }
        public double Y3 { get; set; }
        public double Y4 { get; set; }

        public double Z1 { get; set; }
        public double Z2 { get; set; }
        public double Z3 { get; set; }
        public double Z4 { get; set; }

        public double W1 { get; set; }
        public double W2 { get; set; }
        public double W3 { get; set; }
        public double W4 { get; set; }

        public double[] Flattern()
        {
            return new double[] {
                X1, X2, X3, X4,
                Y1, Y2, Y3, Y4,
                Z1, Z2, Z3, Z4,
                W1, W2, W3, W4
            };
        }

        public MatrixD4(
            double x1, double x2, double x3, double x4,
            double y1, double y2, double y3, double y4,
            double z1, double z2, double z3, double z4,
            double w1, double w2, double w3, double w4)
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
        public MatrixD4(MatrixD2 mt) : this(
            mt.X1, mt.X2, 0, 0,
            mt.Y1, mt.Y2, 0, 0,
            0, 0, 1, 0,
            0, 0, 0, 1)
        { }
        public MatrixD4(MatrixD3 mt) : this(
            mt.X1, mt.X2, mt.X3, 0,
            mt.Y1, mt.Y2, mt.Y3, 0,
            mt.Z1, mt.Z2, mt.Z3, 0,
            0, 0, 0, 1)
        { }

        public double Determinant {
            get {
                var a = new MatrixD3(Y2, Z2, W2, Y3, Z3, W3, Y4, Z4, W4).Determinant;
                var b = new MatrixD3(Y1, Z1, W1, Y3, Z3, W3, Y4, Z4, W4).Determinant;
                var c = new MatrixD3(Y1, Z1, W1, Y2, Z2, W2, Y4, Z4, W4).Determinant;
                var d = new MatrixD3(Y1, Z1, W1, Y2, Z2, W2, Y3, Z3, W3).Determinant;

                return a - b + c - d;
            }
        }

        public IMatrix<double> Add(IMatrix<double> obj)
        {
            if (!(obj is MatrixD4)) throw new Exception("obj is not of type MatrixD4");

            var m = (MatrixD4)obj;

            return new MatrixD4(
                X1 + m.X1, X2 + m.X2, X3 + m.X3, X4 + m.X4,
                Y1 + m.Y1, Y2 + m.Y2, Y3 + m.Y3, Y4 + m.Y4,
                Z1 + m.Z1, Z2 + m.Z2, Z3 + m.Z3, Z4 + m.Z4,
                W1 + m.W1, W2 + m.W2, W3 + m.W3, W4 + m.W4
            );
        }
        public IMatrix<double> Subtract(IMatrix<double> obj)
        {
            if (!(obj is MatrixD4)) throw new Exception("obj is not of type MatrixD4");

            var m = (MatrixD4)obj;

            return new MatrixD4(
                X1 - m.X1, X2 - m.X2, X3 - m.X3, X4 - m.X4,
                Y1 - m.Y1, Y2 - m.Y2, Y3 - m.Y3, Y4 - m.Y4,
                Z1 - m.Z1, Z2 - m.Z2, Z3 - m.Z3, Z4 - m.Z4,
                W1 - m.W1, W2 - m.W2, W3 - m.W3, W4 - m.W4
            );
        }

        public IMatrix<double> Inverse()
        {
            return Divide(Determinant);
        }

        public IMatrix<double> Multiply(IMatrix<double> obj)
        {
            if (!(obj is MatrixD4)) throw new Exception("obj is not of type MatrixD4");

            var m = (MatrixD4)obj;

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

            return new MatrixD4(
                x1, x2, y3, x4,
                y1, y2, y3, y4,
                z1, z2, z3, z4,
                w1, w2, w3, w4
            );
        }
        public IVector<double> Multiply(IVector<double> obj)
        {
            if (!(obj is VectorD4)) throw new Exception("Vector is not of type VectorD4");

            var p = (VectorD4)obj;

            var x = X1 * p.X + X2 * p.X + X3 * p.X + X4 * p.X;
            var y = Y1 * p.Y + Y2 * p.Y + Y3 * p.Y + Y4 * p.Y;
            var z = Z1 * p.Z + Z2 * p.Z + Z3 * p.Z + Z4 * p.Z;
            var w = W1 * p.W + W2 * p.W + W3 * p.W + W4 * p.W;

            return new VectorD4(x, y, z, w);
        }
        public IMatrix<double> Multiply(double obj)
        {
            return new MatrixD4(
                X1 * obj, X2 * obj, X3 * obj, X4 * obj,
                Y1 * obj, Y2 * obj, Y3 * obj, Y4 * obj,
                Z1 * obj, Z2 * obj, Z3 * obj, Z4 * obj,
                W1 * obj, W2 * obj, W3 * obj, W4 * obj
            );
        }
        public IMatrix<double> Divide(double obj)
        {
            return Multiply(1 / obj);
        }

        public static MatrixD4 operator +(MatrixD4 a, MatrixD4 b) => (MatrixD4)a.Add(b);
        public static MatrixD4 operator -(MatrixD4 a, MatrixD4 b) => (MatrixD4)a.Subtract(b);
        public static MatrixD4 operator *(MatrixD4 a, MatrixD4 b) => (MatrixD4)a.Multiply(b);
        public static MatrixD4 operator *(MatrixD4 b, double a) => (MatrixD4)b.Multiply(a);
        public static Vector4 operator *(MatrixD4 a, VectorD4 b) => (VectorD4)a.Multiply(b);
        public static MatrixD4 operator /(MatrixD4 a, double b) => (MatrixD4)a.Divide(b);

        public MatrixD4 CreateTranslationX(double x)
        {
            return new MatrixD4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, x, 0, 0, 1);
        }
        public MatrixD4 CreateTranslationY(double y)
        {
            return new MatrixD4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, y, 0, 1);
        }
        public MatrixD4 CreateTranslationZ(double z)
        {
            return new MatrixD4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, z, 1);
        }
        public MatrixD4 CreateTranslation(double x, double y, double z)
        {
            return new MatrixD4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, x, y, z, 1);
        }

        public MatrixD4 CreateScaleX(double x)
        {
            return new MatrixD4(new MatrixD3(x, 0, 0, 0, 1, 0, 0, 0, 1));
        }
        public MatrixD4 CreateScaleY(double y)
        {
            return new MatrixD4(new MatrixD3(1, 0, 0, 0, y, 0, 0, 0, 1));
        }
        public MatrixD4 CreateScaleZ(double z)
        {
            return new MatrixD4(new MatrixD3(1, 0, 0, 0, 1, 0, 0, 0, z));
        }
        public MatrixD4 CreateScale(double x, double y, double z)
        {
            return new MatrixD4(new MatrixD3(x, 0, 0, 0, y, 0, 0, 0, z));
        }

        public MatrixD4 CreateRotationX(double deg)
        {
            var cos = Math.Cos(deg * 180 / Math.PI);
            var sin = Math.Cos(deg * 180 / Math.PI);

            return new MatrixD4(new MatrixD3(1, 0, 0, 0, cos, -sin, 0, sin, cos));
        }
        public MatrixD4 CreateRotationY(double deg)
        {
            var cos = Math.Cos(deg * 180 / Math.PI);
            var sin = Math.Cos(deg * 180 / Math.PI);

            return new MatrixD4(new MatrixD3(cos, 0, sin, 0, 1, 0, -sin, 0, cos));
        }
        public MatrixD4 CreateRotationZ(double deg)
        {
            var cos = Math.Cos(deg * 180 / Math.PI);
            var sin = Math.Cos(deg * 180 / Math.PI);

            return new MatrixD4(new MatrixD2(cos, -sin, sin, cos));
        }
    }
}
