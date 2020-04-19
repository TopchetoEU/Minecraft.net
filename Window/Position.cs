using System;
using System.Drawing;

namespace Minecraft.MainWindow
{
    /// <summary>
    /// A two-dimentional point in the space
    /// </summary>
    public class Position
    {
        /// <summary>
        /// Position, located at(0, 0)
        /// </summary>
        public static readonly Position Zero = new Position(0, 0);

        /// <summary>
        /// Fires when X coordinate has changed
        /// </summary>
        public event EventHandler XChanged;
        /// <summary>
        /// Fires when Y coordinate has changed
        /// </summary>
        public event EventHandler YChanged;

        private int x = 0;
        private int y = 0;
        /// <summary>
        /// The X coordinate
        /// </summary>
        public int X
        {
            get => x;
            set
            {
                x = value;
                XChanged?.Invoke(this, new EventArgs());
            }
        }
        /// <summary>
        /// The Y coordinate
        /// </summary>
        public int Y
        {
            get => y;
            set
            {
                y = value;
                YChanged?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Creates new two-dimentional point
        /// </summary>
        /// <param name="x">The X coordinate</param>
        /// <param name="y">The Y coordinate</param>
        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Converts a <see cref="Point"/> to <see cref="Position"/>
        /// </summary>
        /// <param name="point">The point to be converted</param>
        public Position(Point point)
        {
            x = point.X;
            y = point.Y;
        }
    }
}