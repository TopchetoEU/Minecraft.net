using System;
using System.Collections.Generic;
using System.Linq;

namespace NetGL.GraphicsAPI
{
    public class VBO<T>: IBuffer<T> where T : struct
    {
        private uint ArrayID;
        private uint BufferID;
        private bool disposedValue;

        private uint structSize = 0;

        private Type LastType;

        public uint Length { get; private set; } = 0;
        public uint UnpackedLength { get; private set; } = 0;
        public uint ByteLength { get; private set; } = 0;
        private uint target = (uint)BufferTarget.ArrayBuffer;

        public VBO()
        {
            LLGraphics.graphics_init();
            ArrayID = LLGraphics.graphics_createVAO();
            BufferID = LLGraphics.graphics_createBuffer(target);

            LLGraphics.graphics_setVAO(ArrayID);
            LLGraphics.graphics_setBuffer(target, BufferID);
            var arrId = LLGraphics.graphics_createNativeArray(0);
            LLGraphics.graphics_setBufferData(target, 0, arrId, (uint)UsageHint.DynamicDraw);
            LLGraphics.graphics_destroyNativeArray(arrId);

            structSize = typeof(T).GetBufferElementSize();

            ApplyAttribPointers(typeof(T).ExtractAttribPointerMap());

        }

        public void Use()
        {
            LLGraphics.graphics_setBuffer((uint)BufferTarget.ArrayBuffer, ArrayID);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                LLGraphics.graphics_destroyVAO(ArrayID);
                LLGraphics.graphics_destroyBuffer(BufferID);
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
            LLGraphics.graphics_setBuffer(target, BufferID);
            LLGraphics.graphics_getBufferData(target, UnpackedLength, data);

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

            LLGraphics.graphics_setVAO(ArrayID);
            LLGraphics.graphics_setBufferData(target, size,
                dataId,
                (uint)UsageHint.DynamicDraw
            );
            LLGraphics.graphics_destroyNativeArray(dataId);

            ByteLength = size;
            Length = (uint)data.Length;
            UnpackedLength = (uint)rawData.Length;
            LastType = typeof(T);
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

        public void TempDraw()
        {
            LLGraphics.graphics_drawBuffer(ArrayID, BufferID, Length, (uint)GraphicsPrimitive.Triangles);
        }
    }
}
