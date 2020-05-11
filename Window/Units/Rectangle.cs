using MinecraftNetWindow.MainWindow;

namespace MinecraftNetWindow.Units
{
    /// <summary>
    /// A rectangle in 2D space
    /// </summary>
    public class Rectangle
    {
        /// <summary>
        /// The position (the top left corner) of the rectangle
        /// </summary>
        public Point2D Position { get; }

        /// <summary>
        /// The second position (bottom right corner) of the rectangle
        /// </summary>
        public Point2D SecondPosition {
            get => Position + new Point2D(Size.Width, Size.Height);
        }

        /// <summary>
        /// Geometrically signed bottom left corner of the rectangle
        /// </summary>
        public Point2D A { get => new Point2D(Position.X, SecondPosition.Y); }
        /// <summary>
        /// Geometrically signed bottom right corner of the rectangle
        /// </summary>
        public Point2D B { get => SecondPosition; }
        /// <summary>
        /// Geometrically signed top right corner of the rectangle
        /// </summary>
        public Point2D C { get => new Point2D(SecondPosition.X, Position.Y); }
        /// <summary>
        /// Geometrically signed top left corner of the rectangle
        /// </summary>
        public Point2D D { get => Position; }

        /// <summary>
        /// Size of the rectangle
        /// </summary>
        public Size2D Size { get; set; }

        /// <summary>
        /// Creates a rectangle in the 2D space
        /// </summary>
        /// <param name="position">Position of the rectangle</param>
        /// <param name="size">Size of the rectangle</param>
        public Rectangle(Point2D position, Size2D size)
        {
            Position = position;
            Size = size;
        }

        /// <summary>
        /// Creates a rectangle in 2D space
        /// </summary>
        /// <param name="x">X coordinate of rectangle</param>
        /// <param name="y">Y coordinate of rectangle</param>
        /// <param name="width">Width of rectangle</param>
        /// <param name="height">Height of rectangle</param>
        public Rectangle(float x, float y, float width, float height) : this(new Point2D(x, y), new Size2D(width, height)) { }

        /// <summary>
        /// Checks if a rectangle intersects with this rectangle
        /// </summary>
        /// <param name="rectangle">The rectangle to check for</param>
        /// <returns>If the rectangles are intersecting</returns>
        public bool IntersectWithRectangle(Rectangle rectangle)
        {
            return (A.InRectangle(rectangle) &&
                    B.InRectangle(rectangle) &&
                    C.InRectangle(rectangle) &&
                    D.InRectangle(rectangle)) ||
                   (rectangle.A.InRectangle(this) &&
                    rectangle.B.InRectangle(this) &&
                    rectangle.C.InRectangle(this) &&
                    rectangle.D.InRectangle(this));
        }

        public static Rectangle operator *(Rectangle rect, Size2D multiple)
        {
            return new Rectangle(rect.Position.X * multiple.Width, rect.Position.Y * multiple.Height,
                                 rect.Size.Width * multiple.Width, rect.Size.Height * multiple.Height);
        }

        public static Rectangle operator /(Rectangle rect, Size2D multiple)
        {
            return new Rectangle(rect.Position.X / multiple.Width, rect.Position.Y / multiple.Height,
                                 rect.Size.Width / multiple.Width, rect.Size.Height / multiple.Height);
        }
    }
}
