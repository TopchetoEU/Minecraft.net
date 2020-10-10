using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace NetGL.GraphicsAPI
{
    [StructLayout(LayoutKind.Sequential)]
    internal unsafe struct NativeArrayElement
    {
        public uint Type { get; set; }
        public void* Value { get; set; }

        public NativeArrayElement(uint type, IntPtr val)
        {
            Type = type;
            Value = val.ToPointer();
        }

        public unsafe static NativeArrayElement[] GetNativeArray(params object[] array)
        {
            return array
                .Where(item => item.GetType().ToGraphicsType().TryStruct())
                .Select(item =>
                {
                    var ptr = GCHandle.Alloc(item, GCHandleType.Pinned).AddrOfPinnedObject();

                    return new NativeArrayElement((uint)item.GetType().ToGraphicsType(), ptr);
                }).ToArray();
        }
    }
}
