using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using MinecraftNetWindow.Units;
using OpenTK.Graphics.OpenGL;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;
using Rectangle = MinecraftNetWindow.Units.Rectangle;

namespace MinecraftNetWindow.GUI
{
    /// <summary>
    /// A bigger texture, storing smaller ones
    /// </summary>
    public class Atlas
    {
        private int id;
        private bool hasTexture = false;

        /// <inheritdoc/>
        private void CreateNewImage(Size2D size)
        {
            var newTextName = GL.GenTexture();

            if (hasTexture)
            {
                byte[] pixels = new byte[0];

                GL.BindTexture(TextureTarget.Texture2D, id);
                GL.GetTexImage(TextureTarget.Texture2D, 0, PixelFormat.Rgba, PixelType.Bitmap, pixels);

                GL.BindTexture(TextureTarget.Texture2D, newTextName);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, (int)Size.Width, (int)Size.Height, 0,
                    PixelFormat.Bgra, PixelType.UnsignedByte, pixels);
            }
            else
            {
                var data = new Bitmap((int)size.Width, (int)size.Height)
                    .LockBits(new System.Drawing.Rectangle(0, 0, (int)size.Width, (int)size.Height),
                              ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                var pixels = data.Scan0;

                GL.BindTexture(TextureTarget.Texture2D, newTextName);

                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, (int)Size.Width, (int)Size.Height, 0,
                    PixelFormat.Bgra, PixelType.Bitmap, pixels);
            }

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            id = newTextName;
            Size = size;
            hasTexture = true;
        }

        /// <inheritdoc/>
        public void Use()
        {
            GL.BindTexture(TextureTarget.Texture2D, id);
        }

        /// <inheritdoc/>
        private void SetImageRegion(Bitmap bitmap, Point2D position)
        {
            var pixels = new List<byte>();
            var pixs = bitmap.LockBits(new System.Drawing.Rectangle(Point.Empty, bitmap.Size),
                ImageLockMode.ReadWrite, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Use();
            GL.TexSubImage2D(TextureTarget.Texture2D, 0, (int)position.X, (int)position.Y, bitmap.Width, bitmap.Height, PixelFormat.Bgra, PixelType.UnsignedByte, pixs.Scan0);
            bitmap.UnlockBits(pixs);
        }


        private int index = 0;

        /// <summary>
        /// Size of the atlas
        /// </summary>
        public Size2D Size { get; private set; }

        private List<GUITexture> Textures = new List<GUITexture>();

        /// <summary>
        /// Loads new texture (if possible) into the atlas
        /// </summary>
        /// <param name="bitmap">The new texture</param>
        /// <returns>Id of the texture</returns>
        public GUITexture LoadImage(Bitmap bitmap)
        {
            Rectangle successfulRect = null;

            if (Textures.Count == 0) successfulRect = new Rectangle(Point2D.Zero, new Size2D(bitmap.Size));

            foreach (var texture in Textures)
            {
                var bottom = new Rectangle(texture.TextureArea.A, new Size2D(bitmap.Size));
                var left = new Rectangle(texture.TextureArea.C, new Size2D(bitmap.Size));

                if (!RectangleOccupied(bottom)) { successfulRect = bottom; break; }
                if (!RectangleOccupied(left)) { successfulRect = left; break; }
            }

            if (successfulRect == null) throw new Exception("Image can't be fit. Consider using another atlas");
            var newWidth = Size.Width;
            var newHeight = Size.Height;
            bool change = false;
            if (successfulRect.SecondPosition.X > Size.Width)
            {
                newWidth = Math.Max(successfulRect.SecondPosition.X, Size.Width * 2);
                change = true;
            }
            if (successfulRect.SecondPosition.X > Size.Width)
            {
                newHeight = Math.Max(successfulRect.SecondPosition.Y, Size.Height * 2);
                change = true;
            }

            if (change) CreateNewImage(new Size2D(newWidth, newHeight));

            SetImageRegion(bitmap, successfulRect.Position);

            index++;

            var text = new GUITexture(index - 1, successfulRect / Size, this);

            Textures.Add(text);

            return text;
        }

        /// <summary>
        /// Replaces the bitmap in a texture
        /// </summary>
        /// <param name="bitmap">New bitmap</param>
        /// <param name="texture">Texture to be replaced</param>
        /// <returns>The new texture</returns>
        public GUITexture ReplaceTexture(Bitmap bitmap, GUITexture texture)
        {
            RemoveImage(texture);
            var a = LoadImage(bitmap);

            return new GUITexture(a.TextureID, a.TextureArea, this);
        }

        /// <summary>
        /// Remove a texture from the atlas
        /// </summary>
        /// <param name="texture">Texture to be removed</param>
        public void RemoveImage(GUITexture texture)
        {
            var foundIndex = Textures.FindIndex(v => v.TextureID == texture.TextureID);
            if (foundIndex < 0) throw new Exception("Such a texture doesn't exist");
            Textures.RemoveAt(foundIndex);
        }

        /// <summary>
        /// Checks a texture's existence
        /// </summary>
        /// <param name="texture">Texture</param>
        /// <returns>Boolean, representing the existence of the texture</returns>
        public bool TextureExists(GUITexture texture)
        {
            var foundIndex = Textures.FindIndex(v => v.TextureID == texture.TextureID);
            return foundIndex >= 0;
        }

        private bool RectangleOccupied(Rectangle rect)
        {
            foreach (var texture in Textures)
            {
                if ((texture.TextureArea * Size).IntersectWithRectangle(rect)) return true;
            }

            return false;
        }

        /// <summary>
        /// Creates a texture atlas
        /// </summary>
        /// <param name="width">Width of atlas texture</param>
        /// <param name="height">Height of atlas texture</param>
        public Atlas(int width, int height)
        {
            this.Size = new Size2D(width, height);

            CreateNewImage(Size);
        }
    }
}
