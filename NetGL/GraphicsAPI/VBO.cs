using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGL.GraphicsAPI
{
    public class VBO<T>: IBuffer<T> where T : struct
    {
        private uint ArrayID;
        public uint ID { get; }
        private bool disposedValue;

        private uint structSize = 0;

        public uint Length { get; private set; } = 0;
        public uint UnpackedLength { get; private set; } = 0;
        public uint ByteLength { get; private set; } = 0;
        private uint target = (uint)BufferTarget.ArrayBuffer;

        public VBO(ShaderProgram program)
        {
            ArrayID = LLGraphics.graphics_createVAO();
            ID = LLGraphics.graphics_createBuffer(target);

            LLGraphics.graphics_setVAO(ArrayID);
            LLGraphics.graphics_setBuffer(target, ID);
            var arrId = LLGraphics.graphics_createNativeArray(0);
            LLGraphics.graphics_setBufferData(target, 0, arrId, (uint)UsageHint.DynamicDraw);
            LLGraphics.graphics_destroyNativeArray(arrId);

            structSize = typeof(T).GetBufferElementSize();

            ApplyAttribPointers(typeof(T).ExtractAttribPointerMap(program));

        }

        public void Use()
        {
            LLGraphics.graphics_setVAO(ArrayID);
            LLGraphics.graphics_setBuffer((uint)BufferTarget.ArrayBuffer, ID);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                LLGraphics.graphics_destroyVAO(ArrayID);
                LLGraphics.graphics_destroyBuffer(ID);
                disposedValue = true;
            }
        }

        ~VBO()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Broken
        /// </summary>
        /// <returns></returns>
        private T[] GetData()
        {
            // TODO: Fix me

            var data = new object[UnpackedLength];
            LLGraphics.graphics_setVAO(ArrayID);
            LLGraphics.graphics_setBuffer(target, ID);
            LLGraphics.graphics_getBufferData(target, UnpackedLength, data);

            foreach (var item in data) {
                Console.WriteLine((float)item);
            }

            var elementSize = UnpackedLength / Length;

            var elementPacks = new object[Length][];

            for (uint i = 0; i < Length; i++)
            {
                elementPacks[i] = new object[elementSize];

                for (uint j = 0; j < elementSize; j++)
                {
                    elementPacks[i][j] = data[i * elementSize + j];
                }
            }

            return elementPacks
                .Select(pack => BufferElementPacker<T>.Pack(pack))
                .ToArray();
        }
        public void SetData(T[] data)
        {
            var size = structSize * (uint)data.Length;

            var rawData = data.SelectMany(v => BufferElementPacker<T>.Unpack(v)).ToArray();

            var dataId = LLGraphics.graphics_loadNativeArray(
                NativeArrayElement.GetNativeArray(rawData),
                (uint)rawData.Length);

            LLGraphics.graphics_setBuffer(target, ID);
            LLGraphics.graphics_setBufferData(target, size,
                dataId,
                (uint)UsageHint.DynamicDraw
            );
            LLGraphics.graphics_destroyNativeArray(dataId);

            ByteLength = size;
            Length = (uint)data.Length;
            UnpackedLength = (uint)rawData.Length;
        }

        private void ApplyAttribPointers(Dictionary<uint, AttribPointer> pointers)
        {
            var size = (uint)pointers.Sum(v => v.Value.Size * v.Value.Type.GetSize());

            uint stride = 0;

            LLGraphics.graphics_clearBufferAttributes(ArrayID);
            foreach (var pointer in pointers)
            {
                LLGraphics.graphics_createBufferAttribute(
                    ArrayID, pointer.Key, pointer.Value.Size, (uint)pointer.Value.Type, size, stride
                );

                stride += pointer.Value.Size * pointer.Value.Type.GetSize();
            }
        }

        public void Draw(GraphicsPrimitive primitiveType)
        {
            LLGraphics.graphics_drawBuffer(ArrayID, ID, Length, (uint)primitiveType);
        }
    }
    public class EBO: IBuffer<uint>
    {
        private bool disposedValue;

        public uint ID { get; }
        public uint Length { get; private set; }
        public GraphicsPrimitive Primitive { get; set; }

        public EBO(GraphicsPrimitive primitive = GraphicsPrimitive.Triangles)
        {
            ID = LLGraphics.graphics_createBuffer((uint)BufferTarget.ElementArrayBuffer);
            Primitive = primitive;
        }

        public void SetData(uint[] data)
        {
            var size = sizeof(uint) * (uint)data.Length;

            var rawData = data;

            var dataId = LLGraphics.graphics_loadNativeArray(
                NativeArrayElement.GetNativeArray(rawData.Select(v => (object)v).ToArray()),
                (uint)rawData.Length);

            LLGraphics.graphics_setBuffer((uint)BufferTarget.ElementArrayBuffer, ID);
            LLGraphics.graphics_setBufferData((uint)BufferTarget.ElementArrayBuffer, size,
                dataId,
                (uint)UsageHint.DynamicDraw
            );
            LLGraphics.graphics_destroyNativeArray(dataId);

            Length = (uint)data.Length;
        }

        public void Use()
        {
            LLGraphics.graphics_setBuffer((uint)BufferTarget.ElementArrayBuffer, ID);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue) {
                if (disposing) {
                    LLGraphics.graphics_destroyBuffer(ID);
                }

                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void Draw<T>(VBO<T> vbo) where T : struct
        {
            vbo.Use();

            uint size = 0;

            switch (Primitive) {
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
            Use();

            LLGraphics.graphics_drawElement((uint)Primitive, Length / size, Length, ID, vbo.ID);
        }
    }
}
