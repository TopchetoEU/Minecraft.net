using OpenTK;
using System.Drawing;
using System.Reflection;

namespace MinecraftNetWindow
{
    public static class ExtensionMethods
    {
        public static float Map(this int value, int fromSource, int toSource, int fromTarget, int toTarget)
        {
            return ((float)value).Map(fromSource, toSource, fromTarget, toTarget);
        }
        public static float Map(this float value, float fromSource, float toSource, float fromTarget, float toTarget)
        {
            return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
        }
        public static Matrix4 GetPointFMatrix(this PointF point)
        {
            return Matrix4.CreateTranslation(point.X, point.Y, 0);
        }
        public static Point Sum(this Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }
        public static Point Subtract(this Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
        }
        public static PointF Multiply(this PointF a, float b)
        {
            return new PointF(a.X * b, a.Y * b);
        }
        public static T MemberwiseClone<T>(this T obj)
        {
            var cloner = typeof(T).GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);

            return (T)cloner.Invoke(obj, null);
        }
    }
}