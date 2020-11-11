using System;

namespace NetGL.GraphicsAPI
{
    [AttributeUsage(AttributeTargets.Struct, Inherited = false, AllowMultiple = true)]
    internal sealed class MatrixAttribute: Attribute
    {
        public uint Width { get; }
        public uint Height { get; }
        public MultiDimensionType Type { get; }

        public MatrixAttribute(uint width, uint height, MultiDimensionType type)
        {
            if (width < 2 && width > 4) throw new Exception("Width can be between 2 and 4");
            if (height < 2 && height > 4) throw new Exception("Height can be between 2 and 4");
            Width = width;
            Height = height;
            Type = type;
        }
    }
}
