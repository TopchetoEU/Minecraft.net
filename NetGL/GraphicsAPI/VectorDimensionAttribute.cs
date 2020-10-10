using System;
using System.Collections.Generic;
using System.Text;

namespace NetGL.GraphicsAPI
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class VectorDimensionAttribute: Attribute
    {
        public uint DimensionId { get; }
        public VectorDimensionAttribute(uint dimensionId)
        {
            DimensionId = dimensionId;
        }
    }
}
