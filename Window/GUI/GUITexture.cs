using System.Drawing;
using System.Linq;
using MinecraftNetWindow.Units;
using Rectangle = MinecraftNetWindow.Units.Rectangle;

namespace MinecraftNetWindow.GUI
{
    /// <summary>
    /// An atlas texture
    /// </summary>
    public class GUITexture
    {
        /// <summary>
        /// Id of the texture in the atlas
        /// </summary>
        public int? TextureID { get; } = null;
        /// <summary>
        /// The area of the texture in the atlas
        /// </summary>
        public Rectangle TextureArea { get; }

        public Atlas Atlas { get; }

        internal GUITexture(int? id, Rectangle textureArea, Atlas atlas)
        {
            TextureID = id;
            TextureArea = textureArea;
            Atlas = atlas;
        }
    }
}
