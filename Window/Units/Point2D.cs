using System;
using System.Drawing;

namespace MinecraftNetWindow.Units
{
    /// <summary>
    /// A two-dimensional point in space; Measured in <see cref="int"/>
    /// </summary>
    public class Point2D
    {
        /// <summary>
        /// Position, located at(0, 0)
        /// </summary>
        public static readonly Point2D Zero = new Point2D(0, 0);

        /// <summary>
        /// The X coordinate
        /// </summary>
        public float X
        {
            get;
        }
        /// <summary>
        /// The Y coordinate
        /// </summary>
        public float Y
        {
            get;
        }

        /// <summary>
        /// Creates new two-dimentional point
        /// </summary>
        /// <param name="x">The X coordinate</param>
        /// <param name="y">The Y coordinate</param>
        public Point2D(float x, float y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Converts a <see cref="Point"/> to <see cref="Point2D"/>
        /// </summary>
        /// <param name="point">The point to be converted</param>
        public Point2D(Point point)
        {
            X = point.X;
            Y = point.Y;
        }

        /// <summary>
        /// Sums both coordinates of positions, respectively
        /// </summary>
        /// <param name="a">First point</param>
        /// <param name="b">second point</param>
        /// <returns>The "summed" point</returns>
        public static Point2D operator +(Point2D a, Point2D b)
        {
            return new Point2D(a.X + b.X, a.Y + b.Y);
        }

        /// <summary>
        /// Subtracts both coordinates of positions, respectively
        /// </summary>
        /// <param name="a">First point</param>
        /// <param name="b">second point</param>
        /// <returns>The difference between the point</returns>
        public static Point2D operator -(Point2D a, Point2D b)
        {
            return new Point2D(a.X - b.X, a.Y - b.Y);
        }

        /// <summary>
        /// Checks if a point is in a rectangle
        /// </summary>
        /// <param name="rectangle">The rectangle to check for</param>
        /// <returns>Is the point in the rectangle</returns>
        public bool InRectangle(Rectangle rectangle)
        {
            return X > rectangle.Position.X && X < rectangle.SecondPosition.X &&
                   Y > rectangle.Position.Y && Y < rectangle.SecondPosition.Y;
        }

        /// <summary>
        /// Converts <see cref="Point2D"/> to <see cref="Point"/>
        /// </summary>
        /// <returns>The converted point</returns>
        public PointF ToSystemDrawingPointF() => new PointF(X, Y);
    }
}