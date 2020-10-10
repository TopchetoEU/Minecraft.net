using NetGL.WindowAPI;
using System;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace NetGL.GraphicsAPI
{
    internal class LLGraphics
    {
        [DllImport(OSDetector.GraphicsDLL)]
        internal static extern void graphics_init();
        [DllImport(OSDetector.GraphicsDLL)]
        internal static extern void graphics_swapBuffers();

        [DllImport(OSDetector.GraphicsDLL)]
        internal static extern uint graphics_createVAO();

        [DllImport(OSDetector.GraphicsDLL)]
        internal static extern void graphics_destroyVAO(uint vaoId);

        [DllImport(OSDetector.GraphicsDLL)]
        internal static extern uint graphics_createBuffer(uint target);

        [DllImport(OSDetector.GraphicsDLL)]
        internal static extern void graphics_destroyBuffer(uint buff);

        [DllImport(OSDetector.GraphicsDLL)]
        internal static extern void graphics_setBuffer(uint target, uint buff);

        [DllImport(OSDetector.GraphicsDLL)]
        internal static extern uint graphics_getBuffer(uint target);

        [DllImport(OSDetector.GraphicsDLL)]
        internal static extern void graphics_setVAO(uint vaoId);

        [DllImport(OSDetector.GraphicsDLL)]
        internal static extern uint graphics_getVAO();

        [DllImport(OSDetector.GraphicsDLL)]
        internal unsafe static extern void graphics_setBufferData(uint target, uint size, uint dataArr, uint usage);

        [DllImport(OSDetector.GraphicsDLL)]
        internal static extern void graphics_getBufferData(uint target, uint size, object[] data);


        [DllImport(OSDetector.GraphicsDLL)]
        internal static extern void graphics_clearBufferAttributes(uint buff);

        [DllImport(OSDetector.GraphicsDLL)]
        internal static extern void graphics_createBufferAttribute(uint buff, uint attrId, uint byteSize, uint type, uint stride, uint offset);

        [DllImport(OSDetector.GraphicsDLL)]
        internal static extern void graphics_destroyBufferAttribute(uint buff, uint attrId);

        [DllImport(OSDetector.GraphicsDLL)]
        internal static extern void graphics_drawBuffer(uint vaoId, uint buff, uint amount, uint mode);


        [DllImport(OSDetector.GraphicsDLL)]
        internal static extern uint graphics_loadNativeArray(NativeArrayElement[] arr, uint size);
        [DllImport(OSDetector.GraphicsDLL)]
        internal static extern uint graphics_createNativeArray(uint size);
        [DllImport(OSDetector.GraphicsDLL)]
        internal static extern void graphics_destroyNativeArray(uint arr);



        [DllImport(OSDetector.GraphicsDLL)]
        internal static extern uint graphics_createShader(uint type, string raw, int length);
        
        [DllImport(OSDetector.GraphicsDLL)]
        internal static extern void graphics_destroyShader(uint shader);

        
        [DllImport(OSDetector.GraphicsDLL)]
        internal static extern bool graphics_getShaderBuildSuccess(uint shaderId);
        
        [DllImport(OSDetector.GraphicsDLL)]
        internal static extern string graphics_getShaderInfoLog(uint shaderId);
        
        [DllImport(OSDetector.GraphicsDLL)]
        internal static extern string graphics_getShaderSource(uint shaderId);

        
        [DllImport(OSDetector.GraphicsDLL)]
        internal static extern void graphics_detachShaderFromProgram(uint program, uint shader);
        
        [DllImport(OSDetector.GraphicsDLL)]
        internal static extern void graphics_attachShaderToProgram(uint program, uint shader);

        
        [DllImport(OSDetector.GraphicsDLL)]
        internal static extern uint graphics_createShaderProgram();
        [DllImport(OSDetector.GraphicsDLL)]
        internal static extern void graphics_compileShaderProgram(uint program);

        [DllImport(OSDetector.GraphicsDLL)]
        internal static extern void graphics_destroyShaderProgram(uint program);
        
        [DllImport(OSDetector.GraphicsDLL)]
        internal static extern uint graphics_loadShaderProgram(uint[] shaders, uint count);


        [DllImport(OSDetector.GraphicsDLL)]
        internal static extern void graphics_setShaderProgram(uint program);

        [DllImport(OSDetector.GraphicsDLL)]
        internal static extern uint graphics_getShaderProgram();


        [DllImport(OSDetector.GraphicsDLL)]
        internal static extern string testA(uint a);

        [DllImport(OSDetector.GraphicsDLL)]
        internal static extern bool testB(uint a);


        [DllImport(OSDetector.GraphicsDLL)]
        internal static extern void graphics_setBackground(float r, float g, float b, float a);

        [DllImport(OSDetector.GraphicsDLL)]
        internal static extern void graphics_getBackground(ref float r, ref float g, ref float b, ref float a);

        [DllImport(OSDetector.GraphicsDLL)]
        internal static extern void graphics_clear(uint type);

    }

    public interface IDrawable
    {
        void Draw();
    }

    public class Mesh<T> : IDrawable, IDisposable where T : struct
    {
        private bool disposedValue;

        public VBO<T> Buffer { get; }
        public ShaderProgram Program { get; set; }

        public Mesh(IMeshGenerator<T> generator)
        {
            Buffer = generator.GetBuffer();
            Program = generator.GetProgram();
        }
        public Mesh(VBO<T> vbo, ShaderProgram program)
        {
            Buffer = vbo;
            Program = program;
        }
        public Mesh()
        {
            Buffer = new VBO<T>();
        }

        public void LoadVertices(params T[] data)
        {
            Buffer.SetData(data);
        }

        public static Mesh<T> FromMeshGenerator(IMeshGenerator<T> generator)
        {
            return new Mesh<T>(generator);
        }

        public void Draw()
        {
            Program?.Use();
            Buffer.TempDraw();
        }

        #region Dispose
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Buffer.Dispose();
                    Program.Dispose();
                }

                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
    public interface IMeshGenerator<T> where T : struct
    {
        VBO<T> GetBuffer();
        ShaderProgram GetProgram();
    }

    public class Scene: IDrawable, IDisposable
    {
        private bool disposedValue;

        public void Draw()
        {
             
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                }

                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

    public class Graphics
    {
        private Window attachedWindow;

        public Point4 BackgroundColor {
            get {
                float r = 0, g = 0, b = 0, a = 0;

                LLGraphics.graphics_getBackground(ref r, ref g, ref b, ref a);

                return new Point4(r, g, b, a);
            }
            set {
                LLGraphics.graphics_setBackground(value.X, value.Y, value.Z, value.W);
            }
        }

        public void Clear()
        {
            attachedWindow.Use();
            LLGraphics.graphics_clear(0x00004000);
        }
        public void SwapBuffers()
        {
            attachedWindow.Use();
            LLGraphics.graphics_swapBuffers();
        }

        public Graphics(Window window)
        {
            attachedWindow = window;
        }
    }
}
