using NetGL.GraphicsAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetGL
{
    [Vector(2, GraphicsType.Float)]
    public struct Point2: IVector<float>
    {
        [VectorDimension(0)] public float X { get; set; }
        [VectorDimension(1)] public float Y { get; set; }

        public float Length {
            get => (float)Math.Sqrt(LengthSquared);
            set {
                var x = X / Length * value;
                var y = Y / Length * value;

                X = x; Y = y;
            }
        }
        public float LengthSquared {
            get => X * X + Y * Y;
            set => Length = (float)Math.Sqrt(value);
        }

        public Point2(float x, float y)
        {
            X = x;
            Y = y;
        }
        public Point2(float val) : this(val, val) { }

        public float Dot(IVector<float> vector)
        {
            if (!(vector is Point2)) throw new Exception("Vector isn't instance of Point2");
            var vec = (Point2)vector;

            return X * vec.X + Y * vec.Y;
        }

        public IVector<float> Add(IVector<float> obj)
        {
            if (!(obj is Point2)) throw new Exception("Vector isn't instance of Point2");
            var vec = (Point2)obj;

            return new Point2(X + vec.X, Y + vec.Y);
        }
        public IVector<float> Subtract(IVector<float> obj)
        {
            if (!(obj is Point2)) throw new Exception("Vector isn't instance of Point2");
            var vec = (Point2)obj;

            return new Point2(X - vec.X, Y - vec.Y);
        }
        public IVector<float> Multiply(float obj)
        {
            return new Point2(X * obj, Y * obj);
        }
        public IVector<float> Divide(float obj)
        {
            return new Point2(X / obj, Y / obj);
        }

        public float[] Flattern() => new[] { X, Y };
    }

    public interface IAddable<InT, OutT>
    {
        public OutT Add(InT obj);
    }
    public interface ISubtractable<InT, OutT>
    {
        public OutT Subtract(InT obj);
    }
    public interface IMultipilable<InT, OutT>
    {
        public OutT Multiply(InT obj);
    }
    public interface IDivideable<InT, OutT>
    {
        public OutT Divide(InT obj);
    }

    public interface IVector<T> : 
        IAddable<IVector<T>, IVector<T>>,      ISubtractable<IVector<T>, IVector<T>>,
        IMultipilable<float, IVector<T>>,      IDivideable<float, IVector<T>> where T: struct
    {
        float Length { get; set; }
        float LengthSquared { get; set; }

        float Dot(IVector<T> vector);

        T[] Flattern();
    }
}
