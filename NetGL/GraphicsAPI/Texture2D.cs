using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;

namespace NetGL.GraphicsAPI
{
    public enum InterpolationType
    {
        Nearest = 0x2600,
        Linear = 0x2601,
    }
    public class Texture2D: ITexture
    {
        private bool disposedValue;

        public uint Width { get; private set; }
        public uint Height { get; private set; }

        private const uint MagFilter = 0x2800;
        private const uint MinFilter = 0x2801;

        private InterpolationType interpolation;

        public InterpolationType Interpolation {
            get => interpolation;
            set {
                LLGraphics.graphics_setTextureParameter(
                    (uint)TextureTarget.Texture2D, MagFilter, (int)value);
                LLGraphics.graphics_setTextureParameter(
                    (uint)TextureTarget.Texture2D, MinFilter, (int)value);
            }
        }

        public uint Id { get; }

        public uint[] Sizes => new uint[] { Width, Height };

        internal Texture2D(uint width, uint height, Vector4? colorFill = null)
        {
            if (!colorFill.HasValue)
                colorFill = new Vector4(0, 0, 0, 0);

            var genData = new float[4 * width * height];
            for (ulong i = 0; i < width * height; i++) {
                genData[i * 4 + 0] = colorFill.Value.X;
                genData[i * 4 + 1] = colorFill.Value.Y;
                genData[i * 4 + 2] = colorFill.Value.Z;
                genData[i * 4 + 3] = colorFill.Value.W;
            }

            Width = width;
            Height = height;

            Interpolation = InterpolationType.Linear;
            SetData(genData, PixelFormat.RGBA);
        }
        internal Texture2D(Bitmap bmp)
        {
            Id = LLGraphics.graphics_createTexture();
            Use();
            Interpolation = InterpolationType.Linear;
            SetData(bmp);
        }

        public void SetData(float[] data, PixelFormat format)
        {
            SetData((IEnumerable<float>)data, format);
        }
        public void SetData(IEnumerable<float> data, PixelFormat format)
        {
            var width = Width;
            var height = Height;

            var arrId = new NativeArray(data.Cast<object>().ToArray());

            Use();
            LLGraphics.graphics_setTexture2DData(
                (uint)TextureTarget.Texture2D, width, height, (uint)format, (uint)GraphicsType.Float, arrId.Id);

            arrId.Dispose();
        }
        public void SetData(Bitmap bmp)
        {
            Width = (uint)bmp.Width;
            Height = (uint)bmp.Height;

            var newData = new float[Width * Height * 4];

            for (int y = 0; y < bmp.Height; y++) {
                for (int x = 0; x < bmp.Width; x++) {
                    int currPixelId = x + y * bmp.Width;
                    var currPixel = bmp.GetPixel(x, y);

                    newData[currPixelId * 4 + 0] = currPixel.R / 255f;
                    newData[currPixelId * 4 + 1] = currPixel.G / 255f;
                    newData[currPixelId * 4 + 2] = currPixel.B / 255f;
                    newData[currPixelId * 4 + 3] = currPixel.A / 255f;
                }
            }

            SetData(newData, PixelFormat.RGBA);
        }
        public void SetData(string pathName)
        {
            SetData(new Bitmap(pathName));
        }

        public void SetData(float[] data, uint width, uint height, PixelFormat format)
        {
            Width = width;
            Height = height;

            SetData(data, format);
        }

        public void Use()
        {
            LLGraphics.graphics_setTexture(Id, (uint)TextureTarget.Texture2D);
        }
        internal void Bind(uint uniformId)
        {
            Use();
            LLGraphics.graphics_setUniformTex2((uint)TextureTarget.Texture2D, uniformId);
        }
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue) {
                if (disposing) {
                    LLGraphics.graphics_destroyTexture(Id);
                }

                disposedValue = true;
            }
        }
    }
}
