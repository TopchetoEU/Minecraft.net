using NetGL.WindowAPI;
using System.Collections.Generic;
using System.Drawing;

namespace NetGL.GraphicsAPI
{
    /// <summary>
    /// Graphics object, used to modify window-level global states
    /// </summary>
    public class Graphics
    {
        /// <summary>
        /// Clears the specified buffers
        /// </summary>
        /// <param name="type">The buffer to clear (you can clear multiple buffer, by listing the buffers with "|")</param>
        public void Clear(ClearType type = ClearType.ColorBuffer)
        {
            LLGraphics.graphics_clear((uint)type);
        }
        private const uint Blending = 0xFA10;
        private const uint Culling = 0xFA11;
        private const uint _Dithering = 0xFA12;
        private const uint DepthTest = 0xFA13;
        private const uint StencilTest = 0xFA14;

        private bool depthTesting = false;
        private bool stencilTesting = false;
        private bool alphaBlending = false;
        private bool backfaceCulling = false;
        private bool dithering = false;
        private uint depthFunc = 0;
        private uint alphaFunc = 0;
        private uint sourceBlendFunc = 0;
        private uint destBlendFunc = 0;
        private float alphaValue = 0;

        /// <summary>
        /// Depth testing in scene (compares each pixel with the already drawn objects)
        /// </summary>
        public bool DepthTesting {
            get => depthTesting;
            set {
                if (depthTesting != value) {
                    LLGraphics.graphics_setRenderOption(DepthTest, value);
                    depthTesting = value;
                }
            }
        }
        /// <summary>
        /// Stencil testing in scene. Determines wether or not a pixel is drawn, depending on the selected stencil buffer
        /// </summary>
        public bool StencilTesting {
            get => stencilTesting;
            set {
                if (value != stencilTesting) {
                    LLGraphics.graphics_setRenderOption(StencilTest, value);
                    stencilTesting = value;
                }
            }
        }
        /// <summary>
        /// When two transparent primitives overlap, blends between them to produce another color
        /// </summary>
        public bool AlphaBlending {
            get => alphaBlending;
            set {
                if (alphaBlending != value) {
                    LLGraphics.graphics_setRenderOption(Blending, value);
                    alphaBlending = value;
                }
            }
        }
        /// <summary>
        /// Discards faces that are not pointing towards the camera (primitive vertices must be going clockwide)
        /// </summary>
        public bool BackfaceCulling {
            get => backfaceCulling;
            set {
                if (backfaceCulling != value) {
                    LLGraphics.graphics_setRenderOption(Culling, value);
                    backfaceCulling = value;
                }
            }
        }
        /// <summary>
        /// Uses dithering for expressing higher bit colors
        /// </summary>
        public bool Dithering {
            get => dithering;
            set {
                if (dithering != value) {
                    LLGraphics.graphics_setRenderOption(_Dithering, value);
                    dithering = value;
                }
            }
        }

        /// <summary>
        /// The comparasion function for depth testing
        /// </summary>
        public ComparativeFunc DepthTestFunction {
            get => (ComparativeFunc)depthFunc;
            set {
                if (depthFunc != (uint)value) {
                    LLGraphics.graphics_setDepthTestOptions((uint)value);
                    depthFunc = (uint)value;
                }
            }
        }
        private ComparativeFunc AlphaTestFunction {
            get => (ComparativeFunc)alphaFunc;
            set {
                if ((uint)value != alphaFunc) {
                    LLGraphics.graphics_setAlphaTestOptions((uint)value, alphaValue);
                    alphaFunc = (uint)value;
                }
            }
        }
        private float AlphaValue {
            get => alphaValue;
            set {
                if (alphaValue != value) {
                    LLGraphics.graphics_setAlphaTestOptions(alphaFunc, value);
                    alphaValue = value;
                }
            }
        }

        /// <summary>
        /// The blending function for the primitive drawn
        /// </summary>
        public BlendingFunc SourceBlendFunction {
            get => (BlendingFunc)sourceBlendFunc;
            set {
                if ((uint)value != sourceBlendFunc) {
                    LLGraphics.graphics_setBlendingOptions((uint)value, destBlendFunc);
                    sourceBlendFunc = (uint)value;
                }
            }
        }
        /// <summary>
        /// The blending function for the already drawn on screen
        /// </summary>
        public BlendingFunc DestinationBlendFunction {
            get => (BlendingFunc)destBlendFunc;
            set {
                if ((uint)value != sourceBlendFunc) {
                    LLGraphics.graphics_setBlendingOptions(sourceBlendFunc, (uint)value);
                    destBlendFunc = (uint)value;
                }
            }
        }

        /// <summary>
        /// The background color of the window
        /// </summary>
        public Vector4 BackgorundColor {
            get {
                float r = 0, g = 0, b = 0, a = 0;

                LLGraphics.graphics_getBackground(ref r, ref g, ref b, ref a);

                return new Vector4(r, g, b, a);
            }
            set => LLGraphics.graphics_setBackground(value.X, value.Y, value.Z, value.W);
        }

        internal Graphics()
        {
        }

        public void DrawObject(VBO vbo, EBO ebo = null)
        {
            if (ebo != null) {

                vbo.Use();

                uint size = 0;

                switch (ebo.Primitive) {
                    case GraphicsPrimitive.Lines:
                    case GraphicsPrimitive.LineLoop:
                    case GraphicsPrimitive.LineStrip:
                        size = 2;
                        break;
                    case GraphicsPrimitive.Triangles:
                    case GraphicsPrimitive.TriangleStrip:
                    case GraphicsPrimitive.TriangleFan:
                        size = 3;
                        break;
                    case GraphicsPrimitive.Quads:
                    case GraphicsPrimitive.QuadStrip:
                        size = 4;
                        break;
                    case GraphicsPrimitive.Polygon:
                        size = vbo.Length;
                        break;
                }

                vbo.Use();
                ebo.Use();

                LLGraphics.graphics_drawElement((uint)ebo.Primitive, ebo.Length / size, ebo.Length, ebo.ID, vbo.ID);
            }
            else {
                vbo.Draw(GraphicsPrimitive.Triangles);
            }
        }
        public void DrawObject(VBO vbo, GraphicsPrimitive primitive)
        {
            vbo.Draw(primitive);
        }

        public EBO CreateElementBuffer(GraphicsPrimitive primitive = GraphicsPrimitive.Triangles)
        {
            return new EBO(primitive);
        }
        public VBO CreateVertexBuffer(ShaderProgram program)
        {
            return new VBO(program);
        }

        public Texture2D CreateTexture2D(uint width, uint height)
        {
            return new Texture2D(width, height);
        }
        public Texture2D CreateTexture2DFromBitmap(Bitmap bmp) => new Texture2D(bmp);

        public ShaderProgram CreateShaderProgram(params Shader[] shaders)
        {
            return new ShaderProgram(shaders);
        }
        public Shader CreateShader(string source, ShaderType type)
        {
            return new Shader(source, type);
        }
        public Shader CreateShaderFromFile(string path, ShaderType type)
        {
            return Shader.FromFile(path, type);
        }
    }
}
