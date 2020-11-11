using System;

namespace NetGL.GraphicsAPI
{
    public class ShaderSyntaxException: Exception
    {
        public ShaderSyntaxException(string log) : base(log) { }
    }
}
