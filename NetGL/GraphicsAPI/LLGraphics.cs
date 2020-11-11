using System.ComponentModel;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

namespace NetGL.GraphicsAPI
{
    internal static partial class LLGraphics
    {

        [DllImport(OSDetector.GraphicsDLL)] public static extern uint graphics_createVAO();
        [DllImport(OSDetector.GraphicsDLL)] public static extern void graphics_destroyVAO(uint vaoId);
        [DllImport(OSDetector.GraphicsDLL)] public static extern uint graphics_createBuffer(uint target);
        [DllImport(OSDetector.GraphicsDLL)] public static extern void graphics_destroyBuffer(uint buff);
        [DllImport(OSDetector.GraphicsDLL)] public static extern void graphics_setBuffer(uint target, uint buff);
        [DllImport(OSDetector.GraphicsDLL)] public static extern uint graphics_getBuffer(uint target);
        [DllImport(OSDetector.GraphicsDLL)] public static extern void graphics_setVAO(uint vaoId);
        [DllImport(OSDetector.GraphicsDLL)] public static extern uint graphics_getVAO();
        [DllImport(OSDetector.GraphicsDLL)] public unsafe static extern void graphics_setBufferData(uint target, uint size, uint dataArr, uint usage);
        [DllImport(OSDetector.GraphicsDLL)] public static extern void graphics_getBufferData(uint target, uint size, object[] data);

        [DllImport(OSDetector.GraphicsDLL)] public static extern void graphics_clearBufferAttributes(uint buff);
        [DllImport(OSDetector.GraphicsDLL)] public static extern void graphics_createBufferAttribute(uint buff, uint attrId, uint byteSize, uint type, uint stride, uint offset);
        [DllImport(OSDetector.GraphicsDLL)] public static extern void graphics_destroyBufferAttribute(uint buff, uint attrId);

        [DllImport(OSDetector.GraphicsDLL)] public static extern void graphics_drawElement(uint indicieType, uint indicieCount, uint indicieEdges, uint ebo, uint vbo);
        [DllImport(OSDetector.GraphicsDLL)] public static extern void graphics_drawBuffer(uint vaoId, uint buff, uint amount, uint mode);


        [DllImport(OSDetector.GraphicsDLL)] internal static extern uint graphics_loadNativeArray(NativeArrayElement[] arr, uint size);
        [DllImport(OSDetector.GraphicsDLL)] internal static extern uint graphics_createNativeArray(uint size);
        [DllImport(OSDetector.GraphicsDLL)] internal static extern void graphics_destroyNativeArray(uint arr);



        [DllImport(OSDetector.GraphicsDLL)] public static extern uint graphics_createShader(uint type, string raw, int length);

        [DllImport(OSDetector.GraphicsDLL)] public static extern void graphics_destroyShader(uint shader);


        [DllImport(OSDetector.GraphicsDLL)] public static extern bool graphics_getShaderBuildSuccess(uint shaderId);

        [DllImport(OSDetector.GraphicsDLL)] public static extern string graphics_getShaderInfoLog(uint shaderId);

        [DllImport(OSDetector.GraphicsDLL)] public static extern string graphics_getShaderSource(uint shaderId);


        [DllImport(OSDetector.GraphicsDLL)] public static extern void graphics_detachShaderFromProgram(uint program, uint shader);

        [DllImport(OSDetector.GraphicsDLL)] public static extern void graphics_attachShaderToProgram(uint program, uint shader);


        [DllImport(OSDetector.GraphicsDLL)] public static extern uint graphics_createShaderProgram();
        [DllImport(OSDetector.GraphicsDLL)] public static extern void graphics_compileShaderProgram(uint program);

        [DllImport(OSDetector.GraphicsDLL)] public static extern void graphics_destroyShaderProgram(uint program);

        [DllImport(OSDetector.GraphicsDLL)] public static extern uint graphics_loadShaderProgram(uint[] shaders, uint count);


        [DllImport(OSDetector.GraphicsDLL)] public static extern void graphics_setShaderProgram(uint program);

        [DllImport(OSDetector.GraphicsDLL)] public static extern uint graphics_getShaderProgram();


        [DllImport(OSDetector.GraphicsDLL)] public static extern void graphics_setBackground(float r, float g, float b, float a);

        [DllImport(OSDetector.GraphicsDLL)] public static extern void graphics_getBackground(ref float r, ref float g, ref float b, ref float a);

        [DllImport(OSDetector.GraphicsDLL)] public static extern void graphics_clear(uint type);

        [DllImport(OSDetector.GraphicsDLL)] public static extern uint graphics_createTexture();
        [DllImport(OSDetector.GraphicsDLL)] public static extern void graphics_destroyTexture(uint id);
        [DllImport(OSDetector.GraphicsDLL)] public static extern uint graphics_getTexture(uint target);
        [DllImport(OSDetector.GraphicsDLL)] public static extern void graphics_setTexture(uint id, uint target);
        [DllImport(OSDetector.GraphicsDLL)] public static extern void graphics_setTexture1DData(
            uint target, uint length, uint textureFormat, uint dataType, uint dataArrId
        );
        [DllImport(OSDetector.GraphicsDLL)] public static extern void graphics_setTexture2DData(
            uint target, uint width, uint height, uint textureFormat, uint dataType, uint dataArrId
        );
        [DllImport(OSDetector.GraphicsDLL)] public static extern void graphics_setTexture3DData(
            uint target, uint width, uint height, uint depth, uint textureFormat, uint dataType, uint dataArrId
        );
        [DllImport(OSDetector.GraphicsDLL)] internal static extern void graphics_setUniformTex2(
            uint id, uint uniformId
        );
        [DllImport(OSDetector.GraphicsDLL)] internal static extern void graphics_setTextureParameter(
            uint target, uint paramName, int value
        );

        [DllImport(OSDetector.GraphicsDLL)] internal static extern void graphics_setRenderOption(
            uint option, bool enabled
        );
        [DllImport(OSDetector.GraphicsDLL)] internal static extern void graphics_setAlphaTestOptions(
            uint func, float val
        );
        [DllImport(OSDetector.GraphicsDLL)] internal static extern void graphics_setDepthTestOptions(
            uint func
        );
        [DllImport(OSDetector.GraphicsDLL)] internal static extern void graphics_setBlendingOptions(
            uint sourceFunc, uint destFunc
        );
        [DllImport(OSDetector.GraphicsDLL)] internal static extern void graphics_setStencilOptions(
            uint func, int refrence, uint mask
        );

        [DllImport(OSDetector.GraphicsDLL)] public static extern uint graphics_getUniformLocation(uint program, string name);
        [DllImport(OSDetector.GraphicsDLL)] public static extern uint graphics_getAttribLocation(uint program, string name);
    }
}
