using System;
using System.Drawing;

namespace MinecraftNetWindow
{
    public class GUITextElement: GUIElement
    {
        Texture textTexture;
        protected override void Render()
        {
            var font = new Font(FontFamily.GenericMonospace, 15);

            var w = (int)Size.Width;
            var h = (int)Size.Height;

            w = w < 1 ? 1 : w;
            h = h < 1 ? 1 : h;

            var b = new Bitmap(w, h);
            var g = Graphics.FromImage(b);

            g.DrawString(text, font, Brushes.White, 0, 0);
            g.Dispose();

            textTexture.LoadBitmap(b);

            Sprites = new[] { 
                new GUISprite(Position, Size, textTexture, new RectangleF(0, 0, 1, 1)) 
            };
        }

        private string text = "hey!";
        private Font font = new Font(FontFamily.GenericMonospace, 16);
        public string Text {
            get => text;
            set {
                text = value;
                Update();
            }
        }
        public Font Font {
            get => font;
            set {
                font = value;
                Update();
            }
        }

        public GUITextElement()
        {
            textTexture = new Texture();
        }

        protected override void UpdateSize()
        {
            var horisontal = AutoSize == AutoSizeDirection.Horisontal;
            var vertical = AutoSize == AutoSizeDirection.Vertical;
            var both = AutoSize == AutoSizeDirection.Both;

            horisontal = horisontal || both;
            vertical = vertical || both;

            var tempGraphics = Graphics.FromHwnd(IntPtr.Zero);
            var auto = tempGraphics.MeasureString(text, font);

            if (horisontal)
                size.Width = auto.Width;
            if (vertical)
                size.Height = auto.Height;
        }
    }
}
