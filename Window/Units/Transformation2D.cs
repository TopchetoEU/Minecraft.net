using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftNetWindow.Units
{
    public class Transformation2D
    {
        /// <summary>
        /// A default transformation
        /// </summary>
        public static readonly Transformation2D Zero = new Transformation2D(0, 0, 1, 1, 0);

        /// <summary>
        /// Position of the transformation
        /// </summary>
        public Point2D Position { get; }
        /// <summary>
        /// Scale of the transformation
        /// </summary>
        public Size2D Scale { get; }
        /// <summary>
        /// Rotation of the transformation
        /// </summary>
        public float Rotation { get; }

        /// <summary>
        /// Creates a transformation
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="width">Width (multiple)</param>
        /// <param name="height">Height (multiple)</param>
        /// <param name="rotation">Rotation</param>
        public Transformation2D(int x, int y, int width, int height, int rotation)
        {
            Position = new Point2D(x, y);
            Scale = new Size2D(width, height);
            Rotation = rotation % 360;
        }

        /// <summary>
        /// Creates a transformation
        /// </summary>
        /// <param name="position">Position</param>
        /// <param name="scale">Scale (multiple)</param>
        /// <param name="rotation">Rotation</param>
        public Transformation2D(Point2D position, Size2D scale, int rotation)
        {
            Position = position;
            Scale = scale;
            Rotation = rotation % 360;
        }
    }
}
