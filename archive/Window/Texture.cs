using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Collections.Generic;
using System;

namespace MinecraftNetWindow
{
    public class Texture: IDisposable
    {
        public static int BoundTextureID { get; private set; }
        public int ID { get; private set; }
        public Size Size { get; private set; }

        private TextureMagFilter textureFilter;
        public TextureMagFilter TextureFilter {
            get => textureFilter;
            set {
                textureFilter = value;

                Use();
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)value);
            }
        }

        public void Use()
        {
            if (ID != BoundTextureID)
                GL.BindTexture(TextureTarget.Texture2D, ID);
            BoundTextureID = ID;
        }
        public void LoadBitmap(Bitmap bitmap)
        {
            Size = bitmap.Size;

            var pixels = new List<byte>();

            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    var pixel = bitmap.GetPixel(x, y);
                    pixels.Add(pixel.R);
                    pixels.Add(pixel.G);
                    pixels.Add(pixel.B);
                    pixels.Add(pixel.A);
                }
            }

            Use();

            GL.TexImage2D(
                TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba,
                bitmap.Width, bitmap.Height, 0, PixelFormat.Rgba,
                PixelType.UnsignedByte, pixels.ToArray());
        }
        public void DrawBitmap(Bitmap bitmap, int x1, int y1)
        {
            var pixels = new List<byte>();

            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    var pixel = bitmap.GetPixel(x, y);
                    pixels.Add(pixel.R);
                    pixels.Add(pixel.G);
                    pixels.Add(pixel.B);
                    pixels.Add(pixel.A);
                }
            }

            Use();

            GL.TexSubImage2D(
                TextureTarget.Texture2D, 0, x1, y1,
                bitmap.Width, bitmap.Height, PixelFormat.Rgba, PixelType.UnsignedByte, pixels.ToArray());
        }

        private void Setup()
        {
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);

            TextureFilter = TextureMagFilter.Nearest;
        }

        public Texture()
        {
            ID = GL.GenTexture();

            LoadBitmap(new Bitmap(1, 1));

            Setup();
        }
        public Texture(Bitmap bitmap)
        {
            ID = GL.GenTexture();

            LoadBitmap(bitmap);

            Setup();
        }
        public Texture(string path)
        {
            ID = GL.GenTexture();

            LoadBitmap(new Bitmap(path));

            Setup();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    GL.DeleteTexture(ID);
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
