using OpenTK;

namespace MinecraftNetWindow.Geometry
{
    public class Vector4Uniform: IUniform
    {
        public Vector4Uniform(string name, Vector4 value)
        {
            Name = name;
            Value = value;
        }
        public string Name { get; set; }
        public Vector4 Value { get; set; }

        public void ApplyToShader(ShaderProgram shader) => shader.SetUniform(Name, Value.X, Value.Y, Value.Z, Value.W);
    }
}
