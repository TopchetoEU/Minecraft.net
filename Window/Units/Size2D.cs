using System.Drawing;
using System.Runtime.Remoting.Messaging;

namespace MinecraftNetWindow.Units
{
    /// <summary>
    /// A 2D size of an geometric shape in space, measured in <see cref="float"/>
    /// </summary>
    public class Size2D
    {
        /// <summary>
        /// Zero size (0, 0)
        /// </summary>
        public static readonly Size2D Zero = new Size2D(0, 0);

        /// <summary>
        /// The width of the size
        /// </summary>
        public float Width { get; }
        /// <summary>
        /// The Y coordinate
        /// </summary>
        public float Height { get; }

        /// <summary>
        /// Creates new two-dimensional point
        /// </summary>
        /// <param name="width">The width of the size</param>
        /// <param name="height">The height of the size</param>
        public Size2D(float width, float height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Converts a <see cref="Size"/> to <see cref="Size2D"/>
        /// </summary>
        /// <param name="size"><see cref="Size"/> to be converted</param>
        public Size2D(Size size)
        {
            Width = size.Width;
            Height = size.Height;
        }

        /// <summary>
        /// Sums both measure of sizes, respectively
        /// </summary>
        /// <param name="a">First size</param>
        /// <param name="b">second size</param>
        /// <returns>The "summed" sizes</returns>
        public static Size2D operator +(Size2D a, Size2D b)
        {
            return new Size2D(a.Width + b.Width, a.Height + b.Height);
        }

        /// <summary>
        /// Subtracts both measures of the , respectively
        /// </summary>
        /// <param name="a">First size</param>
        /// <param name="b">second size</param>
        /// <returns>The difference between the sizes</returns>
        public static Size2D operator -(Size2D a, Size2D b)
        {
            return new Size2D(a.Width - b.Width, a.Height - b.Height);
        }

        /// <summary>
        /// Converts <see cref="Size2D"/> to <see cref="Size"/>
        /// </summary>
        /// <returns>The converted size</returns>
        public SizeF ToSystemDrawingSizeF() => new SizeF(Width, Height);

        public static Size2D operator *(Size2D size, Size2D scale)
            => new Size2D(size.Width * scale.Width, size.Height * scale.Height);
    }
}