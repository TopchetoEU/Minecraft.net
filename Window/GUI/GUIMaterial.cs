using MinecraftNetWindow.Units;
using System.Drawing;

namespace MinecraftNetWindow.GUI
{
    /// <summary>
    /// A texture to be displayed by the renderer
    /// </summary>
    /// 
    public class GUIMaterial
    {
        /// <summary>
        /// The pending texture to be displayed
        /// </summary>
        public GUITexture Texture { get; set; }
        /// <summary>
        /// Transformations of the texture
        /// </summary>
        public Transformation2D Transformation { get; set; }
        private Size2D size;

        public Size2D Size {
            get {
                if (Texture != null)
                    return Texture.TextureArea.Size;
                else
                    return size;
            }
            set {
                size = value;
            }
        }
        public Color Color { get; set; }

        public GUIMaterial(GUITexture texture, Transformation2D transformation)
        {
            Texture = texture;
            Transformation = transformation ?? new Transformation2D(0, 0, 1, 1, 0);
        }
        public GUIMaterial(Color color, Transformation2D transformation, Size2D size)
        {
            Color = color;
            Transformation = transformation;
            Size = size;
        }
    }
}
