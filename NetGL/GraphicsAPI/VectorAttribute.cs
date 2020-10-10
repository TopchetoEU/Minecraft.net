using System;

namespace NetGL.GraphicsAPI
{
    [AttributeUsage(AttributeTargets.Struct, Inherited = false, AllowMultiple = true)]
    public sealed class VectorAttribute: Attribute
    {
        public uint Dimensions { get; }
        public GraphicsType Type { get; }

        public VectorAttribute(uint dimensions, GraphicsType type)
        {
            if (dimensions < 2 && dimensions > 4) throw new Exception("Dimensions can be between 2 and 4");
            Dimensions = dimensions;
            Type = type;
        }

        internal AttribPointer ToAttribPointer() => new AttribPointer(Dimensions, Type);
    }
}
