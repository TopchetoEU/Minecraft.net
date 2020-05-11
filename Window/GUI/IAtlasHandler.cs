using MinecraftNetWindow.Units;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftNetWindow.GUI
{
    /// <summary>
    /// A handler for atlas
    /// </summary>
    public interface IAtlasHandler
    {
        /// <summary>
        /// Sets a region of the atlas
        /// </summary>
        /// <param name="bitmap">Bitmap to put</param>
        /// <param name="position">Position at which to insert</param>
        void SetImageRegion(Bitmap bitmap, Point2D position);
        /// <summary>
        /// Creates new atlas canvas, resizes the canvas, if it exists
        /// </summary>
        /// <param name="size">Size of new atlas</param>
        void CreateNewImage(Size2D size);
        /// <summary>
        /// Prepares the atlas for use (required for GLAtlasHandler)
        /// </summary>
        void UseAtlas();
    }
}
