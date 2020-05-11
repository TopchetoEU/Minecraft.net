using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using MinecraftNetWindow.MainWindow;
using MinecraftNetWindow.Units;

namespace MinecraftNetWindow.GUI
{
    /// <summary>
    /// Regular button
    /// </summary>
    public class GUIButton : GUIElement
    {
        private Atlas atlas;

        public bool HoverOverlay { get; set; }
        public bool ActiveOverlay { get; set; }

        private GUIMaterial bgSprite;
        private GUIMaterial hvSprite;
        private GUIMaterial acSprite;


        /// <summary>
        /// Hover color (if hover texture is <c>null</c>)
        /// </summary>
        public GUIMaterial BackgroundSprite { 
            get => bgSprite;
            set {
                bgSprite = value;
                Update();
            }
        }
        /// <summary>
        /// Hover texture
        /// </summary>
        public GUIMaterial HoverSprite {
            get => hvSprite;
            set {
                hvSprite = value;
                Update();
            }
        }
        /// <summary>
        /// Active texture
        /// </summary>
        public GUIMaterial ActiveSprite {
            get => acSprite;
            set {
                acSprite = value;
                Update();
            }
        }

        private string text = "";

        /// <summary>
        /// The text of the button
        /// </summary>
        public string Text {
            get => text;
            set {
                text = value;
                Update();
            }
        }

        private GUITexture textTexture;

        private void RenderText()
        {
            var tempGraphics = Graphics.FromHwnd(IntPtr.Zero);
            var font = new Font(FontFamily.GenericSansSerif, 10);
            var textSize = tempGraphics.MeasureString(text, font);

            var bmp = new Bitmap((int)textSize.Width + 1, (int)textSize.Height + 1);
            var g = Graphics.FromImage(bmp);
            g.DrawString(text, font, Brushes.Black, 0, 0);

            if (textTexture == null) textTexture = atlas.LoadImage(bmp);
            else textTexture = atlas.ReplaceTexture(bmp, textTexture);

            g.Dispose();
            bmp.Dispose();
        }
        /// <inheritdoc/>
        protected override void Render()
        {
            var texts = new List<GUIMaterial>();

            RenderText();

            texts.Insert(0, new GUIMaterial(textTexture, new Transformation2D(0, 0, 1, 1, 0)));

            var hover = Mouse.MouseOver;
            var active = Mouse.Left || Mouse.Right || Mouse.Middle;
            if (active)
            {
                if (ActiveSprite != null) texts.Insert(0, ActiveSprite);

                if (ActiveOverlay) return;
            } else if (hover)
            {
                if (HoverSprite != null) texts.Insert(0, HoverSprite);

                if (HoverOverlay) return;
            }

            if (BackgroundSprite != null) texts.Insert(0, BackgroundSprite);

            Textures = texts.ToArray();
        }

        /// <summary>
        /// Creates a button element
        /// </summary>
        /// <param name="atlas">Atlas to store textures</param>
        /// <param name="size">Size of the button</param>
        /// <param name="text">Text of the button</param>
        public GUIButton(Atlas atlas, Size2D size, string text = "")
        {
            this.atlas = atlas;
            Size = size;
            Text = text;

            Mouse.MouseEntered += Mouse_MouseChanged;
            Mouse.MouseLeft += Mouse_MouseChanged;
            Mouse.MousePressed += Mouse_MousePressed;
            Mouse.MouseReleased += Mouse_MousePressed;
        }

        private void Mouse_MousePressed(object sender, MouseEventArgs e) => Render();
        private void Mouse_MouseChanged(object sender, EventArgs e) => Render();
    }
}
