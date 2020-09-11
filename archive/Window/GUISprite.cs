using System.Drawing;

namespace MinecraftNetWindow
{
    public class GUISprite
    {
        public PointF Position { get; set; }
        public SizeF Size { get; set; }

        public Texture Texture { get; }
        public RectangleF Area { get; }

        public GUISprite(PointF position, SizeF size, Texture texture, RectangleF area)
        {
            Position = position; Size = size;
            Texture = texture; Area = area;
        }
    }
}
