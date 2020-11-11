using System;

namespace NetGL.GraphicsAPI
{
    /// <summary>
    /// Ignores a property of a vertice struct
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class IgonrePropertyAttribute: Attribute
    {
        public IgonrePropertyAttribute()
        {
        }
    }
}
