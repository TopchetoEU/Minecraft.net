using NetGL.GraphicsAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetGL
{
    [Vector(2, MultiDimensionType.Float)]
    public struct Vector2: IVector<float>
    {
        public static Vector2 Zero => new Vector2(0, 0);
        [VectorDimension(0)] public float X { get; set; }
        [VectorDimension(1)] public float Y { get; set; }

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

        public float this[int component] {
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
                        break;
                    default:
                        throw new Exception("Invalid component index");
                }
            }
        }

        public Vector2 this[char a, char b] {
            get {
                var x = getCharComponent(a);
                var y = getCharComponent(b);

                return new Vector2(this[x], this[y]);
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
        public float this[char component] {
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

                X = x; Y = y;
            }
        }
        public float LengthSquared {
            get => X * X + Y * Y;
            set => Length = (float)Math.Sqrt(value);
        }

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }
        public Vector2(float val) : this(val, val) { }

        public float Dot(IVector<float> vector)
        {
            if (!(vector is Vector2)) throw new Exception("Vector isn't instance of Point2");
            var vec = (Vector2)vector;

            return X * vec.X + Y * vec.Y;
        }

        public IVector<float> Add(IVector<float> obj)
        {
            if (!(obj is Vector2)) throw new Exception("Vector isn't instance of Point2");
            var vec = (Vector2)obj;

            return new Vector2(X + vec.X, Y + vec.Y);
        }
        public IVector<float> Subtract(IVector<float> obj)
        {
            if (!(obj is Vector2)) throw new Exception("Vector isn't instance of Point2");
            var vec = (Vector2)obj;

            return new Vector2(X - vec.X, Y - vec.Y);
        }
        public IVector<float> Multiply(float obj)
        {
            return new Vector2(X * obj, Y * obj);
        }
        public IVector<float> Divide(float obj)
        {
            return new Vector2(X / obj, Y / obj);
        }

        public float[] Flattern() => new[] { X, Y };

        public static implicit operator VectorI2(Vector2 pt)
        {
            return new VectorI2((int)pt.X, (int)pt.Y);
        }

        public static Vector2 operator +(Vector2 a, Vector2 b) => (Vector2)a.Add(b);
        public static Vector2 operator -(Vector2 a, Vector2 b) => (Vector2)a.Subtract(b);
        public static Vector2 operator /(Vector2 a, float b) => (Vector2)a.Divide(b);
        public static Vector2 operator *(Vector2 a, float b) => (Vector2)a.Multiply(b);
    }
}
