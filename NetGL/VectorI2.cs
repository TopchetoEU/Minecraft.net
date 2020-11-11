using NetGL.GraphicsAPI;
using System;

namespace NetGL
{
    [Vector(2, MultiDimensionType.Int)]
    public struct VectorI2: IVector<int>
    {
        [VectorDimension(0)] public int X { get; set; }
        [VectorDimension(1)] public int Y { get; set; }

        public int this[int component] {
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

        public VectorI2 this[char a, char b] {
            get {
                var x = getCharComponent(a);
                var y = getCharComponent(b);

                return new VectorI2(this[x], this[y]);
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
        public int this[char component] {
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

                X = (int)x; Y = (int)y;
            }
        }
        public float LengthSquared {
            get => X * X + Y * Y;
            set => Length = (float)Math.Sqrt(value);
        }

        public VectorI2(int x, int y)
        {
            X = x;
            Y = y;
        }
        public VectorI2(int val) : this(val, val) { }

        public int Dot(IVector<int> vector)
        {
            if (!(vector is VectorI2)) throw new Exception("Vector isn't instance of Point2");
            var vec = (VectorI2)vector;

            return X * vec.X + Y * vec.Y;
        }

        public IVector<int> Add(IVector<int> obj)
        {
            if (!(obj is VectorI2)) throw new Exception("Vector isn't instance of Point2");
            var vec = (VectorI2)obj;

            return new VectorI2(X + vec.X, Y + vec.Y);
        }
        public IVector<int> Subtract(IVector<int> obj)
        {
            if (!(obj is VectorI2)) throw new Exception("Vector isn't instance of Point2");
            var vec = (VectorI2)obj;

            return new VectorI2(X - vec.X, Y - vec.Y);
        }
        public IVector<int> Multiply(float obj)
        {
            return new VectorI2((int)(X * obj), (int)(Y * obj));
        }
        public IVector<int> Divide(float obj)
        {
            return new VectorI2((int)(X / obj), (int)(Y / obj));
        }

        public int[] Flattern() => new[] { X, Y };

        public static implicit operator Vector2(VectorI2 pt)
        {
            return new Vector2(pt.X, pt.Y);
        }

        public static VectorI2 operator +(VectorI2 a, VectorI2 b) => (VectorI2)a.Add(b);
        public static VectorI2 operator -(VectorI2 a, VectorI2 b) => (VectorI2)a.Subtract(b);
        public static VectorI2 operator /(VectorI2 a, float b) => (VectorI2)a.Divide(b);
        public static VectorI2 operator *(VectorI2 a, float b) => (VectorI2)a.Multiply(b);
    }
}
